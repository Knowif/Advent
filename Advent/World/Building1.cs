using System.Collections.Generic;

namespace Advent
{
	partial class GameMap
	{
		static void BuildBuilding1()
		{
			void FirstFloor()
			{
				Building1F1.Name = "行政楼[1#]一楼";
				Building1F1.Alias = new string[0];

				Area NLobby = new Area();
				Area SLobby = new Area();
				Area Library = new Area();
				Area Study = new Area();
				Area CStairs = new Area();
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
					Campus.CurrentArea = Campus.FindArea("教学楼间的小广场");
					return HandleResult.Continue;
				};
				Building1F1.Objects.Add(doorNLobby);
				SetupArea(ref NLobby, "大厅北侧",
					desc:		"你站在一个空旷、昏暗的大厅中，北侧开阔的玻璃门外教学楼间的广场被暗紫色的天空微弱地照亮。两边通往大厅南侧的过道之间，是往上延伸的宽敞大理石台阶。",
					ldesc:		"空旷的大厅，在手电筒的光束下甚至显得更加空旷，你站在中间。北侧开阔的玻璃门外教学楼间的广场被暗紫色的天空微弱地照亮。两边通往大厅南侧的过道之间，是往上延伸的宽敞大理石台阶。",
					usable:		new AObject[] { doorNLobby },
					notClear:	new AObject[0],
					godir:		new Dictionary<Direction, Area>
								{ { Direction.S, SLobby } },
					defDoor:	doorNLobby);

				AObject doorSLobby = AObject.SimpleDoor(
					dest:		Campus,
					name:		"南侧大门",
					alias:		new[] { "外面", "大门", "门" },
					desc:		"一扇开阔的玻璃门外，道路后面是寝室楼和树木的阴暗影子。",
					flopside:	() => Campus.FindObjectInternal("行政楼的南门"));
				doorSLobby.OnEntering = (s, v) =>
				{
					Campus.CurrentArea = Campus.FindArea("道路");
					return HandleResult.Continue;
				};
				Building1F1.Objects.Add(doorSLobby);
				SetupArea(ref SLobby, "大厅南侧",
					desc:		"你站在一个昏暗的大厅中，南侧开阔的玻璃门外，道路后面是寝室楼和树木的阴暗影子。两边通往大厅北侧的过道之间是一面有浮雕的墙。",
					ldesc:		"大厅在手电筒的光束下显得既狭小又空旷；你站在中间。南侧开阔的玻璃门外，道路后面是寝室楼和树木的阴暗影子。两边通往大厅北侧的过道之间是一面有浮雕的墙。",
					usable:		new AObject[] { doorSLobby },
					notClear:	new AObject[0],
					godir:		new Dictionary<Direction, Area>
								{ { Direction.N, NLobby } },
					defDoor:	doorSLobby);
			}

			FirstFloor();
		}
	}
}
