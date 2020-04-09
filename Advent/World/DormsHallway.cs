using System.Collections.Generic;
using static Advent.Interactive;

namespace Advent
{
	partial class GameMap
	{
		static void BuildDormsHallway()
		{
			DormsHallway.Name = "寝室楼二楼走廊";
			DormsHallway.Alias = new[] { "走廊", "门廊" };

			DormsHallway.Description =
				"“安全出口”的标志在这里发出荧荧的绿光。走廊延伸到远处，两边尽头都是墙壁。北侧有上下楼梯，南侧相应位置是储藏室。此外就是寝室了。从9#到13#是你们班的，而其中12#就是你所在的那间。";
			DormsHallway.LightDescription =
				"在手电筒开辟出的坚硬光柱之外，幽暗的墙壁和瓷砖里映出一点点绿色光芒。走廊延伸到远处，两边尽头都是墙壁。北侧有上下楼梯，南侧相应位置是储藏室。此外就是寝室了。从9#到13#是你们班的，而其中12#就是你所在的那间。";

			AObject Sign = new AObject(
				"标志", new[] { "安全出口", "绿光", "标牌" },
				desc:	"很远就能看得清楚，主要是因为它一如往常，没什么特别：“安全出口”，一个指向楼梯的发光箭头，还有一个逃跑人形的发光标识。",
				// You may see that the escaping humanoid symbol disappears in the dark. This is only an ominous easter egg, for clarification.
				ldesc:	"绿光就是从这个标志上发出来。你现在用手电筒指向它，它的光就收敛起来了。标志上有“安全出口”几个字加上一个指向楼梯的箭头。");

			DormsHallway.Objects.Clear();
			DormsHallway.Objects.Add(Sign);

			DormsHallway.Objects.Add(new AObject("墙壁", 
				new[] { "墙壁", "墙", "地面", "地板", "地", "楼梯", "门" }));

			// Now we build all the 20 doors
			for (int i = 1; i <= 20; i++)
			{
				if (i == 12)
				{
					AObject d12 = AObject.SimpleDoor(Dormitory12,
						"12#寝室的门", new[] { "12#寝室门", "12#门", "门" },
						"去" + i + "#寝室的门。",
						flopside: () => Dormitory12.FindObjectInternal("去走廊的门"));
					DormsHallway.Objects.Add(d12);
				} else
				{
					AObject d = AObject.SimpleDoor(null, i + "#寝室的门", 
						new[] { i + "#寝室门", i + "#门", i + "#寝室", i + "#", "门" },

						"去" + i + "#寝室的门。", locked: true);
					d.OnOpening = (self, v) =>
						{
							if (!v.foundDoorsLocked)
							{
								Print("你拧了拧门把手。根本转不动，就像整个是一块实心金属一样；这些寝室肯定是因为某些原因被锁住了。\n\n");
								v.foundDoorsLocked = true;
								return HandleResult.Refused;
							}
							return HandleResult.Continue;
						};
					DormsHallway.Objects.Add(d);
				}
			}

			DormsHallway.RoomTo.Clear();
			DormsHallway.RoomTo.Add(Direction.Down, LobbyNo8);
		}
	}
}