using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Advent.Interactive;

namespace Advent
{
	public class PlayerVariables
	{
		// common variables shared in game

		public Room currentRoom { get; set; }
		public Player player { get; set; }
		public List<AObject> inventory  { get; private set; } = new List<AObject>();

		public int day { get; set; } = 1;
		public int cmdCount { get; set; } = 0;
		public int temperature { get; set; } = 0;
		public bool withClothes { get; set; } = false;

		// night-related

		public bool gotUp1			{ get; set; } = false; // have we gotten up at the first night?
		public bool foundDarkness1	{ get; set; } = false; // have we found the darkness?
		public bool foundNoVoid2	{ get; set; } = false; // have we found there is no longer darkness around us?

		// equipments

		public bool flashlightOK	{ get; set; } = false; // have we gotten the flashlight working?
		public bool firstLight		{ get; set; } = false; // have we seen the flashlight work?

		// personal experiences

		public bool foundLampUnlit	{ get; set; } = false; // have we found lamp has no use?
		public bool foundNobody		{ get; set; } = false; // have we discovered that nobody else is here?
		public bool foundWatchStop	{ get; set; } = false; // have we found our watch has stopped?
		public bool foundDoorsLocked { get; set; } = false; // have we discovered other dorms are locked? 
		public bool foundTimeStop	{ get; set; } = false; // have we found every clock has stopped?

		// map-related experiences

		public bool foundVoidGate	{ get; set; } = false; // have we discovered the darkness outside the gates?
		public bool foundClassroom	{ get; set; } = false; // have we found the lighted classroom?
		public bool triedGetFish	{ get; set; } = false; // have we attempted to get fish from the ponds?

		// each night

		public bool dreamStop		{ get; set; } = false;
		public string stopReason	{ get; set; } = "";
		public bool onceWokeByCold	{ get; set; } = false;

		public void Save(BinaryWriter w)
		{
			w.Write(day);
			w.Write(temperature);
			w.Write(withClothes);

			w.Write(gotUp1);
			w.Write(flashlightOK);
			w.Write(firstLight);
			w.Write(foundLampUnlit);
			w.Write(foundNobody);
			w.Write(foundWatchStop);
			w.Write(foundDarkness1);
			w.Write(foundDoorsLocked);
			w.Write(foundNoVoid2);
		}

		public AObject Watch { get; private set; } = new AObject(
			"手表", new[] { "表", "时间" },
			desc: "手表的荧光指针显示着1:12。",
			ldesc: "手表指着1:12:37，秒针一动不动。")
		{
			OnExaminaion = (self, v) =>
			{
				if (v.IsLight() && !v.foundWatchStop)
				{
					Print($"手表的指针显示1:12。在手电筒的光照下，你清清楚楚地看见秒针一动不动地指在数字7和8中间某处。所以它停了，唯一你能确定的只有现在*至少*是凌晨1:12——你昨天下午看过表，那时它还在走。{(!v.foundDarkness1 && v.day == 1 ? "也许……现在已经接近四五点，也许天空已经开始泛白。你发现自己幻想着：你可以找一个合适的地方，安静地等待日出。那一定会很美丽，因为你从来没有在这种时候待在外面过。" : "")}\n\n");
					v.foundWatchStop = true;
					return HandleResult.FullManaged;
				}
				return HandleResult.Continue;
			}
		};

		public PlayerVariables(Player p)
		{
			player = p;
			inventory.Add(Watch);
		}

		public bool IsLight()
		{
			return currentRoom.IsPlayerLit || currentRoom.IsLit;
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
