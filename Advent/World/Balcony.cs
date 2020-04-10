namespace Advent
{
	partial class GameMap
	{
		static void BuildBalcony()
		{
			Balcony.Name = "阳台";
			Balcony.Alias.Clear();
			Balcony.IsWarm = false;

			Balcony.LightDescription =
				"小瓷砖铺成的地面在光照下异常耀眼。周围环绕着的金属栏杆增加着这里的狭小感。地面上有两个垃圾桶。你身后是回洗手间过道的移门。风声呼啸。";
			Balcony.Description =
				"没有灯光照明，阳台看上去只是模糊的剪影。你得小心点才不会碰到地上的两个垃圾桶。你身后是回洗手间过道的移门。风声呼啸。";
			Balcony.OnListen = (self, v, p) =>
			{
				Interactive.Print("你听见风的声音。");
				return HandleResult.Continue;
			};

			Balcony.PostDescription = (self, v) =>
			{
				v.foundDarkness1 = true;
				return "";
			};

			AObject TrashBin = new AObject(
				"垃圾桶", new[] { "桶", "垃圾" },
				desc: 		"看上去黑漆漆的。",
				ldesc: 		"一个套着垃圾袋的垃圾桶，里面没有任何东西。")
			{ IsContainer = true };

			AObject Darkness = new AObject(
				"黑暗", new string[0],
				desc: 		"在黑暗中看着黑暗是一种怪异的体验。事实上它并不全是黑暗：它其实在发光，室内窗帘后的微光便是它发出的。看着它使你感到头晕。",
				ldesc: 		"不像是雾，手电筒照不穿它。你只能形容它为“黑暗”：事实上它在发光，室内窗帘后的微光便是它发出的。但你看着它的时候你完全确定它就是一片黑暗，深不可测，使你头晕。")
			{ Information = (s, v) => "栏杆外什么也没有，只是一片深深的、超现实的黑暗。" };

			AObject RestroomDoor = AObject.SimpleDoor(
				Restroom, "去洗手间过道的门", new string[]
					{ "厕所门", "洗手间门", "厕所", "洗手间", "移门", "门" },
				desc: 		"通往洗手间过道的移门。",
				flopside: 	() => Restroom.FindObjectInternal("去阳台的移门"));

			Balcony.Objects.Clear();
			Balcony.Objects.AddRange(new[] { TrashBin, Darkness, RestroomDoor });

			Balcony.DefaultDoor = RestroomDoor;

			Balcony.Objects.Add(new AObject("墙壁", new string[]
				{ "墙", "地面", "地板", "地", "柱子", "栏杆" }));
		}
	}
}