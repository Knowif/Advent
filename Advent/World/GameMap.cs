using static Advent.Interactive;

namespace Advent
{
	partial class GameMap
	{
		public static Room Dormitory12 = new Room();
		public static Room Restroom = new Room();
		public static Room Balcony = new Room();
		public static Room DormsHallway = new Room();
		public static Room LobbyNo8 = new Room();
		public static Room DarknessRoom = new Room();

		public static void BuildWorld()
		{
			Print("正在建构世界……");

			BuildCommons();
			BuildDormitory12();
			BuildRestroom();
			BuildBalcony();
			BuildDormsHallway();
			BuildLobbyNo8();
			BuildDarknessRoom();

			Print("完毕。\n\n");
		}

		public static void BuildCommons()
		{
			// reusable objects here
			
		}
	}
}
