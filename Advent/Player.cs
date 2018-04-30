using System;
using System.Collections.Generic;
using System.Linq;
using static Advent.Interactive;

namespace Advent
{
	class Player
	{
		public PlayerVariables Variables;

		private Dictionary<string, Action<string>> VerbTable;
		
		public Player()
		{
			VerbTable = new Dictionary<string, Action<string>>
			{
				{ "等", Wait },

				{ "把", ParseInversedCmd }, { "将", ParseInversedCmd },

				{ "穿衣", PutOnClothes }, {"穿上", PutOnClothes},
				{ "脱衣", Undress}, { "脱下", Undress },

				{ "走进", GoTo }, { "进入", GoTo }, { "去", GoTo },
				{ "进", GoTo }, { "回到", GoTo }, { "回", GoTo },

				{ "拧开", TurnOn }, { "拉开", TurnOn }, { "打开", TurnOn },
				{ "旋开", TurnOn }, { "开启", TurnOn }, { "开", TurnOn },
				{ "关闭", TurnOff }, { "关上", TurnOff }, { "关", TurnOff },

				{ "拿起", Take }, { "拿走", Take}, {"拿", Take},
				{ "取", Take}, {"取走", Take},

				{ "看物品", Inventory }, { "物品", Inventory},
				{ "看", Look }, { "观察", Look },
				{ "检查", Examine }
			};
		}

		public void StartGame()
		{
			// build number
			int build = int.Parse(System.IO.File.ReadAllText("build.txt"));
			Print("Build " + build + ". \n\n\n");
			System.IO.File.WriteAllText("build.txt", (build + 1).ToString());

			Variables = new PlayerVariables();
			GameMap.BuildWorld();

			while (true)
			{
				string ans = Input("是否显示帮助？").ToLower();
				if (ans == "是")
				{
					foreach (var line in Properties.Resources.Help.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.None))
					{
						Print(line + "\n");
						Input();
					}
					break;
				} else if (ans != "否") Print("请输入“是”或“否”。\n");
				else break;
			}

			FirstNight();
			SecondNight();
		}

		public void FirstNight()
		{
			Console.Clear();
			Print(
				"\n第一夜。\n\n\n" +
				"你醒了，有一段时间你甚至没有意识到。周围很暗，只有后面的窗帘中透出的一点路灯光" +
				"使你得以辨认处周围事物的熟悉轮廓。你感到自己醒得不是时候。现在也许是午夜，或者" +
				"是凌晨，反正是一个本应由黑暗、寂静和睡眠主宰的时刻。你不知道你为什么醒来。你一" +
				"点也不冷，身体还保持着刚躺下时的姿势：就好像根本没有睡过一样。\n\n" +
				"提示：起床？看手表？\n\n");
			Variables.currentRoom = GameMap.Dormitory12;

			while (true)
			{
				string cmd = Input(">> ");
				ParseInput(cmd);
				if (Variables.dreamStop)
					break;
			}

			Input("按回车继续。");
			Clear();
			Input("按回车继续。");
			Clear();

			Print("你突然醒了过来");
			if (Variables.gotUp1)
				if (Variables.stopReason == "darkness")
					Print("，这时那黑暗带来的眩晕感还回旋在你头脑中");
				else if (Variables.stopReason == "cold")
					Print("，仍然浑身颤抖着");

			Print(
				"。\n\n头顶上又是熟识的床板，四周又是熟识的墙壁和窗帘。但是你现在快乐而清晰地看" +
				"着晨光穿过帘子重新照射着房间，使一切容光焕发。");
			if (Variables.InventoryGet("手电筒") != null)
				Print("你的手电筒仍然放在架子上原位处。");
			if (Variables.InventoryGet("水杯") != null)
				Print("你的水杯仍然放在桌子上。");

			Print("你肯定做了一场奇怪的梦，梦见自己醒来。");
			if (Variables.foundNobody) Print(
				"它反正不会是真的，你不可能梦游，因为——你稍稍起身，仿佛是确认一下——大家都在床上" +
				"，一个不少。");
			Print("你看向手表，");
			if (Variables.foundWatchStop) Print("秒针欢快地走着，");
			Print(
				"时间是早晨六点十六分。周二：生活的必然性从表盘上三个小小的字母中钻出来：TUE。" +
				"啊，还有很多事要做呢。周四考试，这个本可放松一下的星期二也有了很多任务。你翻来覆" +
				"去地想着，试图抓住一点生活的尾巴，让它慢下来。\n\n");

			Input("第一夜结束。按回车继续。");
		}

		public void SecondNight()
		{
			Console.Clear();
			Print(
				"\n第二夜。");

			Input("第二夜结束。按回车继续。");
		}

		private void ParseInput(string cmd)
		{
			// directions
			Direction? dir = null;
			if (cmd == "上" || cmd == "上去" || cmd == "上楼")
				dir = Direction.Up;
			else if (cmd == "下" || cmd == "下去" || cmd == "下楼")
				dir = Direction.Down;
			else
			{
				string[] dirs = new string[] { "东", "南", "西", "北", "东北", "西北", "东南", "西南" };
				for (int i = 0; i < 8; i++)
					if (dirs[i] == cmd) dir = (Direction) i;
			}
			if (dir != null)
			{
				Doorway door = CRoom().GetDoorway((Direction) dir);
				if (door == null)
				{
					Print("那个方向没有路。\n\n");
					return;
				}
				GoDoor(door);
				return;
			}


			// commands
			HandleResult res = (CArea() != null) ? 
				CArea().BeforeCommand(CArea(), Variables, cmd) : HandleResult.Continue;
			if (res != HandleResult.Continue) return;
			res = CRoom().BeforeCommand(CRoom(), Variables, cmd);
			if (res != HandleResult.Continue) return;

			foreach (var key in VerbTable.Keys)
				if (cmd.StartsWith(key))
				{
					VerbTable[key].Invoke(cmd.Substring(key.Length));
					
					res = (CArea() != null) ?
						CArea().PostCommand(CArea(), Variables, cmd) : HandleResult.Continue;
					if (res != HandleResult.Continue) return;
					res = CRoom().PostCommand(CRoom(), Variables, cmd);
					return;
				}

			res = (CArea() != null) ?
				CArea().PostCommand(CArea(), Variables, cmd) : HandleResult.Continue;
			if (res != HandleResult.Continue) return;
			res = CRoom().PostCommand(CRoom(), Variables, cmd);
			if (res != HandleResult.Continue) return;
			Print("我不理解这个，请尝试不同的表达方法。\n\n");
		}

		private void ParseInversedCmd(string p)
		{
			// e.g. 把食物吃掉
			foreach (var key in VerbTable.Keys)
				if (p.EndsWith(key))
				{
					VerbTable[key](p.Substring(1, p.Length - key.Length - 1));
					return;
				}

			// e.g. 将钥匙插入锁眼
			string[] putIns = new string[] { "放进", "放入", "扔进", "扔给", "插进", "插入", "拿进" };
			foreach (var pi in putIns)
			{
				if (p.Contains(pi))
				{
					string a, b; AObject aa, bb;
					a = p.Substring(1, p.IndexOf(pi) - 1);
					b = p.Substring(p.IndexOf(pi) + pi.Length);
					aa = CRoom().GetObject(a, Variables);
					bb = CRoom().GetObject(b, Variables);
					if (aa == null || bb == null)
						Print("我不知道这是什么，请尝试不同的表达方法。\n\n");
					else if (!bb.CanPutOn)
						Print("你不能这么做。\n\n");
					else
						bb.OnPuttingOn(bb, Variables, aa);
					return;
				}
			}
		}

		// HELPERS

		private Room CRoom() => Variables.currentRoom;
		private Area CArea() => Variables.currentRoom.CurrentArea;

		private void DescribeRoom()
		{
			Print(CRoom().Name);
			if (CArea() != null)
				Print("，" + CRoom().CurrentArea.Name);
			Print("\n==========\n\n");

			HandleResult res = (CArea() != null) ? 
				CArea().OnDescription(CArea(), Variables) : HandleResult.Continue;
			if (res == HandleResult.Continue)
			{
				Print(CRoom().GetDescription(CRoom(), Variables) + "\n\n");

				foreach (var obj in CRoom().Objects)
					if (obj.Information != "") Print(obj.Information + "\n\n");

				foreach (var door in CRoom().Doorways)
					if (door.IsDoor) Print(door.DoorName + (door.IsOpen ? "开着" : "关着") + "。");

				Print("\n\n" + CRoom().PostDescription(CRoom(), Variables));
			}
		}

		private void DescribeObject(AObject obj)
		{
			if (obj == AObject.NoDescObject)
			{
				Print("你没有看到任何特别之处。\n\n");
				return;
			}
			
			HandleResult res = CRoom().BeforeExaminaion(CRoom(), Variables, obj);
			if (res == HandleResult.Continue)
			{
				res = obj.BeforeExaminaion(obj, Variables);
				if (res == HandleResult.Continue)
					if (CRoom().IsLight || CRoom().IsPlayerLight)
						Print(obj.LightDescription + "\n\n");
					else
						Print(obj.Description + "\n\n");
			}

			res = CRoom().PostExamination(CRoom(), Variables, obj);
			if (res == HandleResult.Continue)
				obj.PostExamination(obj, Variables);

			foreach (var subObj in obj.SubObjects)
				Print(subObj.Information + "\n\n");
		}

		private void GoDoor(Doorway door)
		{
			if (!door.IsOpen)
				Print("门还没有打开。\n\n");
			else
			{
				HandleResult res = door.BeforeGoingIn(door, Variables);
				if (res == HandleResult.Continue)
				{
					if (door.IsDoor)
						Print("你走进" + door.DoorName + "。\n\n");
					else
						Print("你走了过去。\n\n");

					if (CRoom().IsPlayerLight)
					{
						CRoom().IsPlayerLight = false;
						door.ConnectTo.IsPlayerLight = true;
					}

					Variables.currentRoom = door.ConnectTo;
					if (CRoom().isWarm) Variables.temperature = 0;
					DescribeRoom();
				}
			}

		}

		private void TurnOnThing(AObject obj)
		{
			if (obj.IsOn)
				Print("它已经是开着的了。\n\n");
			else
			{
				HandleResult res = CRoom().BeforeTurningOn(CRoom(), Variables, obj);
				if (res == HandleResult.Continue)
				{
					res = obj.BeforeTurningOn(obj, Variables);
					if (res == HandleResult.Continue)
					{
						obj.IsOn = true;
						Print("你打开了它。\n\n");
					}
				}
				res = CRoom().PostTurningOn(CRoom(), Variables, obj);
				if (res == HandleResult.Continue)
					obj.PostTurningOn(obj, Variables);
			}
		}

		private void TurnOffThing(AObject obj)
		{
			if (!obj.IsOn)
				Print("它已经是关着的了。\n\n");
			else
			{
				HandleResult res = CRoom().BeforeTurningOff(CRoom(), Variables, obj);
				if (res == HandleResult.Continue)
				{
					res = obj.BeforeTurningOff(obj, Variables);
					if (res == HandleResult.Continue)
					{
						obj.IsOn = false;
						Print("你关闭了它。\n\n");
					}
				}
				res = CRoom().PostTurningOff(CRoom(), Variables, obj);
				if (res == HandleResult.Continue)
					obj.PostTurningOff(obj, Variables);
			}
		}

		private void OpenThing(AObject obj)
		{
			if (obj.IsOpen)
				Print("它已经是开着的了。\n\n");
			else
			{
				HandleResult res = CRoom().BeforeOpening(CRoom(), Variables, obj);
				if (res == HandleResult.Continue)
				{
					res = obj.BeforeOpening(obj, Variables);
					if (res == HandleResult.Continue)
					{
						obj.IsOpen = true;
						Print("你打开了它。\n\n");
					}
				}
				res = CRoom().PostOpening(CRoom(), Variables, obj);
				if (res == HandleResult.Continue)
					obj.PostOpening(obj, Variables);
			}
		}

		private void CloseThing(AObject obj)
		{
			if (!obj.IsOpen)
				Print("它已经是关着的了。\n\n");
			else
			{
				HandleResult res = CRoom().BeforeClosing(CRoom(), Variables, obj);
				if (res == HandleResult.Continue)
				{
					res = obj.BeforeClosing(obj, Variables);
					if (res == HandleResult.Continue)
					{
						obj.IsOpen = false;
						Print("你关上了它。\n\n");
					}
				}
				res = CRoom().PostClosing(CRoom(), Variables, obj);
				if (res == HandleResult.Continue)
					obj.PostClosing(obj, Variables);
			}
		}

		private void OpenDoor(Doorway door)
		{
			HandleResult res = door.BeforeOpening(door, Variables);
			if (res == HandleResult.Continue)
			{
				if (door.IsOpen)
					Print("它已经是开着的了。\n\n");
				else if (door.IsLocked)
					Print("你不能开门，因为它锁着。\n\n");
				else
				{
					door.IsOpen = true;
					if (door.ConnectTo != null)
						foreach (var d in door.ConnectTo.Doorways)
							if (d.ConnectTo == CRoom())
								d.IsOpen = true;

					res = door.PostOpening(door, Variables);
					if (res == HandleResult.Continue) Print("门开了。\n\n");
				}
			}
		}

		private void CloseDoor(Doorway door)
		{
			HandleResult res = door.BeforeClosing(door, Variables);
			if (res == HandleResult.Continue)
			{
				if (!door.IsOpen)
					Print("它已经是关着的了。\n\n");
				else
				{
					door.IsOpen = false;
					if (door.ConnectTo != null)
						foreach (var d in door.ConnectTo.Doorways)
							if (d.ConnectTo == CRoom())
								d.IsOpen = false;

					res = door.PostClosing(door, Variables);
					if (res == HandleResult.Continue) Print("门关了。\n\n");
				}
			}
		}

		// VERBS

		public void Wait(string p)
		{
			// we do nothing for now, maybe time+1 in the future?
		}

		public void PutOnClothes(string p)
		{
			if (Variables.withClothes)
			{
				Print("你已经穿上了保暖的衣服和裤子，没有必要再穿了。\n\n");
				return;
			} else if (Variables.InventoryGet("衣服") != null)
			{
				Variables.withClothes = true;
				Print("你穿上衣服。现在保暖了。\n\n");
				return;
			} else if (CRoom().GetObject("衣服", Variables) != null)
			{
				Variables.withClothes = true;
				Print("你穿上一件外套和一件裤子。现在保暖了。\n\n");
				return;
			} else
			{
				Print("你看不见可以穿的衣服。\n\n");
			}
		}

		public void Undress(string p)
		{
			if (!Variables.withClothes)
			{
				Print("我不觉得你现在会想继续脱下去，对吧……？\n\n");
				return;
			} else
			{
				Variables.withClothes = false;
				Print("你脱下衣服，身上又只剩下了内衣。\n\n");
			}
		}

		public void Take(string p)
		{
			AObject obj = CRoom().GetObject(p, Variables);
			if (obj == null)
				Print("我不知道这是什么，请尝试不同的表达方法。\n\n");
			else if (!obj.IsTakable)
				Print("它不是你能拿起来的东西。\n\n");
			else
			{
				if (obj.Parent != null)
				{
					obj.Parent.SubObjects.Remove(obj);
					obj.Parent = null;
				} else
					CRoom().Objects.Remove(obj);
				Variables.inventory.Add(obj);
				Print("拿到了。\n\n");
			}
		}

		public void GoTo(string p)
		{
			Doorway d = CRoom().GetDoorway(p);
			if (d == null)
				Print("我不知道你是指哪里，请尝试不同的表达方法。\n\n");
			else
				GoDoor(d);
		}

		public void TurnOn(string p)
		{
			Doorway door = CRoom().GetDoorway(p);
			if (door != null)
			{
				OpenDoor(door);
				return;
			}

			AObject iobj = Variables.InventoryGet(p);
			AObject obj = CRoom().GetObject(p, Variables);

			if (iobj != null)
				TurnOnThing(iobj);
			else if (obj == null)
				Print("我不知道这是什么，请尝试不同的表达方法。\n\n");
			else if (!obj.CanOnOff && !obj.Openable)
				Print("这不是一个能够开关的东西。\n\n");
			else if (obj.CanOnOff)
				TurnOnThing(obj);
			else if (obj.Openable)
				OpenThing(obj);
		}

		public void TurnOff(string p)
		{
			Doorway door = CRoom().GetDoorway(p);
			if (door != null)
			{
				CloseDoor(door);
				return;
			}

			AObject iobj = Variables.InventoryGet(p);
			AObject obj = CRoom().GetObject(p, Variables);

			if (iobj != null)
				TurnOffThing(iobj);
			else if (obj == null)
				Print("我不知道这是什么，请尝试不同的表达方法。\n\n");
			else if (!obj.CanOnOff && !obj.Openable)
				Print("这不是一个能够开关的东西。\n\n");
			else if (obj.CanOnOff)
				TurnOnThing(obj);
			else if (obj.Openable)
				CloseThing(obj);
		}

		public void Look(string p)
		{
			AObject iobj = Variables.InventoryGet(p);
			if (iobj != null)
				DescribeObject(iobj);
			else if (p == "" || p == CRoom().Name || CRoom().Alias.Contains(p) 
						|| p == "房间" || p == "周围")
				DescribeRoom();
			else
				Examine(p);
		}

		public void Examine(string p)
		{
			AObject iobj = Variables.InventoryGet(p);
			if (iobj != null)
				DescribeObject(iobj);

			AObject obj = CRoom().GetObject(p, Variables);
			if (obj == null)
				Print("我不知道这是什么，请尝试不同的表达方法。\n\n");
			else
				DescribeObject(obj);
		}

		public void Inventory(string p)
		{
			if (Variables.inventory.Count == 0)
				Print("你什么也没有带。\n\n");
			else
			{
				Print("你身上带着：");
				for (int i = 0; i < Variables.inventory.Count; i++)
					Print(Variables.inventory[i].Name + 
							(i < (Variables.inventory.Count - 1) ? "，" : "。\n\n"));
			}
		}
	}
}