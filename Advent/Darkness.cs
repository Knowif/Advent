using System.Collections.Generic;
using static Advent.Interactive;

namespace Advent
{
	partial class GameMap
	{
		static void BuildDarknessRoom()
		{
			DarknessRoom.Name = "黑暗";
			DarknessRoom.Alias = new string[] { "黑色" };

			DarknessRoom.GetDescription = (self, v) =>
						"你走进那片黑暗之后，周围的一切就突然消失了。你身边只有无尽的虚空，" +
						"没有颜色，没有形体。你的四肢碰不到任何支撑物，而你又不觉得自己在坠" +
						"落。事实上，你甚至不知道自己还有没有四肢……";

			DarknessRoom.PostDescription = (self, v) =>
			{
				v.dreamStop = true;
				v.stopReason = "darkness";
				return "";
			};
		}
	}
}