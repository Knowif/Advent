using System.Collections.Generic;
using static Advent.Interactive;

namespace Advent
{
	partial class GameMap
	{
		static void BuildDormitory12()
		{
			Dormitory12.Name = "你的寝室";
			Dormitory12.Alias = new string[] { "寝室", "12#寝室", "12#" };

			Dormitory12.LightDescription =
						"这是你熟悉的寝室，闭上眼都能想象出它的样子。你能看见衣柜，桌子，床" +
						"铺（四张：你睡在靠窗的下铺）以及通向走廊和洗手间过道的两扇门。";
			Dormitory12.Description =
						"你只能在黑暗中看见大概的轮廓：衣柜，桌子，床铺（四张：你睡在靠窗的" +
						"下铺），以及通向走廊和洗手间过道的两扇门。";
			Dormitory12.IsLight = false;

			AObject Clothes = new AObject(
				"衣服", new string[] { "衣物", "服装" },
				desc:	"杂乱堆着的衣服；在顶上有几件外套和外裤。",
				ldesc:	"杂乱堆着的衣服；在顶上有几件外套和外裤。");

			AObject Closet = new AObject(
				"衣柜", new string[] { "柜子", "橱柜", "柜门" },
				desc:	"普通的寝室衣柜，没有上锁。")
			{ Openable = true,
				BeforeOpening = (self, v) =>
				{
					self.IsOpen = true;
					v.currentRoom.Objects.Add(Clothes);
					Print("你打开衣柜，里面杂乱地堆放着你的衣服。\n\n提示：可以穿衣\n\n");
					return HandleResult.FullManaged;
				},
				BeforeClosing = (self, v) =>
				{
					v.currentRoom.Objects.Remove(Clothes);
					return HandleResult.Continue;
				} };

			AObject Bottle = new AObject(
				"水杯", new string[] { "水瓶", "杯子", "瓶子", "保温杯" },
				desc:	"它的金属表面握起来凉凉的，晃一晃能感觉到里面还有一半水。",
				ldesc:	"它银色的金属表面光滑发亮，晃一晃能感觉到里面还有一半水。",
				info:	"你的一个水杯放在这里。")
			{ IsTakable = true };

			AObject Flashlight = new AObject(
				"手电筒", new string[] { "手电", "电筒" },
				desc:	"简单的手电筒，然而设计得十分周到，在黑暗中还会发出一小圈荧光。",
				ldesc:	"一个闪亮的银色金属手电筒。这花了你不少钱，但是挺漂亮，照明范围也大。",
				info:	"你的一个小手电筒放在这里。") 
			{ IsTakable = true, CanOnOff = true,
				BeforeTurningOn = (self, v) =>
				{
					if (!v.inventory.Contains(self))
					{
						Print("你先得拿到它。\n\n");
						return HandleResult.Refused;
					} else if (!v.flashlightOK)
					{
						v.flashlightOK = true; self.IsOn = true;
						Print("咔哒。没有反应。难道是接触不良？\n\n");
					} else
					{
						self.IsOn = true;
						v.currentRoom.IsPlayerLight = true;
						if (!v.firstLight)
							Print("咔哒。这次灯亮了，猛烈的白光使你一时间难以睁眼。不过令你欣慰的" +
								"是你竟然没有惊动别人。一切看起来都在原位。\n\n");
						else
							Print("咔哒。灯亮了。\n\n");
						v.firstLight = true;
					}
					return HandleResult.FullManaged;
				},
				BeforeTurningOff = (self, v) =>
				{
					if (!v.inventory.Contains(self))
					{
						Print("你先得拿到它。\n\n");
						return HandleResult.Refused;
					} else
					{
						self.IsOn = false;
						if (v.flashlightOK)
							v.currentRoom.IsPlayerLight = false;
						Print("咔哒。\n\n");
						return HandleResult.FullManaged;
					}
				} };

			AObject Desk = new AObject(
				"桌子", new string[] { "课桌", "桌椅" },
				desc:	"看上去漆黑一片，但是你知道自己的位置上有盏台灯，上面的格子里堆放着" +
						"杂物。",
				ldesc:	"普通的桌子，从寝室一边的衣柜延伸到洗手间过道的门边。上面的格子里堆" +
						"放着杂物。") 
			{ SubObjects = new List<AObject> { Bottle, Flashlight } };

			AObject Lamp = new AObject(
				"台灯", new string[] { "灯" },
				desc:	"虽然看不太清楚，但是你至少知道开关在哪里。",
				ldesc:	"廉价的白色塑料台灯。如果能开，应该会挺亮的。") 
			{ CanOnOff = true,
				BeforeTurningOn = (self, v) =>
				{
					if (!v.foundLampUnlit)
					{
						Print("咔哒。没有反应。你突然明白过来，这个时候寝室里没有电源。于是你又把它关了。\n\n");
						v.foundLampUnlit = true;
					} else
						Print("咔哒。没有反应。\n\n");
					return HandleResult.Refused;
				} };

			AObject Beds = new AObject(
				"床铺", new string[] { "床", "床位", "被子" },
				desc:	"看上去只是漆黑一片。",
				ldesc:	"床铺里面全都空无一人，被子随意铺开，就好像睡觉的人凭空消失了一样。床" +
						"架、栏杆都非常完整，没有其他的痕迹。") 
			{
				BeforeExaminaion = (self, v) => 
				{
					if (!v.foundNobody && v.currentRoom.IsPlayerLight)
					{
						Print(
						"也许只是想确认一下，你打算看一眼床铺。当光束聚焦在同学的床上时，有什么" +
						"东西吓得你倒退了一步。最开始你不敢相信这是真的，但是无论怎么眨眼你看到" +
						"的都是同一幅景象。\n\n");
						v.foundNobody = true;
					}
					return HandleResult.Continue;
				} };

			AObject Window = new AObject(
				"窗户", new string[] { "窗", "玻璃", "窗外" },
				desc:	"窗户被窗帘遮挡着，透出微弱的一点光。",
				ldesc:	"窗帘没有拉开，你看不见外面的样子。");

			AObject Darkness = new AObject(
				"黑暗", new string[] { },
				desc:	"在黑暗中看着黑暗是一种怪异的体验。事实上它并不全是黑暗：它其实在发" +
						"光，室内窗帘后的微光便是它发出的。看着它使你感到头晕。",
				ldesc:	"不像是雾，手电筒照不穿它。你只能形容它为“黑暗”：事实上它在发光，室" +
						"内窗帘后的微光便是它发出的。但你看着它的时候你完全确定它就是一片黑" +
						"暗，深不可测，使你头晕。");

			AObject Curtain = new AObject(
				"窗帘", new string[] { "帘子" },
				desc:	"从窗帘后面什么地方透出微弱的一点光来。",
				ldesc:	"浅黄的窗帘在灯光下显得苍白，遮蔽了窗外的一点点亮光。") // WILL CHANGE
			{ Openable = true, IsOpen = false,
				BeforeOpening = (self, v) =>
				{
					self.IsOpen = true;
					if (!v.foundDarkness1)
					{
						Print(
							"你拉开了窗帘。令你惊奇的是，后面根本就没有什么路灯在发光。事实上后面什" +
							"么也没有：只是一片黑暗。\n\n");
						v.foundDarkness1 = true;
					} else Print(
							"你拉开窗帘。窗帘外是一片黑暗。\n\n");
					self.Description = "窗帘被你拉开缩在一边，外面是一片黑暗。";
					self.LightDescription = self.Description;
					v.currentRoom.Objects.Add(Darkness);
					return HandleResult.FullManaged;
				},
				BeforeClosing = (self, v) =>
				{
					self.IsOpen = false;
					Print("你拉上窗帘。\n\n");
					self.Description = "从窗帘后面什么地方透出微弱的一点光来。";
					self.LightDescription = "浅黄的窗帘在灯光下显得苍白，遮蔽了窗外的一点点亮光。";
					v.currentRoom.Objects.Remove(Darkness);
					return HandleResult.FullManaged;
				} };

			Flashlight.Parent = Desk;

			Doorway RestroomDoor = new Doorway(
				Restroom, "去洗手间过道的门", new string[] { "过道门", "厕所门", "洗手间门", "门" }, 
				desc:	"通往洗手间过道的门。");

			Doorway HallwayDoor = new Doorway(
				DormsHallway, "去走廊的门", new string[] { "走廊门", "门" },
				desc: "通往寝室楼走廊的门。");

			Dormitory12.Objects.AddRange(new AObject[] { Closet, Desk, Lamp, Beds, Window, Curtain });
			Dormitory12.Doorways.AddRange(new Doorway[] { RestroomDoor, HallwayDoor });
			Dormitory12.NoDescObjects.AddRange(new string[]
			{
				"椅子", "物品", "杂物", "枕头", "床单", "床架", "栏杆" , 
				"墙壁", "墙", "地面", "地板", "地", "空调"
			});

			Area BedArea = new Area("床上")
			{
				OnDescription = (self, v) =>
				{
					Print("躺在床上时，所有你能清楚看见的东西就是头顶的金属床板和一点栏杆。\n\n");
					return HandleResult.FullManaged;
				},
				OnQueryingObject = (self, v, p) =>
				{
					Print("躺在床上时，所有你能清楚看见的东西就是头顶的金属床板和一点栏杆。\n\n");
					return HandleResult.Refused;
				} };

			Dormitory12.Areas.Add(BedArea);
			Dormitory12.CurrentArea = BedArea;
			Dormitory12.BeforeCommand = (self, v, p) =>
			{
				if (p == "起床" || p == "起来")
				{
					if (self.CurrentArea == BedArea)
					{
						Print(
							"你推开被子，试图让外部的空气刺激自己，但并没有什么用。最后你还是挣扎着坐了起来，摸" +
							"索着穿上最近的一双拖鞋。在这个时候起来能干什么呢？还是走一圈再继续睡觉吧。\n\n");
						self.CurrentArea = null;
					} else
					{
						Print("你并不在床上。\n\n");
					}
					return HandleResult.FullManaged;
				} else if (p.StartsWith("睡"))
				{
					if (self.CurrentArea != BedArea)
						Print("又一次走到床边躺下来之后，");
					Print("你被一种疲倦轻易地抓住了。你很快发现这很难逃脱——确切地说，你也不想要逃脱。闭上眼睛：" +
						  "这是多么美妙的动作，多么美好的感受啊！的确，你困了。你好像看见彩色的形状和线条在睡眠" +
						  "的王国里旋转，然后就睡着了。你没有再做梦。\n\n");
					v.dreamStop = true;
					v.stopReason = "sleep";
					return HandleResult.FullManaged;
				} else return HandleResult.Continue;
			};
		}
	}
}
