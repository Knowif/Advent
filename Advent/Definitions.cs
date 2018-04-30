using System.Collections.Generic;
using System.Linq;
using static Advent.Interactive;

namespace Advent
{
	enum Direction
	{
		N, E, S, W, NE, NW, SE, SW, Up, Down
	}

	public enum HandleResult
	{
		FullManaged, Continue, Refused
	}

	class AObject
	{
		public static readonly AObject NoDescObject = new AObject();
		public delegate HandleResult Hanadler(AObject self, PlayerVariables v);
		public delegate HandleResult ObjHanadler(AObject self, PlayerVariables v, AObject obj);

		// general properties
		public readonly string Name;
		public readonly string[] Alias;
		public bool IsTakable = false;
		public bool IsFar = false;
		public string Information = "";
		public string Description = "";
		public string LightDescription = "";

		// sub-object
		public List<AObject> SubObjects = new List<AObject>();
		public AObject Parent;

		// attributes
		public bool Openable = false;
		public bool IsOpen = false;
		public bool CanOnOff = false;
		public bool IsOn = false;
		public bool CanPutOn = false;

		// events

		public ObjHanadler OnPuttingOn   = (self, v, p) => HandleResult.Continue;
		public Hanadler BeforeExaminaion	= (self, v) => HandleResult.Continue;
		public Hanadler PostExamination		= (self, v) => HandleResult.Continue;
		public Hanadler BeforeOpening		= (self, v) => HandleResult.Continue;
		public Hanadler PostOpening			= (self, v) => HandleResult.Continue;
		public Hanadler BeforeClosing		= (self, v) => HandleResult.Continue;
		public Hanadler PostClosing			= (self, v) => HandleResult.Continue;
		public Hanadler BeforeTurningOn		= (self, v) => HandleResult.Continue;
		public Hanadler PostTurningOn		= (self, v) => HandleResult.Continue;
		public Hanadler BeforeTurningOff	= (self, v) => HandleResult.Continue;
		public Hanadler PostTurningOff		= (self, v) => HandleResult.Continue;


		public AObject(string name, string[] alias, string desc, string ldesc = "", string info = "")
		{
			Name = name; Alias = alias; Information = info; Description = desc;
			if (ldesc == "") LightDescription = desc; else LightDescription = ldesc;
			if (info == "") IsTakable = false;
		}


		public AObject(AObject obj)
		{
			Name = obj.Name;
			Alias = obj.Alias;
			IsTakable = obj.IsTakable;
			IsFar = obj.IsFar;
			Information = obj.Information;
			Description = obj.Description;
			LightDescription = obj.LightDescription;
			SubObjects = obj.SubObjects;
			Parent = obj.Parent;
			Openable = obj.Openable;
			IsOpen = obj.IsOpen;
			CanOnOff = obj.CanOnOff;
			IsOn = obj.IsOn;
			CanPutOn = obj.CanPutOn;
			OnPuttingOn = obj.OnPuttingOn;
			BeforeExaminaion = obj.BeforeExaminaion;
			PostExamination = obj.PostExamination;
			BeforeOpening = obj.BeforeOpening;
			PostOpening = obj.PostOpening;
			BeforeClosing = obj.BeforeClosing;
			PostClosing = obj.PostClosing;
			BeforeTurningOn = obj.BeforeTurningOn;
			PostTurningOn = obj.PostTurningOn;
			BeforeTurningOff = obj.BeforeTurningOff;
			PostTurningOff = obj.PostTurningOff;
		}

		private AObject() { }
	}

	class Doorway
	{
		public delegate HandleResult Hanadler(Doorway self, PlayerVariables v);

		public Room ConnectTo;

		// door
		public readonly bool IsDoor = false;
		public readonly string DoorName;
		public readonly string[] Alias; 
		public string Description;
		public bool IsOpen = false;
		public bool IsLocked = false;

		// way
		public Direction Direction;

		// events
		public Hanadler BeforeOpening		= (self, v) => HandleResult.Continue;
		public Hanadler PostOpening			= (self, v) => HandleResult.Continue;
		public Hanadler BeforeClosing		= (self, v) => HandleResult.Continue;
		public Hanadler PostClosing			= (self, v) => HandleResult.Continue;
		public Hanadler BeforeGoingIn		= (self, v) => HandleResult.Continue;

		public Doorway(Room dest, Direction dir)
		{
			IsDoor = false; ConnectTo = dest; Direction = dir; IsOpen = true;
		}

		public Doorway(Room dest, string name, string[] alias, 
						string desc, bool open = false, bool locked = false)
		{
			IsDoor = true; ConnectTo = dest; DoorName = name; Alias = alias;
			Description = desc; IsOpen = open; IsLocked = locked;
		}
	}

	class Area
	{
		public delegate HandleResult Hanadler(Area self, PlayerVariables v);
		public delegate HandleResult ObjHanadler(Area self, PlayerVariables v, AObject obj);
		public delegate HandleResult CmdHanadler(Area self, PlayerVariables v, string obj);

		public string Name;
		public Hanadler OnDescription			= (self, v) => HandleResult.Continue;
		public Hanadler OnExamination			= (self, v) => HandleResult.Continue;
		public CmdHanadler OnQueryingObject		= (self, v, p) => HandleResult.Continue;
		public CmdHanadler BeforeCommand		= (self, v, p) => HandleResult.Continue;
		public CmdHanadler PostCommand			= (self, v, p) => HandleResult.Continue;

		public Area(string name) { Name = name; }
	}

	class Room
	{
		public delegate string Descriptor(Room self, PlayerVariables v);
		public delegate HandleResult CmdHanadler(Room self, PlayerVariables v, string p);
		public delegate HandleResult ObjHanadler(Room self, PlayerVariables v, AObject obj);

		public string Name;
		public string[] Alias;
		public string Description;
		public string LightDescription;

		public Area CurrentArea;
		public bool IsLight;
		public bool IsPlayerLight = false;
		public bool isWarm = true;

		public List<AObject> Objects = new List<AObject>();
		public List<Doorway> Doorways = new List<Doorway>();
		public List<Area> Areas = new List<Area>();
		public List<string> NoDescObjects = new List<string>();

		public Descriptor  GetDescription		= (self, v) => 
			(self.IsLight || self.IsPlayerLight) ? self.LightDescription : self.Description;
		public Descriptor  PostDescription		= (self, v) => "";

		public CmdHanadler BeforeCommand		= (self, v, p) => HandleResult.Continue;
		public CmdHanadler PostCommand			= (self, v, p) => HandleResult.Continue;

		
		public ObjHanadler BeforeExaminaion		= (self, v, p) => HandleResult.Continue;
		public ObjHanadler PostExamination		= (self, v, p) => HandleResult.Continue;
		public ObjHanadler BeforeOpening		= (self, v, p) => HandleResult.Continue;
		public ObjHanadler PostOpening			= (self, v, p) => HandleResult.Continue;
		public ObjHanadler BeforeClosing		= (self, v, p) => HandleResult.Continue;
		public ObjHanadler PostClosing			= (self, v, p) => HandleResult.Continue;
		public ObjHanadler BeforeTurningOn		= (self, v, p) => HandleResult.Continue;
		public ObjHanadler PostTurningOn		= (self, v, p) => HandleResult.Continue;
		public ObjHanadler BeforeTurningOff		= (self, v, p) => HandleResult.Continue;
		public ObjHanadler PostTurningOff		= (self, v, p) => HandleResult.Continue;

		public Doorway GetDoorway(string name)
		{
			List<Doorway> matches = new List<Doorway>();
			foreach (var obj in Doorways)
			{
				if (obj.IsDoor)
					if (obj.DoorName == name || obj.Alias.Contains(name) || (obj.ConnectTo != null &&
							(obj.ConnectTo.Name == name || obj.ConnectTo.Alias.Contains(name))))
						matches.Add(obj);
			}

			if (matches.Count > 1)
			{
				Print("你所说的“" + name + "”引起了歧义，它可以指：\n");
				foreach (var match in matches)
					Print(" * " + match.DoorName + "\n");
			} else if (matches.Count == 1)
				return matches[0];

			return null;
		}

		public Doorway GetDoorway(Direction dir)
		{
			foreach (var obj in Doorways)
				if (!obj.IsDoor && obj.Direction == dir)
					return obj;
			return null;
		}

		public AObject GetObject(string name, PlayerVariables v)
		{
			if (CurrentArea == null || CurrentArea.OnQueryingObject(CurrentArea, v, name) == HandleResult.Continue)
			{
				if (NoDescObjects.Contains(name)) return AObject.NoDescObject;

				List<AObject> matches = new List<AObject>();
				foreach (var obj in Objects)
				{
					if (obj.Name == name || obj.Alias.Contains(name))
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
					return matches[0];
			}
			return null;
		}
	}
}
