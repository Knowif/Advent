using System.Collections.Generic;
using static Advent.Interactive;

namespace Advent
{
	partial class GameMap
	{
		static void BuildLobbyNo8()
		{
			LobbyNo8.Name = "8#寝室楼大厅";
			LobbyNo8.Alias = new[] { "大厅", "门厅" };
			LobbyNo8.IsLit = true;

			LobbyNo8.Description = LobbyNo8.Description =
				"灯光下是一个空荡的小门厅，墙上有一些花哨夸张的图案。楼梯向上通往二楼，另一侧是宿管老师值班的房间，没开灯，窗内黑洞洞的。一扇玻璃大门通向外面，而室外是一片压抑、看不清楚的黑暗。";
			LobbyNo8.PostDescription = (self, v) =>
			{
				v.foundDarkness1 = true;
				return self.FindObjectInternal("大门").OpenState ? "风从门外卷进来。\n\n" : "";
			};

			LobbyNo8.Objects.Clear();
			LobbyNo8.Objects.Add(new AObject("墙壁", new string[] 
				{ "墙壁", "墙", "地面", "地板", "地", "楼梯", "门", "灯", "灯光", "图案", "天花板" }));

			AObject KeeperRoomDoor = AObject.SimpleDoor(
				null, "去值班房的门", new[] { "值班房门", "值班房", "门" },
				"通向宿管老师值班房的小门。", locked: true);

			// Real fake door for now; will be changed in day 2.
			AObject OutDoor = AObject.SimpleDoor(
				DarknessRoom, "去室外的大门", new[] { "外面", "室外", "大门", "门", "门外" },
				"通向室外的大门。", open: true);
			
			LobbyNo8.Objects.AddRange(new[] { KeeperRoomDoor, OutDoor });

			LobbyNo8.DefaultDoor = OutDoor;

			LobbyNo8.RoomTo.Clear();
			LobbyNo8.RoomTo.Add(Direction.Up, DormsHallway);
		}
	}
}