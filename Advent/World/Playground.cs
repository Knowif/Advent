using System.Collections.Generic;

namespace Advent
{
	partial class GameMap
	{
		static void BuildPlayground()
		{
			Playground.Name = "操场";
			Playground.Alias.Clear();
			Playground.IsLit = true;

			Area Entrance = new Area();
			Area NEntrance = new Area();
			Area FdInstr = new Area();
			Area Court = new Area();
			Area NCourt = new Area();
			Area SCourt = new Area();
			Area TrackN = new Area();
			Area TrackE = new Area();
			Area TrackS = new Area();
			Area TrackW = new Area();
			Area TrackNE = new Area();
			Area TrackNW = new Area();
			Area TrackSE = new Area();
			Area TrackSW = new Area();
			Area NTrackEnd = new Area();
			Area STrackEnd = new Area();
			Area FdNTrack = new Area();
			Area HighPlt = new Area();
			Area NSpectPlt = new Area();
			Area SSpectPlt = new Area();
			Area SpectEndPlt = new Area();
			Area UdHighPlt = new Area();
			Area UdNSpectPlt = new Area();
			Area UdSSpectPlt = new Area();
			Area NPassage = new Area();
			Area SPassage = new Area();
			Playground.Areas.AddRange(new[] { Entrance, NEntrance, FdInstr, Court, NCourt, SCourt, TrackN, TrackE, TrackS, TrackW, TrackNE, TrackNW, TrackSE, TrackSW, NTrackEnd, STrackEnd, FdNTrack, HighPlt, NSpectPlt, SSpectPlt, SpectEndPlt, UdHighPlt, UdNSpectPlt, UdSSpectPlt, NPassage, SPassage });

			SetupArea(ref Entrance, "入口处的空地",
				desc: "你站在操场入口处，环绕场地的栅栏在你东边打开：外面是一条南边走向的大路。往北边，场地通往沙坑和更远处；西边和南边则是一小片围起来的草地。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.N, NEntrance } });
			Entrance.RoomTo.Add(Direction.E, Campus);
			Entrance.OnGoDirection = (s, v, d) =>
			{
				Campus.ChangeArea("操场入口边");
				return HandleResult.Continue;
			};

			// sandpool
			SetupArea(ref NEntrance, "沙坑边的空地",
				desc: "你站在沙坑边，场地向北延伸到单杠器材附近，向南则是操场入口。往西，你可以进入到环绕足球场的跑道上。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.S, Entrance },
					  { Direction.W, TrackE },
					  { Direction.N, FdInstr } });

			SetupArea(ref FdInstr, "单杠附近",
				desc: "你站在单杠和其他器械之间；北边，透过操场的围栏，你只看见一片黑暗；场地向南边延伸，你也可以向西闯过一片草地走向跑道北边的一片空地。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.S, NEntrance },
					  { Direction.W, FdNTrack } });

			SetupArea(ref Court, "足球场上",
				desc: "你站在足球场的草坪上。草长得很茂盛；梦中半暗的天空光照下，它们在脚踝高度投下一片随风颤动的阴影。你可以往南北走向两片铺着塑胶的空地，也可以往东西走向两侧的跑道。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.N, NCourt },
					  { Direction.S, SCourt },
					  { Direction.W, TrackW },
					  { Direction.E, TrackE } });

			SetupArea(ref NCourt, "空地",
				desc: "你站在足球场北边的一小片铺着塑胶的空地上。南边通往足球场，北边是跑道。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.S, Court },
					  { Direction.N, TrackN } });

			SetupArea(ref SCourt, "空地",
				desc: "你站在足球场南边的一小片铺着塑胶的空地上。北边通往足球场，南边是跑道。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.N, Court },
					  { Direction.S, TrackS } });

			SetupArea(ref TrackN, "跑道",
				desc: "你在环绕足球场的塑胶跑道上；弯道向东、西延伸。你也可以横穿跑道，向南或向北走进空地。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.N, FdNTrack },
					  { Direction.S, NCourt },
					  { Direction.W, TrackNW },
					  { Direction.E, TrackNE } });

			SetupArea(ref TrackE, "跑道",
				desc: "你在环绕足球场的塑胶跑道上；直道向南、北延伸。你也可以横穿跑道，向西走进足球场或向东走到沙坑附近。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.N, TrackNE },
					  { Direction.S, TrackSE },
					  { Direction.W, Court },
					  { Direction.E, NEntrance } });

			SetupArea(ref TrackS, "跑道",
				desc: "你在环绕足球场的塑胶跑道上；弯道向东、西延伸。你也可以横穿跑道，向南或向北走进空地。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.N, SCourt },
					  { Direction.W, TrackSW },
					  { Direction.E, TrackSE } });

			SetupArea(ref TrackW, "跑道",
				desc: "你在环绕足球场的塑胶跑道上；直道向南、北延伸。主席台在你西侧的上方；你也可以横穿跑道，向东走进足球场。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.N, TrackNW },
					  { Direction.S, TrackSW },
					  { Direction.W, UdHighPlt },
					  { Direction.E, Court } });

			SetupArea(ref TrackNE, "跑道",
				desc: "你在环绕足球场的塑胶跑道上；这里弯道的拐角向南边和西边延伸。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.S, TrackE },
					  { Direction.W, TrackN } });

			SetupArea(ref TrackNW, "跑道",
				desc: "你在环绕足球场的塑胶跑道上；这里弯道的拐角向北边和西边延伸。你的北面是直跑道延伸出去的末端一截，一百米跑的起点；阶梯状的观众席在你的西侧。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.N, NTrackEnd },
					  { Direction.S, TrackW },
					  { Direction.W, UdNSpectPlt },
					  { Direction.E, TrackN } });

			SetupArea(ref TrackSE, "跑道",
				desc: "你在环绕足球场的塑胶跑道上；这里弯道的拐角向北边和东边延伸。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.N, TrackE },
					  { Direction.W, TrackS } });

			SetupArea(ref TrackSW, "跑道",
				desc: "你在环绕足球场的塑胶跑道上；这里弯道的拐角向北边和东边延伸。你南北面是直跑道延伸出去的末端一截；阶梯状的观众席在你的西侧。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.N, TrackW },
					  { Direction.S, NTrackEnd },
					  { Direction.W, UdSSpectPlt },
					  { Direction.E, TrackS } });

			SetupArea(ref NTrackEnd, "跑道末端",
				desc: "你站在直跑道延伸出去的末端一截，一百米跑的起点附近；南边，跑道汇入围绕足球场的弯道。你也可以向东走向一块空地。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.S, TrackNW },
					  { Direction.E, FdNTrack } });

			SetupArea(ref STrackEnd, "跑道末端",
				desc: "你站在直跑道延伸出去的末端一截；北边，跑道汇入围绕足球场的弯道。你也可以向西上坡走向观众席南端的一个平台。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.W, SpectEndPlt },
					  { Direction.N, TrackSW } });

			SetupArea(ref FdNTrack, "空地",
				desc: "你站在跑道北边的一块空地上；再北侧，操场的栅栏外面只有一片黑暗。你可以向南回到跑道，向西进入一百米跑起点那边直跑道延伸出去的一截，或向东穿过一片草地走向单杠附近。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.S, TrackN },
					  { Direction.W, NTrackEnd },
					  { Direction.E, FdInstr } });

			SetupArea(ref HighPlt, "主席台",
				desc: "你站在主席台的穹顶下，在这个位置你能俯视整个操场；可惜，夜雾阻止了你看得太远。你可以向北或向南回到两边的阶梯观众席上。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.N, NSpectPlt },
					  { Direction.S, SSpectPlt } });
			// should there be a bunch of lights (that can be operated in some obscure place) up there?

			SetupArea(ref NSpectPlt, "观众席（北）",
				desc: "你站在北边的观众席上；这里的阶梯地面空空荡荡，没有任何垃圾或落叶。南边一条道路向上通往主席台；你也可以从东边的台阶下去。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.S, HighPlt },
					  { Direction.E, UdNSpectPlt } });

			SetupArea(ref SSpectPlt, "观众席（南）",
				desc: "你站在南边的观众席上；这里的阶梯地面空空荡荡，没有任何垃圾或落叶。北边一条道路向上通往主席台，南边是观众席末端通向学校西门附近的一个平台；你也可以从东边的台阶下去。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.N, HighPlt },
					  { Direction.E, UdSSpectPlt },
					  { Direction.S, SpectEndPlt } });

			SetupArea(ref SpectEndPlt, "观众席末端平台",
				desc: "你站在观众席南边末端的一个弧形平台上；南边，操场的侧面出入口通向学校西门边的大路。你可以向北进入观众席，或者向东通过阶梯式的地面下去到跑道上。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.N, SSpectPlt },
					  { Direction.E, STrackEnd } });
			SpectEndPlt.RoomTo.Add(Direction.S, Campus);
			SpectEndPlt.OnGoDirection = (s, v, d) =>
			{
				Campus.ChangeArea("通向学校西门的道路");
				return HandleResult.Continue;
			};

			SetupArea(ref UdHighPlt, "主席台前",
				desc: "你站在主席台下（准确来说，东边）的一块小空地上。这里没有直接通向主席台的路。你可以向南或向北走到观众席边。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.N, UdNSpectPlt },
					  { Direction.S, UdSSpectPlt } });

			SetupArea(ref UdNSpectPlt, "观众席前",
				desc: "你站在北侧观众席东边的一块小空地上；阶梯观众席的金属栏杆在你的西侧开口。你可以向南走到主席台前，向东走上跑道，或向西南走穿过主席台底下两条通道中北边的那一条。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.W, NSpectPlt },
					  { Direction.S, UdHighPlt },
					  { Direction.E, TrackNW },
					  { Direction.SW, NPassage } });

			SetupArea(ref UdSSpectPlt, "观众席前",
				desc: "你站在南侧观众席东边的一块小空地上；阶梯观众席的金属栏杆在你的西侧开口。你可以向北走到主席台前，向东走上跑道，或向西北走穿过主席台底下两条通道中南边的那一条。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.W, SSpectPlt },
					  { Direction.N, UdHighPlt },
					  { Direction.E, TrackSW },
					  { Direction.NW, SPassage } });

			SetupArea(ref NPassage, "主席台下的小路",
				desc: "你在一条通道里，通道穿过主席台下向西通往操场后院、向东北通往观众席前。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.NE, UdNSpectPlt } });
			// to back of playground

			SetupArea(ref SPassage, "主席台下的小路",
				desc: "你在一条通道里，通道穿过主席台下向西通往操场后院、向东南通往观众席前。",
				usable: new AObject[] { },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ { Direction.SE, UdSSpectPlt } });
			// to back of playground
		}
	}
}