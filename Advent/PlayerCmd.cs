﻿using System.Linq;
using static Advent.Interactive;

namespace Advent
{
	partial class Player
	{
		private void ParseInput(string cmd)
		{
			Variables.cmdCount++;

			// if any BeforeCommand present, then use it
			HandleResult res = (CArea != null) ?
				CArea.BeforeCommand(CArea, Variables, cmd) : HandleResult.Continue;
			if (res != HandleResult.Continue) return;
			res = CRoom.BeforeCommand(CRoom, Variables, cmd);
			if (res != HandleResult.Continue) return;

			// as direction ...
			Direction? dir = null;
			if (cmd == "上" || cmd == "上去" || cmd == "上楼")
				dir = Direction.Up;
			else if (cmd == "下" || cmd == "下去" || cmd == "下楼")
				dir = Direction.Down;
			else
			{
				string[] dirs = new[] { "东", "南", "西", "北", "东北", "西北", "东南", "西南" };
				for (int i = 0; i < 8; i++)
					if (dirs[i] == cmd) dir = (Direction)i;
			}
			if (dir != null)
			{
				if (!GoDirection((Direction)dir)) return;
				// only look at PostCommand if succeeded
				res = (CArea != null) ?
					CArea.PostCommand(CArea, Variables, cmd) : HandleResult.Continue;
				if (res != HandleResult.Continue) return;
				CRoom.PostCommand(CRoom, Variables, cmd);
				return;
			}

			// as command ...
			foreach (var x in SingleObjVerbs)
				// FIXME: what System.StringComparison should be used?
				if (cmd.StartsWith(x.Key, System.StringComparison.Ordinal))
				{
					x.Value.Invoke(cmd.Substring(x.Key.Length));

					res = (CArea != null) ?
						CArea.PostCommand(CArea, Variables, cmd) : HandleResult.Continue;
					if (res != HandleResult.Continue) return;
					res = CRoom.PostCommand(CRoom, Variables, cmd);
					if (res != HandleResult.Continue) return;
					return;
				}

			Print("我不理解这个，请尝试不同的表达方法。\n\n");
		}

		private void ParseInversedCmd(string p)
		{
			Print($"In ParseInversedCmd ... {p}\n\n");
			foreach (var v in DoubleObjVerbs)
			{
				if (p.Contains(v.Key))
				{
					int start = p.IndexOf(v.Key, System.StringComparison.Ordinal);
					int end = start + v.Key.Length;
					if (end != p.Length)
					{
						v.Value(p.Substring(0, start), p.Substring(end));
						return;
					}
				}
			}
			foreach (var v in SingleObjVerbs)
			{
				if (p.EndsWith(v.Key, System.StringComparison.Ordinal))
				{
					v.Value(p.Substring(0, p.Length - v.Key.Length));
					return;
				}
			}
			Print("解析器无法处理这个句子。\n\n");
		}

		private void DealTemperature()
		{
			if (!Variables.withClothes && CRoom.IsWarm == false)
			{
				Variables.temperature -= 1;
				if (Variables.temperature <= -4)
				{
					// player wakes by coldness
					if (!Variables.onceWokeByCold)
					{
						Print("风声呼啸……你已经无法移动自己的四肢，感觉身体灌满了水银——不，是干冰。皮肤的刺痛渐渐减弱了，你意识到这是现实……你正在失去知觉，眼中黯淡的光也褪去了。你就像是一个局外人，你想到，正在旁观这可怕的一幕。这时黑暗吞没了你。\n\n");
						Variables.onceWokeByCold = true;
					} else Print("风声呼啸。你已经无法移动自己的四肢。我……又是这一套……我竟然再次忘记要穿衣服。你无声地哀号着，任由黑暗吞没自己的身体。\n\n");
					Variables.dreamStop = true;
					Variables.stopReason = "cold";
				}
				string s = "风吹着。";
				switch (Variables.temperature)
				{
					case -1: s += "你感到有点冷。\n\n"; break;
					case -2: s += "你更冷了，发觉到只穿着内衣，而冷风从深不可测的黑暗中渗透进来。\n\n"; break;
					case -3: s += "寒冷似乎在你身体的骨骼间被一次次注射进去。你颤抖着，甚至难以移动。\n\n"; break;
					default: s += "\n\n"; break;
				}
				Print(s);
			}
		}

		// HELPERS

		private Room CRoom => Variables.currentRoom;
		private Area CArea => Variables.currentRoom.CurrentArea;

		private void DescribeRoom(bool showRoomName = true)
		{
			if (showRoomName)
			{
				string head = CRoom.Name;
				PrintCentered(head, true);
				Print("\n\n");
			}

			if (CRoom.CurrentArea != null)
			{
				Print(CRoom.CurrentArea.Name + "\n\n");
			}

			string areadesc = CArea?.OverrideDescription(CArea, Variables);
			if (!string.IsNullOrEmpty(areadesc)) Print(areadesc + "\n\n");
			else Print(CRoom.GetDescription(CRoom, Variables) + "\n\n");

			Print(CRoom.PostDescription(CRoom, Variables));

			bool hasObj = false;
			foreach (var obj in CRoom.Objects) if (CArea == null 
				|| CArea.FilterObject(obj) == ObjectVisibility.Visible)
			{
				string info = obj.Information(obj, Variables);
				if (!string.IsNullOrEmpty(info))
				{
					Print(info);
					hasObj = true;
				}
			}
			if (hasObj) Print("\n\n");
			var shortInfoObjs = CRoom.Objects
				.Where(x =>  x.ShortInfo != null &&
					(CArea == null || CArea.FilterObject(x) == ObjectVisibility.Visible))
				.Select(x => x.FullShortInfo(x, Variables));
			if (shortInfoObjs.Any()) Print($"这里{(hasObj ? "还" : "")}有{string.Join("、", shortInfoObjs)}。\n\n");
		}

		private void DescribeObject(AObject what)
		{
			HandleResult res = CRoom.BeforeExaminaion(CRoom, Variables, what);
			if (res == HandleResult.Continue)
			{
				res = what.OnExaminaion(what, Variables);
				if (res == HandleResult.Continue)
					if (what.IsNondescript)
						Print(AObject.DefaultDescription + "\n\n");
					else if (CRoom.IsLit || CRoom.IsPlayerLit)
						Print(what.LightDescription(what, Variables) + "\n\n");
					else
						Print(what.Description(what, Variables) + "\n\n");
			}

			res = CRoom.PostExamination(CRoom, Variables, what);

			// go on to provide information for all subobjects
			var shortInfoObjs = what.SubObjects
				.Where(x =>  x.ShortInfo != null &&
					(CArea == null || CArea.FilterObject(x) == ObjectVisibility.Visible))
				.Select(x => x.FullShortInfo(x, Variables));
			if (shortInfoObjs.Any())
				Print($"这里放着{string.Join("、", shortInfoObjs)}。\n\n");
		}

		private bool GoDirection(Direction dir)
		{
			// yes, direction
			HandleResult r = (CArea != null) ?
				CArea.OnGoDirection(CArea, Variables, dir) : HandleResult.Continue;
			if (r == HandleResult.Continue)
			{
				Room tor = null; Area toa = null;
				if (CArea == null)
				{
					if (CRoom.RoomTo.ContainsKey(dir)) tor = CRoom.RoomTo[dir];
				} else
				{
					if (CArea.RoomTo.ContainsKey(dir)) tor = CArea.RoomTo[dir];
					if (CArea.AreaTo.ContainsKey(dir)) toa = CArea.AreaTo[dir];
				}
				if (tor != null)
				{
					Print("你走向那里。\n\n");
					if (CRoom.IsPlayerLit)
					{
						// bring player's torch into the next room
						CRoom.IsPlayerLit = false;
						tor.IsPlayerLit = true;
					}
					Variables.currentRoom = tor;
					// FIXME what's this
					if (CRoom.IsWarm) Variables.temperature = 0;
					DescribeRoom();
					return true;
				} else if (toa != null)
				{
					Print("你走向那里。\n\n");
					CRoom.CurrentArea = toa;
					DescribeRoom(false);
					return true;
				} else
				{
					Print("那里没有路。\n\n");
					return false;
				}
			} else if (r == HandleResult.FullManaged) return true;
			return false;
		}

		private bool Enter(AObject obj)
		{
			if (!obj.IsEnterable)
			{
				Print("你没法进入那里。\n\n");
				return false;
			}
			if (obj.IsOpenable && !obj.OpenState)
			{
				// In case the player automatically attempts to open it but failed
				Print("你进不去，门还没有打开。\n\n");
				return false;
			}
			HandleResult res = obj.OnEntering(obj, Variables);
			if (res == HandleResult.Continue)
			{
				Print($"你走进{obj.Name}。\n\n");

				if (CRoom.IsPlayerLit)
				{
					// bring player's torch into the next room
					CRoom.IsPlayerLit = false;
					obj.RoomTo.IsPlayerLit = true;
				}

				Variables.currentRoom = obj.RoomTo;
				if (CRoom.IsWarm) Variables.temperature = 0;
				DescribeRoom();
			}
			return true;
		}

		private void TurnOnThing(AObject obj)
		{
			if (!obj.IsSwitch)
			{
				Print("这不是一个能够开关的东西。\n\n");
				return;
			}
			if (obj.SwitchState)
				Print("它已经是开着的了。\n\n");
			else
			{
				HandleResult res = CRoom.BeforeTurningOn(CRoom, Variables, obj);
				if (res == HandleResult.Continue)
				{
					res = obj.OnTurningOn(obj, Variables);
					if (res == HandleResult.Continue)
					{
						obj.SwitchState = true;
						Print("你打开了它。\n\n");
					}
				}
				CRoom.PostTurningOn(CRoom, Variables, obj);
			}
		}

		private void TurnOffThing(AObject obj)
		{
			if (!obj.IsSwitch)
			{
				Print("这不是一个能够开关的东西。\n\n");
				return;
			}
			if (!obj.SwitchState)
				Print("它已经是关着的了。\n\n");
			else
			{
				HandleResult res = CRoom.BeforeTurningOff(CRoom, Variables, obj);
				if (res == HandleResult.Continue)
				{
					res = obj.OnTurningOff(obj, Variables);
					if (res == HandleResult.Continue)
					{
						obj.SwitchState = false;
						Print("你关闭了它。\n\n");
					}
				}
				CRoom.PostTurningOff(CRoom, Variables, obj);
			}
		}

		private void OpenThing(AObject obj)
		{
			if (!obj.IsOpenable)
			{
				Print("这不是一个能够开关的东西。\n\n");
				return;
			}
			if (obj.OpenState)
			{
				Print("它已经是开着的了。\n\n");
				return;
			}

			HandleResult res = CRoom.BeforeOpening(CRoom, Variables, obj);
			if (res == HandleResult.Continue)
			{
				if (obj.IsLocked)
				{
					Print("它紧紧锁着。\n\n");
					return;
				}
				res = obj.OnOpening(obj, Variables);
				if (res == HandleResult.Continue)
				{
					obj.OpenState = true;
					AObject flop = obj.LinkedSide();
					if (flop != null) flop.OpenState = obj.OpenState;
					Print("你打开了它。\n\n");
				} else if (res == HandleResult.Refused) return;
			} else if (res == HandleResult.Refused) return;
			CRoom.PostOpening(CRoom, Variables, obj);
		}

		private void CloseThing(AObject obj)
		{
			if (!obj.IsOpenable)
			{
				Print("这不是一个能够开关的东西。\n\n");
				return;
			}
			if (!obj.OpenState)
			{
				Print("它已经是关着的了。\n\n");
				return;
			}

			HandleResult res = CRoom.BeforeClosing(CRoom, Variables, obj);
			if (res == HandleResult.Continue)
			{
				res = obj.OnClosing(obj, Variables);
				if (res == HandleResult.Continue)
				{
					obj.OpenState = false;
					AObject flop = obj.LinkedSide();
					if (flop != null) flop.OpenState = obj.OpenState;
					Print("你关上了它。\n\n");
				} else if (res == HandleResult.Refused) return;
			} else if (res == HandleResult.Refused) return;
			CRoom.PostClosing(CRoom, Variables, obj);
		}

		private AObject FindSensoryObject(string p)
		{
			AObject iobj = Variables.InventoryGet(p);
			if (iobj == null) iobj = CRoom.FindObject(p, Variables);
			if (iobj == null)
			{
				Print($"我不知道{p}是什么，请尝试不同的表达方法。\n\n");
				return null;
			}
			if (!iobj.IsReachable)
			{
				Print($"你够不到{iobj.Name}。真是可惜。\n\n");
				return null;
			}
			return iobj;
		}

		// DOUBLE OBJECT VERBS

		private void PutIn(string a, string b)
		{
			AObject oa = FindSensoryObject(a);
			AObject ob = FindSensoryObject(b);
			if (oa == null || ob == null)
				return;
			if (!ob.IsContainer)
			{
				Print("你没法把东西放到那里面。\n\n");
				return;
			}
			if (oa == ob)
			{
				Print("把一样东西放进自身的想法听上去很荒谬；仿佛在向自己炫耀学识一般，你思考：利用一些拓扑学的把戏，这种行为在四维空间中也许很容易做到……或者，你可以把它弄成一个克莱因瓶……算了。\n\n");
				return;
			}
			// if the object is untakable, it'll pass here but complain when you take it
			if (ob.Capacity - ob.SubObjectsSize < oa.Size)
			{
				Print($"看上去装不下：{(ob.Capacity < oa.Size ? $"对于{ob.Name}来说，{oa.Name}实在太大了。" : $"{ob.Name}里面已经装了太多东西。")}\n\n");
				return;
			}
			// We're trying to ensure that in case the put-in action is simply absurd you don't end up still taking the item.
			if (!Variables.inventory.Contains(oa))
			{
				Take(oa.Name);
				if (!Variables.inventory.Contains(oa))
					return; // failed taking object
			}
			// Now it's okay to put it in
			Variables.inventory.Remove(oa);
			ob.SubObjects.Add(oa);
			oa.Parent = ob;
			Print($"你把{oa.Name}放进了{ob.Name}。\n\n");
		}

		// SINGLE OBJECT VERBS

		private void Wait(string p)
		{
			// We do nothing for now.
			if (Variables.foundWatchStop)
				Print("时间（并不？）流逝。\n\n");
			else
				Print("时间流逝。\n\n");
		}

		private void PutOnClothes(string p)
		{
			if (Variables.withClothes)
			{
				Print("你已经穿上了保暖的衣服和裤子，没有必要再穿了。\n\n");
				return;
			} else if (Variables.InventoryGet("衣服") != null)
			{
				// TODO remove clothes from inventory
				Variables.withClothes = true;
				Print("你穿上衣服。现在保暖了。\n\n");
				return;
			} else if (CRoom.FindObject("衣服", Variables) != null)
			{
				Variables.withClothes = true;
				Print("你穿上一件外套和一件裤子。现在保暖了。\n\n");
				return;
			} else
			{
				Print("你看不见可以穿的衣服。\n\n");
			}
		}

		private void Undress(string p)
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

		private void Take(string p)
		{
			if (string.IsNullOrEmpty(p))
			{
				Print("请说明要拿的是什么。\n\n");
				return;
			}

			AObject obj = CRoom.FindObject(p, Variables);
			if (obj == null)
				Print($"我不知道{p}是什么，请尝试不同的表达方法。\n\n");
			else if (obj.OnTaking(obj, Variables) != HandleResult.Continue)
				return;
			else if (!obj.IsTakable)
				Print($"{obj.Name}不像是你能拿起来的东西。\n\n");
			else
			{
				if (obj.Parent != null)
				{
					obj.Parent.SubObjects.Remove(obj);
					obj.Parent = null;
				} else
					CRoom.Objects.Remove(obj);
				Variables.inventory.Add(obj);
				Print("拿到了。\n\n");
			}
		}

		private void Drop(string p)
		{
			if (string.IsNullOrEmpty(p))
			{
				Print("请说明要放下的是什么。\n\n");
				return;
			}

			AObject obj = Variables.InventoryGet(p);
			AObject obj2 = CRoom.FindObject(p, Variables);
			if (obj == null && obj2 == null)
				Print($"我不知道{p}是什么，请尝试不同的表达方法。\n\n");
			else if (obj == null)
				Print($"你还没有拿到它。");
			else
			{
				// FIXME: doesn't work in multiarea rooms with ruled visibility
				Variables.inventory.Remove(obj);
				CRoom.Objects.Add(obj);
			}
		}

		private void GoTo(string p)
		{
			if (string.IsNullOrEmpty(p))
			{
				Print("请说明要进入的是哪里。\n\n");
				return;
			}

			AObject obj = CRoom.FindObject(p, Variables);
			if (obj == null)
				Print("我不知道你是指哪里，请尝试不同的表达方法。\n\n");
			else
			{
				if (obj.IsOpenable && !obj.OpenState)
					OpenThing(obj);
				Enter(obj);
			}
		}

		private void GoOut(string p)
		{
			if (!string.IsNullOrEmpty(p) && !CRoom.Alias.Contains(p) && CRoom.Name != p)
				Print($"你并不在{p}里，所以无法出去。\n\n");
			else if (CArea?.DefaultDoor == null && CArea?.DefaultOutWay == null 
					&& CRoom.DefaultDoor == null && CRoom.DefaultOutWay == null)
				Print("你并不清楚应该如何出去。\n\n");
			else if (CArea?.DefaultOutWay != null)
				GoDirection((Direction)CArea.DefaultOutWay);
			else if (CArea?.DefaultDoor != null)
			{
				if (!CArea.DefaultDoor.OpenState)
					OpenThing(CArea.DefaultDoor);
				Enter(CArea.DefaultDoor);
			} else if (CRoom.DefaultOutWay != null)
				GoDirection((Direction)CRoom.DefaultOutWay);
			else
			{
				if (!CRoom.DefaultDoor.OpenState)
					OpenThing(CRoom.DefaultDoor);
				Enter(CRoom.DefaultDoor);
			}
		}

		private void TurnOnOrOpen(string p)
		{
			if (string.IsNullOrEmpty(p))
			{
				Print("请说明要开的是什么。\n\n");
				return;
			}

			AObject iobj = Variables.InventoryGet(p);
			AObject obj = CRoom.FindObject(p, Variables);

			if (iobj != null)
				TurnOnThing(iobj);
			else if (obj == null)
				Print("我不知道这是什么，请尝试不同的表达方法。\n\n");
			else if (!obj.IsSwitch && !obj.IsOpenable)
				Print($"{obj.Name}不像是一个能够开关的东西。\n\n");
			else if (obj.IsSwitch)
				TurnOnThing(obj);
			else if (obj.IsOpenable)
				OpenThing(obj);
		}

		private void TurnOffOrClose(string p)
		{
			if (string.IsNullOrEmpty(p))
			{
				Print("请说明要关的是什么。\n\n");
				return;
			}

			AObject iobj = Variables.InventoryGet(p);
			AObject obj = CRoom.FindObject(p, Variables);

			if (iobj != null)
				TurnOffThing(iobj);
			else if (obj == null)
				Print("我不知道这是什么，请尝试不同的表达方法。\n\n");
			else if (!obj.IsSwitch && !obj.IsOpenable)
				Print($"{obj.Name}不像是一个能够开关的东西。\n\n");
			else if (obj.IsSwitch)
				TurnOffThing(obj);
			else if (obj.IsOpenable)
				CloseThing(obj);
		}

		private void LookAt(string p)
		{
			AObject iobj = Variables.InventoryGet(p);
			if (iobj != null)
				DescribeObject(iobj);
			else if (string.IsNullOrEmpty(p) || p == CRoom.Name || CRoom.Alias.Contains(p)
						|| p == "房间" || p == "周围")
				DescribeRoom();
			else
				Examine(p);
		}

		private void Examine(string p)
		{
			AObject iobj = Variables.InventoryGet(p);
			if (iobj != null)
				DescribeObject(iobj);

			AObject obj = CRoom.FindObject(p, Variables);
			if (obj == null)
				Print("我不知道这是什么，请尝试不同的表达方法。\n\n");
			else
				DescribeObject(obj);
		}

		private void Inventory(string p)
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

		private void Touch(string p)
		{
			if (string.IsNullOrEmpty(p))
			{
				Print("请说明要触摸的是什么。\n\n");
				return;
			}

			AObject iobj = FindSensoryObject(p);
			if (iobj != null && iobj.OnBeingTouched(iobj, Variables) != HandleResult.Continue)
				return;
			Print(AObject.DefaultSensoryResponse + "\n\n");
		}

		private void Smell(string p)
		{
			if (string.IsNullOrEmpty(p))
			{
				if (CRoom.OnBeingSmelled(CRoom, Variables, p) == HandleResult.Continue)
					Print(Room.DefaultSensoryResponse + "\n\n");
				return;
			}
			AObject iobj = FindSensoryObject(p);
			if (iobj != null && iobj.OnBeingSmelled(iobj, Variables) == HandleResult.Continue)
				Print(AObject.DefaultSensoryResponse + "\n\n");
		}

		private void Listen(string p)
		{
			if (string.IsNullOrEmpty(p))
			{
				if (CRoom.OnListen(CRoom, Variables, p) == HandleResult.Continue)
					Print(Room.DefaultListenResponse + "\n\n");
				return;
			}
			AObject iobj = FindSensoryObject(p);
			if (iobj != null && iobj.OnBeingListened(iobj, Variables) == HandleResult.Continue)
				Print(AObject.DefaultSensoryResponse + "\n\n");
		}
	}
}