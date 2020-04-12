using System.Collections.Generic;
using System.Linq;

namespace Advent
{
	public static partial class GameMap
	{
		public readonly static Room Campus = new Room();
		public readonly static Room Playground = new Room();

		public readonly static Room Dormitory12 = new Room();
		public readonly static Room Restroom = new Room();
		public readonly static Room Balcony = new Room();
		public readonly static Room DormsHallway = new Room();
		public readonly static Room LobbyNo8 = new Room();

		public readonly static Room Building1F1 = new Room();
		public readonly static Room Building1F2 = new Room();
		public readonly static Room Building1F3 = new Room();
		public readonly static Room Building1F4 = new Room();
		public readonly static Room Building1F5 = new Room();
		//public static Room Building1F6 = new Room();

		public readonly static Room DarknessRoom = new Room();
		public readonly static Area DarknessArea = new Area();

		// we need a complete list of rooms, doors, areas and objects
		// when we save game files. Later on we restore eveything.
		public readonly static Room[] Rooms = new Room[]
		{
			Dormitory12, Restroom, Balcony,
			DormsHallway, LobbyNo8, Campus, Building1F1,
			Building1F2
			// FIXME: should DarknessRoom be added?
		};
		public readonly static List<Area> Areas = new List<Area>();
		public readonly static List<AObject> Objects = new List<AObject>();

		static GameMap()
		{
			Interactive.Print("正在建构世界……");
			Interactive.Flush();
			BuildCommons();
			BuildDarkness();
			BuildDormitory12();
			BuildRestroom();
			BuildBalcony();
			BuildDormsHallway();
			BuildLobbyNo8();
			BuildCampus();
			BuildPlayground();
			BuildBuilding1();
			Interactive.Print("完成。\n\n");
			Interactive.Flush();

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
			string ldesc = "", AObject defDoor = null)
		{
			a.Name = name;
			if (string.IsNullOrEmpty(ldesc))
				a.OverrideDescription = (s, v) => desc;
			else
				a.OverrideDescription = (s, v) => v.IsLight() ? ldesc : desc;
			a.AreaTo.Clear();
			foreach (var p in godir)
				a.AreaTo.Add(p.Key, p.Value);
			a.DefaultDoor = defDoor;
			a.FilterObject = (x) =>
			{
				if (notClear.Contains(x)) return ObjectVisibility.Unclear;
				if (usable.Contains(x)) return ObjectVisibility.Visible;
				return ObjectVisibility.NotVisible;
			};
		}

		public static void BuildCommons()
		{
			// reusable objects here

		}
	}
}
