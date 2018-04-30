using System.Collections.Generic;
using static Advent.Interactive;

namespace Advent
{
	partial class GameMap
	{
		static void BuildBalcony()
		{
			Balcony.Name = "阳台";
			Balcony.Alias = new string[] { };
			Balcony.isWarm = false;

			Balcony.LightDescription =
						"小瓷砖铺成的地面在光照下异常耀眼。周围环绕着的金属栏杆增加着这里的" +
						"狭小感。地面上有两个垃圾桶。你身后是回洗手间过道的移门。风声呼啸。";
			Balcony.Description = 
						"没有灯光照明，阳台看上去只是模糊的剪影。你得小心点才不会碰到地上的" +
						"两个垃圾桶。你身后是回洗手间过道的移门。风声呼啸。";

			Balcony.PostDescription = (self, v) =>
			{
				v.foundDarkness1 = true;
				return "";
			};

			AObject TrashBin = new AObject(
				"垃圾桶", new string[] { "桶", "垃圾" },
				desc:	"看上去黑漆漆的。",
				ldesc:	"两个套着垃圾袋的垃圾桶，里面没有任何东西。");

			AObject Darkness = new AObject(
				"黑暗", new string[] { },
				desc:	"在黑暗中看着黑暗是一种怪异的体验。事实上它并不全是黑暗：它其实在发" +
						"光，室内窗帘后的微光便是它发出的。看着它使你感到头晕。",
				ldesc:	"不像是雾，手电筒照不穿它。你只能形容它为“黑暗”：事实上它在发光，室" +
						"内窗帘后的微光便是它发出的。但你看着它的时候你完全确定它就是一片黑" +
						"暗，深不可测，使你头晕。")
			{ Information = "栏杆外什么也没有，只是一片深深的、超现实的黑暗。" };

			Doorway RestroomDoor = new Doorway(
				Restroom, "去洗手间过道的门", new string[] 
					{ "过道门", "厕所门", "洗手间门", "移门", "门" },
				desc: "通往洗手间过道的移门。");

			Balcony.Objects.AddRange(new AObject[] { TrashBin, Darkness });
			Balcony.Doorways.AddRange(new Doorway[] { RestroomDoor });

			Dormitory12.NoDescObjects.AddRange(new string[]
			{
				"墙壁", "墙", "地面", "地板", "地", "柱子", "栏杆"
			});

			Balcony.PostCommand = (self, v, p) =>
			{
				if (!v.withClothes)
				{
					v.temperature -= 1;
					if (v.temperature <= -4)
					{
						// player wakes by coldness
						Print("风声呼啸……你已经无法移动自己的四肢，感觉身体灌满了水银——不，" +
							  "是干冰。皮肤的刺痛渐渐减弱了，你意识到这是现实……你正在失去知" +
							  "觉，眼中黯淡的光也褪去了。你就像是一个局外人，你想到，正在旁" +
							  "观这可怕的一幕。这时黑暗吞没了你。\n\n");
						v.dreamStop = true;
						v.stopReason = "cold";
						return HandleResult.FullManaged;
					}
					string s = "风吹着。";
					switch (v.temperature)
					{
						case -1: s += "你感到有点冷。\n\n"; break;
						case -2: s += "你更冷了，发觉到只穿着内衣，而冷风从深不可测的黑暗" +
									  "中渗透进来。\n\n"; break;
						case -3: s += "寒冷似乎在你身体的骨骼间被一次次注射进去。你颤抖着，" +
									  "甚至难以移动。\n\n"; break;
						default: s += "\n\n"; break;
					}
					Print(s);
				}
				return HandleResult.Continue;
			};
		}
	}
}