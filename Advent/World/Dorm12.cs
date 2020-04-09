using System.Collections.Generic;
using static Advent.Interactive;

namespace Advent
{
	partial class GameMap
	{
		static void BuildDormitory12()
		{
			Dormitory12.Name = "你的寝室";
			Dormitory12.Alias = new[] { "寝室", "12#寝室", "12#" };

			Dormitory12.LightDescription =
				"这是你熟悉的寝室，闭上眼都能想象出它的样子。你能看见衣柜，桌子，床铺（四张：你睡在靠窗的下铺）以及通向走廊和洗手间过道的两扇门。";
			Dormitory12.Description =
				"你只能在黑暗中看见大概的轮廓：衣柜，桌子，床铺（四张：你睡在靠窗的下铺），以及通向走廊和洗手间过道的两扇门。\n\n现在仅有的照明是从窗帘后透出的一点微光。";
			Dormitory12.IsLit = false;

			AObject Clothes = new AObject(
				"衣服", new[] { "衣物", "服装" },
				desc:	"杂乱堆着的衣服；在顶上有几件外套和外裤。",
				ldesc:	"杂乱堆着的衣服；在顶上有几件外套和外裤。",
				info:	"打开的衣柜里能看见你杂乱堆放着的衣服。");

			AObject Closet = new AObject(
				"衣柜", new[] { "柜子", "橱柜", "柜门" },
				desc:	"普通的寝室衣柜，没有上锁。")
			{ IsOpenable = true,
				OnOpening = (self, v) =>
				{
					self.OpenState = true;
					v.currentRoom.Objects.Add(Clothes);
					Print("你打开衣柜，里面杂乱地堆放着你的衣服。\n\n");
					return HandleResult.FullManaged;
				},
				OnClosing = (self, v) =>
				{
					v.currentRoom.Objects.Remove(Clothes);
					return HandleResult.Continue;
				} };

			AObject Bottle = new AObject(
				"水杯", new[] { "水瓶", "杯子", "瓶子", "保温杯" },
				desc:	"它的金属表面握起来凉凉的，晃一晃能感觉到里面还有一半水。",
				ldesc:	"它银色的金属表面光滑发亮，晃一晃能感觉到里面还有一半水。",
				info:	"你的一个水杯放在这里。")
			{ IsTakable = true };

			AObject Flashlight = new AObject(
				"手电筒", new[] { "手电", "电筒" },
				desc:	"简单的手电筒，然而设计得十分周到，在黑暗中还会发出一小圈荧光。",
				ldesc:	"一个闪亮的银色金属手电筒。这花了你不少钱，但是挺漂亮，照明范围也大。",
				info:	"你的一个小手电筒放在这里。") 
			{ IsTakable = true, IsSwitch = true,
				OnTurningOn = (self, v) =>
				{
					if (!v.inventory.Contains(self))
					{
						Print("你先得拿到它。\n\n");
						return HandleResult.Refused;
					} else if (!v.flashlightOK)
					{
						v.flashlightOK = true; self.SwitchState = true;
						Print("咔哒。没有反应。难道是接触不良？\n\n");
					} else
					{
						self.SwitchState = true;
						v.currentRoom.IsPlayerLit = true;
						if (!v.firstLight)
							Print("咔哒。这次灯亮了，猛烈的白光使你一时间难以睁眼。不过令你欣慰的是你竟然没有惊动别人。一切看起来都在原位。\n\n");
						else
							Print("咔哒。灯亮了。\n\n");
						v.firstLight = true;
					}
					return HandleResult.FullManaged;
				},
				OnTurningOff = (self, v) =>
				{
					if (!v.inventory.Contains(self))
					{
						Print("你先得拿到它。\n\n");
						return HandleResult.Refused;
					} else
					{
						self.SwitchState = false;
						if (v.flashlightOK)
							v.currentRoom.IsPlayerLit = false;
						Print("咔哒。\n\n");
						return HandleResult.FullManaged;
					}
				} };

			AObject ACControl = new AObject(
				"空调遥控器", new[] { "遥控器" },
				desc:	"一团方块形状的空调遥控器，按键在黑暗中微弱地发光。",
				ldesc:	"方块形状的白色空调遥控器，侧面带着红色条纹。",
				info:	"一个空调遥控器放在这里。")
			{ IsTakable = true };

			AObject Desk = new AObject(
				"桌子", new[] { "课桌", "桌椅" },
				desc:	"看上去漆黑一片，但是你知道自己的位置上有盏台灯，上面的格子里堆放着杂物。",
				ldesc:	"普通的桌子，从寝室一边的衣柜延伸到洗手间过道的门边。上面的格子里堆放着杂物。") 
			{ IsContainer = true,
				SubObjects = new List<AObject> { Bottle, Flashlight, ACControl } };

			AObject Lamp = new AObject(
				"台灯", new[] { "灯" },
				desc:	"虽然看不清楚，但是你至少知道开关在哪里。",
				ldesc:	"廉价的白色塑料台灯。如果能开，应该会挺亮的。") 
			{ IsSwitch = true,
				OnTurningOn = (self, v) =>
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
				"床铺", new[] { "床", "床位", "被子" },
				desc:	"看上去只是漆黑一片。",
				ldesc:	"床铺里面全都空无一人，被子随意铺开，就好像睡觉的人凭空消失了一样。床架、栏杆都非常完整，没有其他的痕迹。") 
			{ IsContainer = true,
				OnExaminaion = (self, v) => 
				{
					if (!v.foundNobody && (v.currentRoom.IsPlayerLit || v.currentRoom.IsLit))
					{
						string x = v.currentRoom.IsPlayerLit ? "手电筒的光束" : "视线";
						Print(
						"也许只是想确认一下，你打算看一眼床铺。当" + x + "聚焦在同学的床上时，有什么东西吓得你倒退了一步。最开始你不敢相信这是真的，但是无论怎么眨眼你看到的都是同一幅景象。\n\n");
						v.foundNobody = true;
					}
					return HandleResult.Continue;
				} };
			
			AObject AirConditioner = new AObject(
				"空调", new string[0],
				desc:	"空调制热模式低沉的呼吸声中，两个绿点随着在黑暗的高处闪光。", // can change
				ldesc:	"普通而标准的寝室空调，发出制热模式低沉的呼吸声。两个绿点微弱地在面板上亮着。")
			{ IsSwitch = true, SwitchState = true, 
				OnTurningOn = (self, v) =>
				{
					if (!v.inventory.Contains(ACControl))
					{
						Print("你没有拿遥控器。\n\n");
						return HandleResult.Refused;
					}
					self.Description = (_s, _v) => 
						"空调制热模式低沉的呼吸声中，两个绿点随着在黑暗的高处闪光。";
					self.LightDescription = (_s, _v) => 
						"普通而标准的寝室空调，发出制热模式低沉的呼吸声。两个绿点微弱地在面板上亮着。";
					Print("滴。导流板转动的僵硬声音。\n\n");
					self.SwitchState = true;
					return HandleResult.FullManaged;
				},
				OnTurningOff = (self, v) =>
				{
					if (!v.inventory.Contains(ACControl))
					{
						Print("你没有拿遥控器。\n\n");
						return HandleResult.Refused;
					}
					self.Description = (_s, _v) => 
						"看上去只是漆黑一片。";
					self.LightDescription = (_s, _v) => 
						"普通而标准的寝室空调，毫无活力。";
					Print("滴。导流板转动的僵硬声音。\n\n");
					self.SwitchState = false;
					return HandleResult.FullManaged;
				} };

			AObject Window = new AObject(
				"窗户", new[] { "窗", "玻璃", "窗外", "天空", "外面" },
				desc:	"窗户被窗帘遮挡着，透出微弱的一点光。",
				ldesc:	"窗帘没有拉开，你看不见外面的样子。");

			AObject Darkness = new AObject(
				"黑暗", new[] { "窗外", "外面" },
				desc:	"在黑暗中看着黑暗是一种怪异的体验。事实上它并不全是黑暗：它其实在发光，室内窗帘后的微光便是它发出的。看着它使你感到头晕。",
				ldesc:	"不像是雾，手电筒照不穿它。你只能形容它为“黑暗”：事实上它在发光，室内窗帘后的微光便是它发出的。但你看着它的时候你完全确定它就是一片黑暗，深不可测，使你头晕。");

			AObject Curtain = new AObject(
				"窗帘", new[] { "帘子" },
				desc:	"从窗帘后面什么地方透出微弱的一点光来。",
				ldesc:	"浅黄的窗帘在灯光下显得苍白，遮蔽了窗外的一点点亮光。") // WILL CHANGE
			{ IsOpenable = true, OpenState = false,
				OnOpening = (self, v) =>
				{
					self.OpenState = true;
					self.Description = (_s, _v) => "窗帘被你拉开缩在一边，外面是一片黑暗。";
					self.LightDescription = self.Description;
					v.currentRoom.Objects.Remove(Window);
					v.currentRoom.Objects.Add(Darkness);

					if (!v.foundDarkness1)
					{
						Print(
							"你拉开了窗帘。令你惊奇的是，后面根本就没有什么路灯在发光。事实上后面什么也没有：只是一片黑暗。\n\n");
						v.foundDarkness1 = true;
					} else Print(
							"你拉开窗帘。窗帘外是一片黑暗。\n\n");
					return HandleResult.FullManaged;
				},
				OnClosing = (self, v) =>
				{
					self.OpenState = false;
					self.Description = (_s, _v) => "从窗帘后面什么地方透出微弱的一点光来。";
					self.LightDescription = (_s, _v) => "浅黄的窗帘在灯光下显得苍白，遮蔽了窗外的一点点亮光。";
					v.currentRoom.Objects.Remove(Darkness);
					v.currentRoom.Objects.Add(Window);

					Print("你拉上窗帘。\n\n");
					return HandleResult.FullManaged;
				} };

			Flashlight.Parent = Desk;
			Bottle.Parent = Desk;
			ACControl.Parent = Desk;

			AObject RestroomDoor = AObject.SimpleDoor(
				Restroom, "去洗手间过道的门", 
				new[] { "厕所门", "洗手间门", "厕所", "洗手间", "门" }, 
				desc:	"通往洗手间过道的门。",
				flopside: () => Restroom.FindObjectInternal("去寝室的门"));

			AObject HallwayDoor = AObject.SimpleDoor(
				DormsHallway, "去走廊的门", 
				new[] { "走廊门", "走廊", "门" },
				desc: "通往寝室楼走廊的门。",
				flopside: () => DormsHallway.FindObjectInternal("12#寝室的门"));

			Dormitory12.Objects.Clear();
			Dormitory12.Objects.AddRange(new[] { 
				Closet, Desk, Lamp, Beds, Window, Curtain, AirConditioner, RestroomDoor, HallwayDoor });
			Dormitory12.DefaultDoor = HallwayDoor;
			
			Dormitory12.Objects.Add(new AObject("椅子", new[]
				{ "物品", "杂物", "枕头", "床单", "被子", "天花板", "东西", "床架", "栏杆", 
				"墙壁", "墙", "地面", "地板", "地" }));

			Area BedArea = new Area("床上")
			{
				OverrideDescription = (self, v) =>
					"躺在床上时，所有你能清楚看见的东西就是头顶的金属床板和一点栏杆。",
				FilterObject = (x) => ObjectVisibility.NotVisible
			};

			Dormitory12.Areas.Clear();
			Dormitory12.Areas.Add(BedArea);
			Dormitory12.CurrentArea = BedArea;

			Dormitory12.BeforeCommand = (self, v, p) =>
			{
				if (p == "起床" || p == "起来")
				{
					if (self.CurrentArea == BedArea)
					{
						Print("你推开被子，试图让外部的空气刺激自己，但并没有什么用。最后你还是挣扎着坐了起来，摸索着穿上最近的一双拖鞋。在这个时候起来能干什么呢？还是走一圈再继续睡觉吧。\n\n");
						self.CurrentArea = null;
					} else
						Print("你并不在床上。\n\n");
					self.PostCommand(self, v, p);
					return HandleResult.FullManaged;
				} else if (p.StartsWith("睡"))
				{
					Print($"{(self.CurrentArea == BedArea ? "" : "又一次走到床边躺下来之后，")}你被一种疲倦轻易地抓住了。你很快发现这很难逃脱——确切地说，你也不想要逃脱。闭上眼睛：这是多么美妙的动作，多么美好的感受啊！的确，你困了。你好像看见彩色的形状和线条在旋转，然后就睡着了。你没有再做梦。\n\n");
					v.dreamStop = true;
					v.stopReason = "sleep";
					self.PostCommand(self, v, p);
					return HandleResult.FullManaged;
				} else
					return HandleResult.Continue;
			};
			
			int acCounter = -1;
			Dormitory12.PostCommand = (self, v, p) =>
			{
				acCounter++;
				if (!AirConditioner.SwitchState)
					return HandleResult.Continue;
				switch (acCounter % 5)
				{
					case 0: Print(
						"空调发出低沉的呼吸声，吐出温热的空气。\n\n"); break;
					case 1: Print(
						"空调继续呼吸，声音变得更加急促。\n\n"); break;
					case 2: Print(
						"空调平稳地呼吸。\n\n"); break;
					case 3: Print(
						"空调仿佛打了个嗝，传来导流板转动的僵硬声音。咯吱。\n\n"); break;
					case 4: Print(
						"空调的热气晃动着吹来。\n\n"); break;
				}
				return HandleResult.Continue;
			};
		}
	}
}
