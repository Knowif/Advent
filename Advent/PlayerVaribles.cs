using System.Collections.Generic;
using System.Linq;
using static Advent.Interactive;

namespace Advent
{
	class PlayerVariables
	{
		public Room currentRoom;
		public List<AObject> inventory = new List<AObject>();
		public int temperature = 0;

		public bool withClothes = false;

		public bool gotUp1 = false;
		public bool flashlightOK = false;
		public bool firstLight = false;
		public bool foundLampUnlit = false;
		public bool foundNobody = false;
		public bool foundWatchStop = false;
		public bool foundDarkness1 = false;
		public bool foundDoorsLocked = false;

		public bool dreamStop = false;
		public string stopReason = "";

		public AObject Watch = new AObject(
			"手表", new string[] { "表", "时间" },
			desc:		"手表的荧光指针显示着1:12。",
			ldesc:		"手表指着1:12:37，秒针一动不动。")
		{
			BeforeExaminaion = (self, v) =>
			{
				if (v.IsLight() && !v.foundWatchStop)
				{
					Print(
						"手表的指针显示1:12。在手电筒的光照下，你清清楚楚地看见秒针一动不动地" +
						"指在数字7和8中间某处。所以它停了，唯一你能确定的只有现在*至少*是凌晨" +
						"1:12——你昨天下午看过表，那时它还在走。");
					if (!v.foundDarkness1) Print(
						"也许……现在已经接近四五点，也许天空已经开始泛白。你发现自己幻想着：你" +
						"可以找一个合适的地方，安静地等待日出。那一定会很美丽，因为你从来没有在" +
						"这种时候待在外面过。\n\n");
					else Print("\n\n");
					v.foundWatchStop = true;
					return HandleResult.FullManaged;
				}
				return HandleResult.Continue;
			} };

		public PlayerVariables()
		{
			inventory.Add(Watch);
		}

		public bool IsLight()
		{
			return currentRoom.IsPlayerLight || currentRoom.IsLight;
		}

		public AObject InventoryGet(string name)
		{
			List<AObject> matches = inventory.FindAll(
				(x) => x.Name == name || x.Alias.Contains(name));

			if (matches.Count > 1)
			{
				Print("你所说的“" + name + "”引起了歧义，它可以指：\n");
				foreach (var match in matches)
					Print(" * " + match.Name + "\n");
			} else if (matches.Count == 1)
				return matches[0];

			return null;
		}
	}
}
