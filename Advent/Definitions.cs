using System;
using System.Collections.Generic;
using System.Linq;
using static Advent.Interactive;

namespace Advent
{
	enum Direction
	{
		E, S, W, N, NE, NW, SE, SW, Up, Down
	}

	enum ObjectVisibility
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

	class AObject
	{
		// Default responses when examining, smelling, touching, etc.
		// Per-room overridable defaulting mechanisms can also be designed, but this seems enough. 
		public static string DefaultDescription = "你没有看到任何特殊之处。";
		public static string DefaultSensoryResponse = "你没有感觉到任何特殊之处。";
		
		public delegate string Descriptor(AObject self, PlayerVariables v);
		public delegate HandleResult Handler(AObject self, PlayerVariables v);
		public delegate HandleResult ObjHandler(AObject self, PlayerVariables v, AObject obj);
		public static readonly Handler DefaultHandler = (self, v) => HandleResult.Continue;
		public static readonly ObjHandler DefaultObjHandler = (self, v, p) => HandleResult.Continue;

		// general properties
		public string Name;
		public string[] Alias;
		public Descriptor Information = (s, v) => ""; // shown when the object is seen in the room
		public Descriptor Description = (s, v) => ""; // shown when it is being EXAMINEd
		public Descriptor LightDescription = (s, v) => ""; // the same, but in a lighted place

		// sub-objects
		public List<AObject> SubObjects = new List<AObject>();
		public AObject Parent;

		// attributes
		public bool IsNondescript = false; // the parser uses default responses for nondescript objects
		public bool IsTakable = false;
		public bool IsOpenable = false;
		public bool IsContainer = false;
		public bool IsSwitch = false;
		public bool IsClothing = false;
		public bool IsReachable = true; // whether certain sensory cmds are available
		public bool IsClublikeWeapon = false;
		public bool IsBladeWeapon = false;

		// door-likes
		public bool IsEnterable = false;
		public bool IsLocked = false;
		public Room RoomTo = null;
		// On most occasions, when we initialize a door's LinkedSide the linked door hasn't even been declared. So despite its being slower, it is quite useful to leave this property as a lambda that evaluates later.
		public Func<AObject> LinkedSide = () => null;

		// states
		public bool OpenState = false;
		public bool SwitchState = false;

		// events
		public ObjHandler OnPuttingOn	= DefaultObjHandler;
		public Handler OnExaminaion		= DefaultHandler;
		public Handler OnOpening		= DefaultHandler;
		public Handler OnClosing		= DefaultHandler;
		public Handler OnTurningOn		= DefaultHandler;
		public Handler OnTurningOff		= DefaultHandler;
		public Handler OnEntering		= DefaultHandler;
		public Handler OnBeingSmelled	= DefaultHandler;
		public Handler OnBeingTouched	= DefaultHandler;
		public Handler OnBeingListened	= DefaultHandler;

		public AObject(string name, string[] alias)
		{
			Name = name; 
			Alias = alias; 
			IsNondescript = true;
		}

		public AObject(string name, string[] alias, string desc, string ldesc = "", string info = "")
		{
			Name = name; 
			Alias = alias; 
			Information = (s, v) => info; 
			Description = (s, v) => desc;
			if (ldesc == "") 
				LightDescription = (s, v) => desc; 
			else 
				LightDescription = (s, v) => ldesc;
		}

		public static AObject SimpleDoor(Room dest, string name, string[] alias, 
			string desc = "", bool open = false, bool locked = false, Func<AObject> flopside = null)
		{
			return new AObject
			{
				RoomTo				= dest,
				Name				= name,
				Alias				= alias,
				Description			= (s, v) => desc,
				LightDescription	= (s, v) => desc,
				IsNondescript		= desc == "",
				IsEnterable			= true,
				IsOpenable			= true,
				IsLocked			= locked,
				OpenState			= open,
				LinkedSide			= flopside ?? (() => null),
				Information			= (s, v) => $"{name}{(s.OpenState ? "敞开" : "关")}着。"
			};
		}
		
		// It appears that in c# you can only write a copy constructor like this.
		public AObject(AObject obj)
		{
			Name = obj.Name; Alias = obj.Alias; IsTakable = obj.IsTakable;
			Information = obj.Information; Description = obj.Description; 
			LightDescription = obj.LightDescription; SubObjects = obj.SubObjects; 
			Parent = obj.Parent; 

			IsOpenable = obj.IsOpenable; OpenState = obj.OpenState; IsSwitch = obj.IsSwitch; 
			SwitchState = obj.SwitchState; IsClothing = obj.IsClothing;

			OnPuttingOn = obj.OnPuttingOn; OnExaminaion = obj.OnExaminaion;
			OnOpening = obj.OnOpening;OnClosing = obj.OnClosing;
			OnTurningOn = obj.OnTurningOn; OnTurningOff = obj.OnTurningOff;
			OnBeingSmelled = obj.OnBeingSmelled; OnBeingTouched = obj.OnBeingTouched;
			OnBeingListened = obj.OnBeingListened;
		}

		private AObject() { }
	}

	class Area
	{
		public delegate HandleResult Handler(Area self, PlayerVariables v);
		public delegate HandleResult CmdHandler(Area self, PlayerVariables v, string obj);
		public delegate HandleResult DirHandler(Area self, PlayerVariables v, Direction dir);
		public delegate string Descriptor(Area self, PlayerVariables v);
		public static readonly Handler DefaultHandler = (self, v) => HandleResult.Continue;
		public static readonly CmdHandler DefaultCmdHandler = (self, v, p) => HandleResult.Continue;
		public static readonly DirHandler DefaultDirHandler = (self, v, d) => HandleResult.Continue;

		public string Name;

		// For rooms with areas, the area-ways usually replace the room-ways.
		// Leads to other rooms
		public Dictionary<Direction, Room> RoomTo = new Dictionary<Direction, Room>();
		// Leads to other areas in this room
		public Dictionary<Direction, Area> AreaTo = new Dictionary<Direction, Area>();

		public Handler OnExamination			= DefaultHandler;
		public CmdHandler OnQueryingObject		= DefaultCmdHandler;
		public CmdHandler BeforeCommand			= DefaultCmdHandler;
		public CmdHandler PostCommand			= DefaultCmdHandler;
		public DirHandler OnGoDirection			= DefaultDirHandler;
		public Descriptor OverrideDescription	= (s, v) => "";
		
		public Func<AObject, ObjectVisibility> FilterObject = 
			(x) => ObjectVisibility.Visible;

		public Area(string name) { Name = name; }
		public Area() { }
	}

	class Room
	{
		public static string DefaultListenResponse = "这里很安静。";
		public static string DefaultSensoryResponse = "你并未感觉到空气有任何特殊之处。";

		public delegate string Descriptor(Room self, PlayerVariables v);
		public delegate HandleResult CmdHandler(Room self, PlayerVariables v, string p);
		public delegate HandleResult ObjHandler(Room self, PlayerVariables v, AObject obj);

		// general properties
		public string Name;
		public string[] Alias;
		public string Description;
		public string LightDescription;

		// attributes
		public bool IsLit;
		public bool IsPlayerLit = false;
		public bool IsWarm = true;
		public AObject DefaultDoor;
		public Direction? DefaultOutWay;
		public Dictionary<Direction, Room> RoomTo = new Dictionary<Direction, Room>();

		// states
		public Area CurrentArea;

		// objects
		public List<AObject> Objects = new List<AObject>();
		public List<Area> Areas = new List<Area>();

		public Descriptor  GetDescription		= (self, v) => 
			((self.IsLit || self.IsPlayerLit) ? self.LightDescription : self.Description) + "\n\n";
		public Descriptor  PostDescription		= (self, v) => "";

		public CmdHandler BeforeCommand			= (self, v, p) => HandleResult.Continue;
		public CmdHandler PostCommand			= (self, v, p) => HandleResult.Continue;
		public CmdHandler OnBeingSmelled        = (self, v, p) => HandleResult.Continue;
		public CmdHandler OnListen				= (self, v, p) => HandleResult.Continue;
				
		public ObjHandler BeforeExaminaion		= (self, v, p) => HandleResult.Continue;
		public ObjHandler PostExamination		= (self, v, p) => HandleResult.Continue;
		public ObjHandler BeforeOpening			= (self, v, p) => HandleResult.Continue;
		public ObjHandler PostOpening			= (self, v, p) => HandleResult.Continue;
		public ObjHandler BeforeClosing			= (self, v, p) => HandleResult.Continue;
		public ObjHandler PostClosing			= (self, v, p) => HandleResult.Continue;
		public ObjHandler BeforeTurningOn		= (self, v, p) => HandleResult.Continue;
		public ObjHandler PostTurningOn			= (self, v, p) => HandleResult.Continue;
		public ObjHandler BeforeTurningOff		= (self, v, p) => HandleResult.Continue;
		public ObjHandler PostTurningOff		= (self, v, p) => HandleResult.Continue;
				
		public AObject FindObject(string name, PlayerVariables v)
		{
			if (CurrentArea == null 
				|| CurrentArea.OnQueryingObject(CurrentArea, v, name) == HandleResult.Continue)
			{

				List<AObject> matches = new List<AObject>();
				foreach (var obj in Objects)
				{
					if ((obj.Name == name || obj.Alias.Contains(name)) && (CurrentArea == null 
							|| CurrentArea.FilterObject(obj) != ObjectVisibility.NotVisible))
						matches.Add(obj);
					foreach (var subObj in obj.SubObjects)
						if (subObj.Name == name || subObj.Alias.Contains(name))
							matches.Add(subObj);
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

			//throw new ArgumentException("FindObjectInternal failed");
			Print("** ERROR FindObjectInternal failed **\n\n");
			System.Diagnostics.Trace.Assert(false);

			return null;
		}

		public Area FindArea(string name) 
			=> Areas.Find(x => x.Name == name);
		
		public void SetObjectInternal(string s, AObject obj) 
			=> Objects[Objects.FindIndex((x) => x.Name == s)] = obj;
	}
}
