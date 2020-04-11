using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Advent.Interactive;

namespace Advent
{
	public partial class Player
	{
		public PlayerVariables Variables { get; private set; }

		private readonly Dictionary<string, Action<string>> SingleObjVerbs;
		private readonly Dictionary<string, Action<string, string>> DoubleObjVerbs;

		public Player()
		{
			Variables = new PlayerVariables(this);

			SingleObjVerbs = new Dictionary<string, Action<string>>
			{
				{ "等", Wait },

				{ "把", ParseInversedCmd }, { "将", ParseInversedCmd }, 
				{ "用", ParseInversedCmd },

				{ "穿衣", PutOnClothes }, {"穿上", PutOnClothes},
				{ "脱衣", Undress}, { "脱下", Undress },

				{ "走进", GoTo }, { "进入", GoTo }, { "去", GoTo },
				{ "进", GoTo }, { "回到", GoTo }, { "回", GoTo },
				{ "出去", GoOut }, { "出", GoOut }, { "逃脱", GoOut }, { "逃出", GoOut },

				// special functions to handle ambiguous Chinese
				{ "拧开", TurnOnOrOpen }, { "拉开", TurnOnOrOpen },
				{ "打开", TurnOnOrOpen }, { "旋开", TurnOnOrOpen },
				{ "开启", TurnOnOrOpen }, { "开", TurnOnOrOpen },

				{ "关闭", TurnOffOrClose }, { "关上", TurnOffOrClose },
				{ "关", TurnOffOrClose },

				{ "拿起", Take }, { "拿走", Take}, { "拿", Take },
				{ "取", Take }, { "取走", Take }, { "捉", Take }, { "捞", Take }, 

				{ "看物品", Inventory }, { "物品", Inventory},

				{ "看", LookAt }, { "观察", LookAt },

				{ "检查", Examine },

				{ "闻", Smell }, { "触摸", Touch }, { "触碰", Touch },
				{ "摸", Touch }, { "碰", Touch }, { "听", Listen }
				// TODO: 砸 打 攻击
			};

			DoubleObjVerbs = new Dictionary<string, Action<string, string>>
			{
				{ "放进", PutIn }
			};
		}

		public void StartGame()
		{
#if DEBUG
			int build = int.Parse(File.ReadAllText("build.txt"), 
				System.Globalization.CultureInfo.InvariantCulture);
			Print("Build " + build + ". \n");
			File.WriteAllText("build.txt", (build + 1).ToString(
				System.Globalization.CultureInfo.InvariantCulture));
			UnixColoring = false;
			Pause();
			// start right from day 2
			SecondNight();
#else
			Print("为了获得最好的体验，建议使用新版Windows Terminal运行此程序。如果你使用推荐的或任何支持Unix着色系统的终端，请按回车。否则，请按空格。");
			Flush();
			UnixColoring = Console.ReadKey().Key == ConsoleKey.Enter;
			Clear();

			PrintCentered(
				"             _            _                         \n            | |          | |      //                \n _ __   ___ | |_   _  ___| |__   ___  _ __ ___  ___ \n| '_ \\ / _ \\| | | | |/ __| '_ \\ / _ \\| '__/ _ \\/ __|\n| |_) | (_) | | |_| | (__| | | | (_) | | | (_) \\__ \\\n| .__/ \\___/|_|\\__, |\\___|_| |_|\\___/|_|  \\___/|___/\n| |             __/ |                               \n|_|            |___/                                ");
			Print("\n\n");
			PrintCentered("流形\n");
			PrintCentered("[2032年11月]\n\n");
			PrintCentered("------------------------------\n\nAlpha Version\n");

			// Let us see a life as a day, like those,
			// Li'dia, who know not
			// that beneath and past the moment in which we live
			// lies the somber night.
			//                                 Ricardo Reis

			PrintCentered("“浑然不知    \n       我们活过的刹那\n   前后皆是暗夜。”            \n");
			PrintCentered("                    里卡尔多·雷耶斯\n\n");

			Print("\n[如有需要请调整控制台窗口大小并且重启程序]\n\n");
			while (true)
			{
				string ans = Input("是否显示帮助？");
				if (ans == "是")
				{
					foreach (var line in Properties.Resources.Help.Split(new[] { "\r\n\r\n" }, StringSplitOptions.None))
					{
						Print(line + "\n");
						Input();
					}
					break;
				} else if (ans != "否") Print("请输入“是”或“否”。\n");
				else break;
			}

			// chain-style evokation of nights
			FirstNight();
#endif
		}

		public void FirstNight()
		{
			Clear();
			Variables.currentRoom = GameMap.Dormitory12;
			Variables.dreamStop = false;

			PrintCentered("\n第一天", true);
			Print("\n\n\n");
			Print("你醒了。有一段时间你甚至没有意识到。周围很暗，只有后面的窗帘中透出的一点路灯光使你得以辨认处周围事物的熟悉轮廓。你感到自己醒得不是时候。现在也许是午夜，或者是凌晨，一个应当充满黑暗、寂静和睡眠的时刻。你不知道你为什么醒来。你一点也不冷，身体还保持着刚躺下时的姿势：你觉得自己仿佛根本没有睡下过。\n\n提示：起床？看手表？\n\n");

			InputLoop();

			Pause("按回车继续。");
			Clear();
			Pause("按回车继续。");
			Clear();

			Print($"你突然醒了过来{(Variables.stopReason == "darkness" ? "，黑暗带来的眩晕感还回旋在你头脑中" : "")}{(Variables.stopReason == "cold" ? "，仍然浑身颤抖着" : "")}。\n\n头顶上又是熟识的床板，四周又是熟识的墙壁和窗帘。但是你现在快乐而清晰地看着晨光穿过帘子重新照射着房间，使一切容光焕发。{(Variables.InventoryGet("手电筒") == null ? "" : "你的手电筒仍然放在架子上原位处。")}{(Variables.InventoryGet("水杯") == null ? "" : "你的水杯仍然放在桌子上。")}{(Variables.foundNobody || Variables.foundDarkness1 || Variables.foundWatchStop ? "你肯定做了一场奇怪的梦，梦见自己醒来。" : "")}{(Variables.foundNobody ? "它反正不会是真的；你不可能梦游，因为——你稍稍起身，仿佛是确认一下——所有人都在床上。一个不少。" : "")}你看向手表，{(Variables.foundWatchStop ? "秒针欢快地走着，" : "")}时间是早晨六点十六分。周二。生活的必然性从表盘上三个小小的字母中钻出来。还有很多事要做。周四考试，本来可以放松一下的星期二突然有了很多任务。你翻来覆去地思考这些不能改变的东西，仿佛试着抓住生活的尾巴，让它慢下来。\n\n");

			PrintCentered("* 第一夜结束，按回车继续。 *");
			Pause();

			SecondNight();
		}

		public void SecondNight()
		{
			Clear();
			GameMap.Day2Setting();
			Variables.day = 2;
			Variables.currentRoom = GameMap.Dormitory12;
			Variables.dreamStop = false;

			PrintCentered("\n第二天", true);
			Print("\n\n\n");
			Print(
				$"在夜里某一时刻，你又醒了。或者说你又进入了那个奇怪的梦境。寝室里今晚不像昨天那么苍白，弥散着一种特殊的红晕。你一定是在梦里。借着光晕，你看见对面床上没有人。{(Variables.foundNobody ? "连续两个夜晚：" : "")}非同寻常。然而你并不害怕——这使你自己都有点惊异。你从来不觉得自己胆大，而生活也决定了一种优柔寡断的性格；但现在你的头脑极其清醒。\n\n红晕继续暗淡地洒在床上。好奇心使你难以睡下，今晚一定也要走一圈。\n\n");

			InputLoop();

			Pause("按任意键继续。");
			Clear();
			Pause("按任意键继续。");
			Clear();

			bool cold = Variables.stopReason == "water" || Variables.stopReason == "cold";
			Print($"你{(Variables.stopReason == "darkness" ? "带着一种旋转的感觉" : "突然")}醒来{(cold ? "，感觉很冷" : "")}。\n\n你闭着眼睛等铃响，但是铃没有响。然后你掀开被子，感受十一月早晨微弱的{(cold ? "寒意" : "暖气")}。奇怪，时间还早。甚至六点一刻也没有到。如果我一直停在梦中的世界里，醒来洗漱准备上课的时间是否就永远不会到来？你思考。\n\n");

			PrintCentered("* 第二夜结束，按任意键继续。 *");
			Pause();
		}

		private void InputLoop()
		{
			while (true)
			{
				string h = CRoom.Name;
				if (CArea != null)
					h += "，" + CRoom.CurrentArea.Name;
				SetHeader(h);

				string cmd = Input(">> ");
				ParseInput(cmd);
				DealTemperature();
				if (Variables.dreamStop)
					break;
			}
		}

		private void Save()
		{
			using (BinaryWriter w = new BinaryWriter(
				new FileStream("save.txt", FileMode.OpenOrCreate)))
			{
				Variables.Save(w);
			}
			// todo
		}
	}
}