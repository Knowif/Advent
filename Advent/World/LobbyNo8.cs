using System.Collections.Generic;
using static Advent.Interactive;

namespace Advent
{
	partial class GameMap
	{
		static void BuildLobbyNo8()
		{
			LobbyNo8.Name = "8#寝室楼大厅";
			LobbyNo8.Alias = new string[] { "大厅", "门厅" };
			LobbyNo8.IsLight = true;

			LobbyNo8.Description =
						"灯光下是一个空荡的小门厅，墙上有一些花哨夸张的图案。楼梯向上通往二" +
						"楼，另一侧是宿管老师值班的房间，没开灯，窗内黑洞洞的。一扇玻璃大门" +
						"通向外面，而室外是一片压抑、看不清楚的黑暗。";
			LobbyNo8.LightDescription = LobbyNo8.Description;
			LobbyNo8.PostDescription = (self, v) =>
			{
				v.foundDarkness1 = true;
				return self.GetDoorway("大门").IsOpen ? "风从门外卷进来。\n\n" : "";
			};

			Doorway KeeperRoomDoor = new Doorway(
				null, "去值班房的门", new string[] { "值班房门", "值班房", "门" },
				"通向宿管老师值班房的小门。", locked: true);

			Doorway OutDoor = new Doorway(
				DarknessRoom, "去室外的大门", new string[] { "外面", "室外", "大门", "门" },
				"通向室外的大门。", open: true);

			Doorway DormsStairs = new Doorway(DormsHallway, Direction.Up);

			LobbyNo8.Doorways.AddRange(new Doorway[] { KeeperRoomDoor, OutDoor, DormsStairs });
		}
	}
}