using System;
using System.Collections.Generic;
using System.Linq;
using static Advent.Interactive;

namespace Advent
{
	public enum Direction
	{
		E, S, W, N, NE, NW, SE, SW, Up, Down
	}

	public enum ObjectVisibility
	{
		Visible, NotVisible, Unclear
	}

	// When carrying out a command, the object's corresponding handler is often called to see if it handles the situation itself.
	public enum HandleResult
	{
		// Denotes that the object has responded to the command and no further processing is needed.
		FullManaged,
		// Denotes that while the object may have done additional processing, the default routine will still be executed.
		Continue,
		// Denotes that the object has treated the command as invalid or as a mistake and no further processing is needed.
		Refused
	}

	public class AObject
	{
		// Default responses when examining, smelling, touching, etc.
		// Per-room overridable defaulting mechanisms can also be designed, but this seems enough. 
		public static string DefaultDescription { get; set; }  = "你没有看到任何特殊之处。";
		public static string DefaultSensoryResponse { get; set; }  = "你没有感觉到任何特殊之处。";

		public delegate string Descriptor(AObject self, PlayerVariables v);
		public delegate HandleResult Handler(AObject self, PlayerVariables v);
		public static readonly Handler DefaultHandler = (self, v) => HandleResult.Continue;
		public static readonly Descriptor DefaultDescriptor = (self, v) => "";

		// general properties
		public string Name { get; set; }
		public List<string> Alias { get; private set; }
		public Descriptor ShortInfo { get; set; } = null;
		public Descriptor Information { get; set; } 
			= DefaultDescriptor; // shown when the object is seen in the room
		public Descriptor Description { get; set; }  
			= DefaultDescriptor; // shown when it is being EXAMINEd
		public Descriptor LightDescription  { get; set; } 
			= DefaultDescriptor; // the same, but in a lighted place

		// sub-objects
		public List<AObject> SubObjects { get; private set; }  = new List<AObject>();
		public AObject Parent { get; set; }

		// attributes

		// the parser uses default responses for nondescript objects
		public bool IsNondescript		{ get; set; } = false; 
		public bool IsTakable			{ get; set; } = false;
		public bool IsOpenable			{ get; set; } = false;
		public bool IsContainer			{ get; set; } = false;
		public bool IsSwitch			{ get; set; } = false;
		public bool IsClothing			{ get; set; } = false;
		// whether certain sensory cmds are available
		public bool IsReachable			{ get; set; } = true; 
		public bool IsClublikeWeapon	{ get; set; } = false;
		public bool IsBladeWeapon		{ get; set; } = false;
		// for takable objects and containers; the value is roughly in dm^3
		public float Size				{ get; set; } = -1;
		public float Capacity			{ get; set; } = -1;
		public float SubObjectsSize => SubObjects.Sum(x => x.Size == -1 ? 0 : x.Size);
		public Descriptor FullShortInfo { get; } = (s, v) =>
		{
			string ret = s.ShortInfo(s, v);
			if (s.IsOpenable)
			{
				ret += s.OpenState ? "（开着）" : "（关着）";
			}
			if (s.IsContainer)
			{
				var so = s.SubObjects.Where(x => x.IsTakable);
				if (v.IsLight() && so.Any())
					ret += $"（里面有{string.Join("、", so.Select(x => x.ShortInfo(x, v)))}）";
				else if (v.IsLight())
					ret += "（里面什么也没有）";
				else ret += "（你看不清楚里面装了什么）";
			}
			return ret.Replace("）（", "，");
		};

		// door-likes
		public bool IsEnterable			{ get; set; } = false;
		public bool IsLocked			{ get; set; } = false;
		public Room RoomTo				{ get; set; } = null;
		// On most occasions, when we initialize a door's LinkedSide the linked door hasn't even been declared. So despite its being slower, it is quite useful to leave this property as a lambda that evaluates later.
		public Func<AObject> LinkedSide { get; set; } = () => null;
		// Sometimes we intentionally make doors' LinkedSide inconsistent, e.g. a => b but b => c, and would like to have the tester skip these doors.
		public bool SkipConsistencyTest { get; set; } = false;

		// states
		public bool OpenState			{ get; set; } = false;
		public bool SwitchState			{ get; set; } = false;

		// events
		public Handler OnExaminaion		{ get; set; } = DefaultHandler;
		public Handler OnTaking			{ get; set; } = DefaultHandler;
		public Handler OnOpening		{ get; set; } = DefaultHandler;
		public Handler OnClosing		{ get; set; } = DefaultHandler;
		public Handler OnTurningOn		{ get; set; } = DefaultHandler;
		public Handler OnTurningOff		{ get; set; } = DefaultHandler;
		public Handler OnEntering		{ get; set; } = DefaultHandler;
		public Handler OnBeingSmelled	{ get; set; } = DefaultHandler;
		public Handler OnBeingTouched	{ get; set; } = DefaultHandler;
		public Handler OnBeingListened	{ get; set; } = DefaultHandler;

		public AObject(string name, string[] alias)
		{
			Name = name;
			Alias = alias.ToList();
			IsNondescript = true;
		}

		public AObject(string name, string[] alias, string desc, string ldesc = "", 
			string info = "", string sinfo = "")
		{
			Name = name;
			Alias = alias.ToList();
			Description = (s, v) => desc;
			if (!string.IsNullOrEmpty(info))
				Information = (s, v) => info;
			if (!string.IsNullOrEmpty(sinfo))
				ShortInfo = (s, v) => sinfo;
			if (string.IsNullOrEmpty(ldesc))
				LightDescription = (s, v) => desc;
			else
				LightDescription = (s, v) => ldesc;
		}

		public static AObject SimpleDoor(Room dest, string name, string[] alias,
			string desc = "", bool open = false, bool locked = false, 
			Func<AObject> flopside = null)
		{
			return new AObject
			{
				RoomTo				= dest,
				Name				= name,
				Alias				= alias.ToList(),
				Description			= (s, v) => desc,
				LightDescription	= (s, v) => desc,
				IsNondescript		= string.IsNullOrEmpty(desc),
				IsEnterable			= true,
				IsOpenable			= true,
				IsLocked			= locked,
				OpenState			= open,
				LinkedSide			= flopside ?? (() => null),
				Information			= (s, v) => $"{name}{(s.OpenState ? "敞开" : "关")}着。"
			};
		}

		// It appears that in c# you can only write a copy constructor like this.
		public AObject(AObject prototype)
		{
			if (prototype == null)
				throw new ArgumentNullException(nameof(prototype));
			Name = prototype.Name; 
			Alias = prototype.Alias; 
			IsTakable = prototype.IsTakable;
			Information = prototype.Information; 
			Description = prototype.Description;
			LightDescription = prototype.LightDescription; 
			SubObjects = prototype.SubObjects;
			Parent = prototype.Parent;

			IsOpenable = prototype.IsOpenable; 
			OpenState = prototype.OpenState; 
			IsSwitch = prototype.IsSwitch;
			SwitchState = prototype.SwitchState; 
			IsClothing = prototype.IsClothing;

			//OnPuttingOn = obj.OnPuttingOn; 
			OnExaminaion = prototype.OnExaminaion;
			OnOpening = prototype.OnOpening; 
			OnClosing = prototype.OnClosing;
			OnTurningOn = prototype.OnTurningOn; 
			OnTurningOff = prototype.OnTurningOff;
			OnBeingSmelled = prototype.OnBeingSmelled; 
			OnBeingTouched = prototype.OnBeingTouched;
			OnBeingListened = prototype.OnBeingListened;
		}

		private AObject() { }
	}

	public class Area
	{
		public delegate HandleResult Handler(Area self, PlayerVariables v);
		public delegate HandleResult CmdHandler(Area self, PlayerVariables v, string obj);
		public delegate HandleResult DirHandler(Area self, PlayerVariables v, Direction dir);
		public delegate string Descriptor(Area self, PlayerVariables v);
		public static readonly Handler DefaultHandler = (self, v) => HandleResult.Continue;
		public static readonly CmdHandler DefaultCmdHandler = (self, v, p) => HandleResult.Continue;
		public static readonly DirHandler DefaultDirHandler = (self, v, d) => HandleResult.Continue;

		public string Name { get; set; }

		// For rooms with areas, the area-ways usually replace the room-ways.
		// Leads to other rooms
		public Dictionary<Direction, Room> RoomTo { get; private set; } 
			= new Dictionary<Direction, Room>();
		// Leads to other areas in this room
		public Dictionary<Direction, Area> AreaTo { get; private set; } 
			= new Dictionary<Direction, Area>();
				
		public AObject DefaultDoor  { get; set; }
		public Direction? DefaultOutWay  { get; set; }

		public Handler OnExamination			{ get; set; } = DefaultHandler;
		public CmdHandler BeforeCommand			{ get; set; } = DefaultCmdHandler;
		public CmdHandler PostCommand			{ get; set; } = DefaultCmdHandler;
		public DirHandler OnGoDirection			{ get; set; } = DefaultDirHandler;
		public Descriptor OverrideDescription	{ get; set; } = (s, v) => "";

		public Func<AObject, ObjectVisibility> FilterObject { get; set; } 
			= (x) => ObjectVisibility.Visible;

		public Area(string name) { Name = name; }
		public Area() { }
	}

	public class Room
	{
		private static readonly CmdHandler DefaultCHandler = 
			(self, v, p) => HandleResult.Continue;
		private static readonly ObjHandler DefaultOHandler = 
			(self, v, p) => HandleResult.Continue;

		public static string DefaultListenResponse { get; set; } = "这里很安静。";
		public static string DefaultSensoryResponse { get; set; } = "你并未感觉到空气有任何特殊之处。";

		public delegate string Descriptor(Room self, PlayerVariables v);
		public delegate HandleResult CmdHandler(Room self, PlayerVariables v, string p);
		public delegate HandleResult ObjHandler(Room self, PlayerVariables v, AObject obj);

		// general properties
		public string Name { get; set; }
		public List<string> Alias { get; private set; } = new List<string>();
		public string Description { get; set; }
		public string LightDescription { get; set; }

		// attributes
		public bool IsLit { get; set; } = false;
		public bool IsPlayerLit { get; set; } = false;
		public bool IsWarm { get; set; } = true;
		public AObject DefaultDoor { get; set; }
		public Direction? DefaultOutWay { get; set; }
		public Dictionary<Direction, Room> RoomTo { get; private set; } 
			= new Dictionary<Direction, Room>();

		// states
		public Area CurrentArea { get; set; }

		// objects
		public List<AObject> Objects { get; private set; } = new List<AObject>();
		public List<Area> Areas { get; private set; } = new List<Area>();

		public Descriptor GetDescription	{ get; set; } = (self, v) =>
			((self.IsLit || self.IsPlayerLit) ? self.LightDescription : self.Description);
		public Descriptor PostDescription	{ get; set; } = (self, v) => "";

		public CmdHandler BeforeCommand		{ get; set; } = DefaultCHandler;
		public CmdHandler PostCommand		{ get; set; } = DefaultCHandler;
		public CmdHandler OnBeingSmelled	{ get; set; } = DefaultCHandler;
		public CmdHandler OnListen			{ get; set; } = DefaultCHandler;

		public ObjHandler BeforeExaminaion	{ get; set; } = DefaultOHandler;
		public ObjHandler PostExamination	{ get; set; } = DefaultOHandler;
		public ObjHandler BeforeOpening		{ get; set; } = DefaultOHandler;
		public ObjHandler PostOpening		{ get; set; } = DefaultOHandler;
		public ObjHandler BeforeClosing		{ get; set; } = DefaultOHandler;
		public ObjHandler PostClosing		{ get; set; } = DefaultOHandler;
		public ObjHandler BeforeTurningOn	{ get; set; } = DefaultOHandler;
		public ObjHandler PostTurningOn		{ get; set; } = DefaultOHandler;
		public ObjHandler BeforeTurningOff	{ get; set; } = DefaultOHandler;
		public ObjHandler PostTurningOff	{ get; set; } = DefaultOHandler;

		private void FindSubObjectIn(AObject obj, string name, List<AObject> matches)
		{
			if (obj.IsOpenable && !obj.OpenState) return;
			foreach (var subObj in obj.SubObjects)
			{
				if (subObj.Name == name || subObj.Alias.Contains(name))
					matches.Add(subObj);
				FindSubObjectIn(subObj, name, matches);
			}
		}

		public AObject FindObject(string name, PlayerVariables v)
		{
			List<AObject> matches = new List<AObject>();
			foreach (var obj in Objects)
			{
				if (CurrentArea != null && CurrentArea.FilterObject(obj) == ObjectVisibility.NotVisible)
					continue;
				if (obj.Name == name || obj.Alias.Contains(name))
					matches.Add(obj);
				FindSubObjectIn(obj, name, matches);
			}

			if (matches.Count > 1)
			{
				Print("你所说的“" + name + "”引起了歧义，它可以指：\n");
				foreach (var match in matches)
					Print(" * " + match.Name + "\n");
			} else if (matches.Count == 1)
			{
				if (CurrentArea != null
						&& CurrentArea.FilterObject(matches[0]) == ObjectVisibility.Unclear)
					Print("你在远处只能隐约看见它的剪影。\n\n");
				else
					return matches[0];
			}
			return null;
		}

		public AObject FindObjectInternal(string name)
		{
			List<AObject> matches = new List<AObject>();
			foreach (var obj in Objects)
			{
				if (obj.Name == name || obj.Alias.Contains(name))
					matches.Add(obj);
				foreach (var subObj in obj.SubObjects)
					if (subObj.Name == name || subObj.Alias.Contains(name))
						matches.Add(subObj);
			}

			if (matches.Count == 1)
				return matches[0];

			throw new ArgumentException("FindObjectInternal failed");
		}

		public Area FindArea(string name)
			=> Areas.Find(x => x.Name == name) ?? throw new ArgumentException("invalid area");

		public void ChangeArea(string name)
			=> CurrentArea = FindArea(name);

		public void ReplaceObjectInternal(string s, AObject newObj)
			=> Objects[Objects.FindIndex((x) => x.Name == s)] = newObj;
	}
}
