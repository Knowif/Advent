using System.Collections.Generic;
using static Advent.Interactive;

namespace Advent
{
	partial class GameMap
	{

		public static void Day2Setting()
		{
			// Whoooooosh.... 

			// A. reset
			
			BuildDormitory12();
			BuildRestroom();
			BuildBalcony();
			BuildDormsHallway();
			BuildLobbyNo8();

			Dormitory12.Objects.Find((x) => x.Name == "床铺").Description = (_s, _v) => "在暗中看不清楚。";
			
			// B. modify

			// B.1. dorm12

			// remove the light of darkness
			Dormitory12.LightDescription =
				"这是你熟悉的寝室，闭上眼都能想象出它的样子。你能看见衣柜，桌子，床铺（四张：你睡在靠窗的下铺）以及通向走廊和洗手间过道的两扇门。";
			Dormitory12.Description =
				"你只能在黑暗中看见大概的轮廓：衣柜，桌子，床铺（四张：你睡在靠窗的下铺），以及通向走廊和洗手间过道的两扇门。";

			// add the light of sky
			Dormitory12.GetDescription = (self, v) =>
			{
				if (self.IsLit)
					return Dormitory12.LightDescription + 
						"\n\n窗帘拉开，外面的天空里滚动着紫红色的浓云。\n\n";
				else if (self.IsPlayerLit)
					return Dormitory12.LightDescription + 
						"\n\n窗帘后什么地方透出一种红晕，但几乎被手电筒的强光掩盖了。\n\n";
				else
					return Dormitory12.Description + 
						"\n\n有一种可爱的红晕穿透窗帘照进来。\n\n";
			};

			Dormitory12.SetObjectInternal("窗帘", 
				new AObject(Dormitory12.FindObjectInternal("窗帘"))
			{
				Description = (_s, _v) => 
					"从它后面什么地方照进来一种红色的光。",
				LightDescription = (_s, _v) =>
					"从它后面什么地方照进来一种红色的光，但几乎被你手电筒的强光掩盖了。",
				OnOpening = (self, v) =>
				{
					self.OpenState = true;
					self.Description = self.LightDescription = (_s, _v) => "窗帘被你拉开，缩在一边。";
					v.currentRoom.FindObjectInternal("窗户").Description = 
						v.currentRoom.FindObjectInternal("窗户").LightDescription = (_s, _v) =>
							"窗外是对面的9#楼（高中男生寝室）和周围的人行道路。天空是紫红色的。";

					if (!v.foundNoVoid2)
					{
						Print($"你拉开窗帘的一瞬间，紫红色的光涌入房间。它来自对面寝室楼后的天空。楼下的路灯沦为了暗暗的白点。地面也散发出紫色，与天空一起变幻着。在你眼中，有一秒钟时间，天空成了翻滚的海洋，而地面则是风中倒置的迷雾和森林。但是寂静……这片寂静使你惊异。现在甚至听不见外面风声，只有背景中若有若无的耳鸣。{(v.foundDarkness1 ? "你原先期望看到什么？黑暗？……紫色的黑暗？有点可笑。" : "")}\n\n");
						v.foundNoVoid2 = true;
					} else
						Print("你拉开窗帘时，紫红色的光涌入房间。\n\n");
					v.currentRoom.IsLit = true;

					return HandleResult.FullManaged;
				},
				OnClosing = (self, v) =>
				{
					self.OpenState = false;
					self.Description = (_s, _v) => "从它后面什么地方照进来一种红晕。";
					self.LightDescription = (_s, _v) => 
						"从它后面什么地方照进来一种红晕，但几乎被你手电筒的强光掩盖了。";
					v.currentRoom.FindObjectInternal("窗户").Description = (_s, _v) =>
						"窗户被窗帘遮挡着，只从外面透进来一种红晕。";
					v.currentRoom.FindObjectInternal("窗户").LightDescription = (_s, _v) =>
						"窗帘没有拉开，你看不见外面的样子。";

					Print("你拉上窗帘。室内恢复了原来的阴暗。\n\n");

					return HandleResult.FullManaged;
				}
				});

			Dormitory12.SetObjectInternal("窗户",
				new AObject(Dormitory12.FindObjectInternal("窗户"))
			{
				Description = (_s, _v) =>
					"窗户被窗帘遮挡着，只从外面透进来一种红晕。",
				LightDescription = (_s, _v) =>
					"窗帘没有拉开，你看不见外面的样子。"
			});

			Dormitory12.BeforeCommand = (self, v, p) =>
			{
				// convenience magic
				if (p == "xyzzy")
				{
					v.withClothes = true;
					v.currentRoom = Campus;
					Campus.CurrentArea = Campus.Areas.Find((x) => x.Name == "食堂和寝室楼之间");
					Print("Flooooooooooosh!!\n\n");
					return HandleResult.FullManaged;
				}
				if (p == "起床" || p == "起来")
				{
					if (self.CurrentArea.Name == "床上")
					{
						Print(
							"你推开被子，坐起来，摸索着穿上最近的一双拖鞋。\n\n");
						self.CurrentArea = null;
					} else
						Print("你并不在床上。\n\n");

					return HandleResult.FullManaged;
				} else if (p.StartsWith("睡"))
				{
					Print("现在你不想睡觉。\n\n");
					return HandleResult.FullManaged;
				} else
					return HandleResult.Continue;
			};

			// B.2. Restroom: Good. No changes...

			// B.3. Balcony

			// sky
			Balcony.GetDescription = (self, v) => 
				$"阳台的瓷砖地面反射着{(self.IsPlayerLit ? "手电筒和" : "")}天空的色彩。天空在地面上投下你模模糊糊的影子，瓷砖的边缘泛起微光。虽然还没到能看书的地步，但你能注意到光照的亮度很不寻常。眼前漂浮在夜晚朦胧空气中的树和楼房奇怪地漂亮。你能听见风声了。";
			Balcony.PostDescription = (self, v) =>
			{
				if (!v.foundNoVoid2)
				{
					v.foundNoVoid2 = true;
					return $"房间里的红光就来自那天空，使楼下的路灯沦为了暗暗的白点。地面也散发出紫色，与天空一起变幻着。在你眼中，有一秒钟时间，天空成了翻滚的海洋，而地面则是风中倒置的迷雾和森林。但是寂静——这片寂静使你惊异，现在甚至听不见外面风声，只有背景中若有若无的耳鸣。{(v.foundDarkness1 ? "你原先期望看到什么？黑暗？……紫色的黑暗？有点可笑。" : "")}\n\n";
				} else return "";
			};

			Balcony.IsLit = true;

			Balcony.Objects.Remove(Balcony.FindObjectInternal("黑暗"));
			
			Balcony.Objects.Add(new AObject("楼房", 
				new[] { "楼房", "树", "树木", "房子", "寝室楼", "9#楼", "地面", "天空" })
				{ IsReachable = false });

			// B.4. DormsHallway: No changes.

			// B.5. Lobby8

			LobbyNo8.LightDescription = 
				"灯光下是一个空荡的小门厅，墙上有一些花哨夸张的图案。楼梯向上通往二楼，另一侧是宿管老师值班的房间，没开灯，窗内黑洞洞的。一扇玻璃大门通向外面。";

			// real door
			LobbyNo8.FindObjectInternal("外面").RoomTo = Campus;
			LobbyNo8.FindObjectInternal("外面").OnEntering = (self, v) =>
			{
				Campus.CurrentArea = Campus.Areas.Find((x) => x.Name == "食堂和寝室楼之间");
				return HandleResult.Continue;
			};
			LobbyNo8.FindObjectInternal("外面").LinkedSide = () => 
				Campus.FindObjectInternal("去8#寝室楼大厅的门");
		}
	}
}
