using System.Collections.Generic;

namespace Advent
{
	partial class GameMap
	{
		static void BuildBuilding1()
		{
			// first floor
			{
				Building1F1.Name = "行政楼[1#]一楼";
				Building1F1.Alias.Clear();
				Building1F1.IsLit = false;

				Area NLobby = new Area();
				Area SLobby = new Area();
				//Area Library = new Area();
				//Area Study = new Area();
				Building1F1.Areas.Clear();
				Building1F1.Areas.AddRange(new[]
					{ NLobby, SLobby, /*Library, Study, CStairs*/ });
				Building1F1.Objects.Clear();

				AObject doorNLobby = AObject.SimpleDoor(
					dest:		Campus,
					name:		"北侧大门",
					alias:		new[] { "外面", "大门", "门" },
					desc:		"一扇开阔的玻璃门外，教学楼间的广场被暗紫色的天空微弱地照亮。",
					flopside:	() => Campus.FindObjectInternal("行政楼的正门"));
				doorNLobby.OnEntering = (s, v) =>
				{
					Campus.ChangeArea("教学楼间的小广场");
					return HandleResult.Continue;
				};
				Building1F1.Objects.Add(doorNLobby);
				SetupArea(ref NLobby, "大厅北侧",
					desc: "你站在一个空旷、昏暗的大厅中，北侧开阔的玻璃门外教学楼间的广场被暗紫色的天空微弱地照亮。两边通往大厅南侧的过道之间，是往上延伸的宽敞大理石楼梯。",
					ldesc: "空旷的大厅，在手电筒的光束下甚至显得更加空旷，你站在中间。北侧开阔的玻璃门外教学楼间的广场被暗紫色的天空微弱地照亮。两边通往大厅南侧的过道之间，是往上延伸的宽敞大理石台阶。",
					usable: new[] { doorNLobby },
					notClear: new AObject[0],
					godir: new Dictionary<Direction, Area>
								{ { Direction.S, SLobby } },
					defDoor: doorNLobby);
				NLobby.RoomTo.Add(Direction.Up, Building1F2);
				NLobby.OnGoDirection = (self, v, d) =>
				{
					Building1F2.ChangeArea("中央楼梯");
					return HandleResult.Continue;
				};

				AObject doorSLobby = AObject.SimpleDoor(
					dest:		Campus,
					name:		"南侧大门",
					alias:		new[] { "外面", "大门", "门" },
					desc:		"一扇开阔的玻璃门外，道路后面是寝室楼和树木的阴暗影子。",
					flopside:	() => Campus.FindObjectInternal("行政楼的南门"));
				doorSLobby.OnEntering = (s, v) =>
				{
					Campus.ChangeArea("道路");
					return HandleResult.Continue;
				};
				Building1F1.Objects.Add(doorSLobby);
				SetupArea(ref SLobby, "大厅南侧",
					desc: "你站在一个昏暗的大厅中，南侧开阔的玻璃门外，道路后面是寝室楼和树木的阴暗影子。两边通往大厅北侧的过道之间是一面有浮雕的墙。",
					ldesc: "大厅在手电筒的光束下显得既狭小又空旷；你站在中间。南侧开阔的玻璃门外，道路后面是寝室楼和树木的阴暗影子。两边通往大厅北侧的过道之间是一面有浮雕的墙。",
					usable: new[] { doorSLobby },
					notClear: new AObject[0],
					godir: new Dictionary<Direction, Area>
								{ { Direction.N, NLobby } },
					defDoor: doorSLobby);

			}

			// second floor
			{
				Building1F2.Name = "行政楼[1#]二楼";
				Building1F2.Alias.Clear();
				Building1F2.IsLit = false;

				Area CStairs = new Area();
				//Area SLobby = new Area();

				Building1F2.Areas.Clear();
				Building1F2.Areas.AddRange(new[]
					{ CStairs });
				Building1F2.Objects.Clear();

				SetupArea(ref CStairs, "中央楼梯",
					desc: "你站在宽阔的大理石楼梯平台上；楼梯向上通往三楼，向下通往一楼。向南北方向绕过楼梯，是两边的过道；东西两面（西边是一块自习区域，东边堆着些活动用品）的空间被各层过道包围着向上通达楼顶。",
					usable: new AObject[] { },
					notClear: new AObject[0],
					godir: new Dictionary<Direction, Area>
					{ });
				CStairs.RoomTo.Add(Direction.Down, Building1F1);
				CStairs.RoomTo.Add(Direction.Up, Building1F3);
				CStairs.OnGoDirection = (self, v, d) =>
				{
					Building1F1.ChangeArea("大厅北侧");
					Building1F3.ChangeArea("中央楼梯");
					return HandleResult.Continue;
				};
			}

			// third floor
			{
				Building1F3.Name = "行政楼[1#]三楼";
				Building1F3.Alias.Clear();
				Building1F3.IsLit = false;

				Area CStairs = new Area();
				Building1F3.Areas.Clear();
				Building1F3.Areas.AddRange(new[]
					{ CStairs });
				Building1F3.Objects.Clear();

				SetupArea(ref CStairs, "中央楼梯",
					desc: "你站在宽阔的大理石楼梯平台上；楼梯向上通往四楼，向下通往二楼。向南北方向绕过楼梯，是两边的过道。",
					usable: new AObject[] { },
					notClear: new AObject[0],
					godir: new Dictionary<Direction, Area>
					{ });
				CStairs.RoomTo.Add(Direction.Down, Building1F2);
				CStairs.RoomTo.Add(Direction.Up, Building1F4);
				CStairs.OnGoDirection = (self, v, d) =>
				{
					Building1F2.ChangeArea("中央楼梯");
					Building1F4.ChangeArea("中央楼梯");
					return HandleResult.Continue;
				};
			}

			// fourth floor
			{
				Building1F4.Name = "行政楼[1#]四楼";
				Building1F4.Alias.Clear();
				Building1F4.IsLit = false;

				Area CStairs = new Area();
				Building1F4.Areas.Clear();
				Building1F4.Areas.AddRange(new[]
					{ CStairs });
				Building1F4.Objects.Clear();

				SetupArea(ref CStairs, "中央楼梯",
					desc: "你站在宽阔的大理石楼梯平台上；楼梯向上通往五楼，向下通往三楼。向南北方向绕过楼梯，是两边的过道。",
					usable: new AObject[] { },
					notClear: new AObject[0],
					godir: new Dictionary<Direction, Area>
					{ });
				CStairs.RoomTo.Add(Direction.Down, Building1F3);
				CStairs.RoomTo.Add(Direction.Up, Building1F5);
				CStairs.OnGoDirection = (self, v, d) =>
				{
					Building1F3.ChangeArea("中央楼梯");
					Building1F5.ChangeArea("中央楼梯");
					return HandleResult.Continue;
				};
			}

			// fifth floor
			{
				Building1F5.Name = "行政楼[1#]五楼";
				Building1F5.Alias.Clear();
				Building1F5.IsLit = false;

				Area CStairs = new Area();
				Building1F5.Areas.Clear();
				Building1F5.Areas.AddRange(new[]
					{ CStairs });
				Building1F5.Objects.Clear();

				SetupArea(ref CStairs, "中央楼梯",
					desc: "你站在宽阔的大理石楼梯平台上；楼梯向下通往四楼。向南北方向绕过楼梯，是两边的过道。",
					usable: new AObject[] { },
					notClear: new AObject[0],
					godir: new Dictionary<Direction, Area>
					{ });
				CStairs.RoomTo.Add(Direction.Down, Building1F4);
				CStairs.OnGoDirection = (self, v, d) =>
				{
					Building1F4.ChangeArea("中央楼梯");
					return HandleResult.Continue;
				};
			}

			// sixth floor: to be added (only approachable from the side stairs)
		}
	}
}
