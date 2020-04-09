using System.Collections.Generic;
using System.Linq;
using static Advent.Interactive;

namespace Advent
{
	partial class GameMap
	{
		public static Room Campus = new Room();

		public static Room Dormitory12 = new Room();
		public static Room Restroom = new Room();
		public static Room Balcony = new Room();
		public static Room DormsHallway = new Room();
		public static Room LobbyNo8 = new Room();

		public static Room Building1F1 = new Room();
		//public static Room Building1F2 = new Room();
		//public static Room Building1F3 = new Room();
		//public static Room Building1F4 = new Room();
		//public static Room Building1F5 = new Room();
		//public static Room Building1F6 = new Room();

		public static Room DarknessRoom = new Room();
		public static Area DarknessArea = new Area();

		// we need a complete list of rooms, doors, areas and objects
		// when we save game files. Later on we restore eveything.
		public static Room[] Rooms = new Room[]
		{ 
			Dormitory12, Restroom, Balcony, 
			DormsHallway, LobbyNo8, Campus, Building1F1
		};
		public static List<Area> Areas = new List<Area>();
		public static List<AObject> Objects = new List<AObject>();

		static GameMap()
		{
			BuildCommons();
			BuildDarkness();
			BuildDormitory12();
			BuildRestroom();
			BuildBalcony();
			BuildDormsHallway();
			BuildLobbyNo8();
			BuildCampus();
			BuildBuilding1();

			// build list of everything
			foreach (Room r in Rooms)
			{
				Areas.AddRange(r.Areas);
				r.Objects.ForEach((x) => BuildObjList(x));
			}
		}

		private static void BuildObjList(AObject o)
		{
			Objects.Add(o);
			foreach (AObject oo in o.SubObjects)
				BuildObjList(oo);
		}

		private static void SetupArea(ref Area a, string name, string desc, 
			AObject[] usable, AObject[] notClear, Dictionary<Direction, Area> godir, 
			string ldesc = "", string[] noDesc = null)
		{
			a.Name = name;
			if (ldesc == "")
				a.OverrideDescription = (s, v) => desc;
			else
				a.OverrideDescription = (s, v) => v.IsLight() ? ldesc : desc;
			a.AreaTo = godir;
			a.FilterObject = (x) =>
			{
				if (notClear.Contains(x)) return ObjectVisibility.Unclear;
				if (usable.Contains(x)) return ObjectVisibility.Visible;
				return ObjectVisibility.NotVisible;
			};
		}

		private static Area MakeArea(string name, string desc, AObject[] usable,
			AObject[] notClear, Dictionary<Direction, Area> godir)
		{
			Area a = new Area();
			SetupArea(ref a, name, desc, usable, notClear, godir);
			return a;
		}

		private static void AddArea(Room r, string name, string desc, AObject[] usable,
			AObject[] notClear, Dictionary<Direction, Area> godir)
		{
			r.Areas.Add(MakeArea(name, desc, usable, notClear, godir));
		}

		public static void BuildCommons()
		{
			// reusable objects here
			
		}
	}
}
