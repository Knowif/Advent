using System.Collections.Generic;
using static Advent.Interactive;

namespace Advent
{
	partial class GameMap
	{
		static void BuildDormsHallway()
		{
			DormsHallway.Name = "寝室楼二楼走廊";
			DormsHallway.Alias = new string[] { "走廊", "门廊" };

			DormsHallway.Description =
						"“安全出口”的标志在这里发出荧荧的绿光。走廊延伸到远处，两边尽头都有" +
						"窗户。北侧有上下楼梯，右侧相应位置是储藏室。此外就是寝室了。从9#到" +
						"13#是你们班的，而其中12#就是你所在的那间。";
			DormsHallway.LightDescription = 
						"在手电筒开辟出的坚硬光柱之外，幽暗的墙壁和瓷砖反射着一点点绿色光芒。" +
						"走廊延伸到远处，两边尽头都有窗户。北侧有上下楼梯，右侧相应位置是储藏" +
						"室。此外就是寝室了。从9#到13#是你们班的，而其中12#就是你所在的那间。";

			AObject Sign = new AObject(
				"标志", new string[] { "安全出口", "绿光", "标牌" },
				desc:	"它发着光，很远就能看得清楚。不过上面的一如往常，没什么特别：" +
						"“安全出口”，一个指向楼梯的箭头，还有一个逃跑人形的标识。",
				ldesc:	"绿光就是从这个标志上发出来。你现在用手电筒指向它，它的光就收" +
						"敛起来了。上面写着：“安全出口”，加上一个指向楼梯的箭头。");

			DormsHallway.Objects.Add(Sign);
			DormsHallway.NoDescObjects.AddRange(new string[]
			{
				"墙壁", "墙", "地面", "地板", "地", "楼梯", "门"
			});

			// Now we build all the 20 doors

			for (int i = 1; i <= 20; i++)
			{
				if (i == 12)
				{
					Doorway d12 = new Doorway(Dormitory12,
						"12#寝室的门", new string[] { "12#寝室门", "12#门", "门" },
						"去" + i + "#寝室的门。");
					DormsHallway.Doorways.Add(d12);
				} else
				{
					Doorway d = new Doorway(null,
						i + "#寝室的门", new string[] { i + "#寝室门", i + "#门", i + "#寝室", i + "#", "门" },
						"去" + i + "#寝室的门。", locked: true)
					{
						BeforeOpening = (self, v) =>
						{
							if (!v.foundDoorsLocked)
							{
								Print("你拧了拧门把手。根本转不动，就像整个是一块实心金属一样；" +
									"这些寝室肯定是因为某些原因被锁住了。\n\n");
								v.foundDoorsLocked = true;
								return HandleResult.Refused;
							}
							return HandleResult.Continue;
						}
					};
					DormsHallway.Doorways.Add(d);
				}
			}

			Doorway LobbyStairs = new Doorway(LobbyNo8, Direction.Down);
			DormsHallway.Doorways.Add(LobbyStairs);
		}
	}
}