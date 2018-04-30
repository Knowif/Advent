using System.Collections.Generic;
using static Advent.Interactive;

namespace Advent
{
	partial class GameMap
	{
		static void BuildRestroom()
		{
			Restroom.Name = "洗手间和过道";
			Restroom.Alias = new string[] { "洗手间", "过道" };

			Restroom.LightDescription =
						"手电筒在镜子里映出一片光芒，几乎掩盖了你的模样。下面的黑色台面上有" +
						"两个洗手池。镜子的墙后有两个蹲厕和一个淋浴间，没有门，所以夜里上厕" +
						"所会很方便。在洗手池对面，你能透过移门看见阳台一角。";
			Restroom.Description =
						"你的形象模模糊糊地映在黑暗的镜子里。洗手池和墙的后面是蹲厕和淋浴间。" +
						"那里没有门，夜里可以方便地过去：就像现在。洗手池对面有一个移门，通" +
						"往阳台。";
			Restroom.IsLight = false;

			AObject Mirror = new AObject(
				"镜子", new string[] { "镜面", "镜" },
				desc:	"黑暗的镜子里面模模糊糊的映出你的样子和移门后的阳台，但是看不清楚。",
				ldesc:	"手电筒在镜子里映出一片光芒，几乎掩盖了你的模样，看不清什么东西。");

			AObject Sink = new AObject(
				"洗手池", new string[] { "洗手台", "台面", "池", "池子" },
				desc:	"黑色台面上两个白色洗手池，看起来就像两只空洞的眼睛。",
				ldesc:	"黑色台面上的两个洗手池，在晚上使你想到白色的大眼睛，特别是在手电筒" +
						"的光照下。");

			AObject WaterTap = new AObject(
				"水龙头", new string[] { "龙头" },
				desc:	"两个金属水龙头。",
				ldesc:	"两个闪亮的金属水龙头。") 
			{ Openable = true, IsOpen = false, PostExamination = (self, v) =>
				{
					if (!self.IsOpen) return HandleResult.Continue;
					Print("水哗哗地流动着。\n\n");
					return HandleResult.FullManaged;
				} };

			AObject Toilet = new AObject(
				"蹲厕", new string[] { "厕所", "便池" },
				desc:	"你现在并不想上厕所，也不想无缘无故地走进去看。又不是什么好玩的东西。",
				ldesc:	"你现在并不想上厕所，也不想无缘无故地走进去看。又不是什么好玩的东西。");

			AObject Bathroom = new AObject(
				"淋浴间", new string[] { "浴室", "洗澡间" },
				desc:	"半夜里你不想踩进那又暗又滑的地方，更不可能洗澡。",
				ldesc:	"半夜里你不想踩进那湿湿滑滑的地方，更不可能洗澡。");

			Doorway DormDoor = new Doorway(
				Dormitory12, "去寝室的门", new string[] { "寝室门", "门" },
				desc:	"通往寝室的门。");

			Doorway BalconyDoor = new Doorway(
				Balcony, "去阳台的移门", new string[] { "阳台门", "移门", "门" },
				desc:	"通往阳台的移门。");

			Restroom.Objects.AddRange(new AObject[] { Mirror, Sink, WaterTap, Toilet, Bathroom });
			Restroom.Doorways.AddRange(new Doorway[] { DormDoor, BalconyDoor });

			Restroom.NoDescObjects.AddRange(new string[]
			{
				"墙壁", "墙", "地面", "地板", "地"
			});

			Restroom.PostDescription = (self, v) => 
				self.GetObject("水龙头", v).IsOpen ? "一个水龙头里哗哗地不停冒出水。\n\n" : "";
		}
	}
}