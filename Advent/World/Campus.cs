using System.Collections.Generic;

namespace Advent
{
	partial class GameMap
	{
		static void BuildCampus()
		{
			Campus.Name = "学校";
			Campus.Alias = new string[0];
			Campus.IsLit = true;
			Campus.IsWarm = false;

			// No global descriptors. they are placed in the areas
			Campus.GetDescription = (self, v) => "Should not see this\n\n";

			// Buildings

			AObject Building1 = new AObject(
				"1#楼", new[] { "行政楼", "1#", "大楼", "楼房" },
				desc: 		"1#楼：高大、方形的学校行政楼。六层楼里是大大小小的办公室、校长室（你见过，那里有两层房间，永远不知道其中在发生什么）、机房、电子阅览室、图书馆、“谈国家大事的地方”。东西很多。两边的二、三层都有天桥，通向初中和高中楼。");

			AObject Building2 = new AObject(
				"2#楼", new string[] {
					"高中楼", "高中教学楼", "教学楼", "2#", "大楼", "楼房" },
				desc: 		"2#楼：高中教学楼。与初中教学楼分布在行政楼前广场的两边，互相各成背对着的C字形。你没怎么去过，除了到那里一楼教室上美术课的时候。");

			AObject ReportHall = new AObject(
				"报告厅", new string[] {
					"大厅", "楼房" },
				desc: 		"报告厅与高中教学楼[2#]相连，是凸出于教学楼东南侧的一个宽扁的方块。里面的座椅只够一个年级坐下：更大规模的活动通常在体育馆[6#]或者行政楼[1#]六楼举行。");

			AObject Building3 = new AObject(
				"3#楼", new string[] {
					"初中楼", "初中教学楼", "教学楼", "3#", "大楼", "楼房" },
				desc: 		"3#楼：初中教学楼，你白天的寄居之地。与初中教学楼分布在行政楼前广场的两边，互相各成背对着的C字形。");

			AObject Building4 = new AObject(
				"4#楼", new string[] {
					"外事公寓楼", "外事公寓", "外事楼", "4#", "大楼", "楼房" },
				desc: 		"4#楼：外事公寓楼。");

			AObject Building5 = new AObject(
				"5#楼", new[] { "5#", "大楼", "楼房" },
				desc: 		"5#楼：没什么用的一幢楼。游戏里永远锁着。放在这里只是出于方便。");

			AObject Building6 = new AObject(
				"6#楼", new[] { "体育馆", "6#", "大楼", "楼房" },
				desc: 		"6#楼：体育馆，高高的两层楼相当于其他地方的四层，夏天在这里爬上爬下的确令人泄气。不过它众多楼梯中也隐藏着各种各样的房间。");

			AObject Building7 = new AObject(
				"7#楼", new[] { "食堂", "餐厅", "7#", "大楼", "楼房" },
				desc: 		"7#楼：食堂。半夜里世界上最没用的地方；二楼和三楼几盏应急灯微弱的红光从大窗户里透出来，此外一片黑暗。一楼附带超市，但此刻同样没用：你进不去。");

			AObject Building8 = new AObject(
				"8#楼", new[] { "初中男生寝室楼", "初中寝室楼", "男生寝室楼", "寝室楼", "8#", "大楼", "楼房" },
				desc: 		"8#楼：你最熟悉的寝室楼。灯全部关了，外墙上失去了生活的气息，但那些交替凹凸排列着的窗户和阳台还是令你感到规整而舒适。");

			AObject Building9 = new AObject(
				"9#楼", new[] { "高中男生寝室楼", "高中寝室楼",
					"男生寝室楼", "寝室楼", "9#", "大楼", "楼房" },
				desc: 		"");

			AObject Building10 = new AObject(
				"10#楼", new[] { "高中女生寝室楼", "高中寝室楼",
					"女生寝室楼", "寝室楼", "10#", "大楼", "楼房" },
				desc: 		"");

			AObject Building11 = new AObject(
				"11#楼", new[] { "初中女生寝室楼", "初中寝室楼",
					"女生寝室楼", "寝室楼", "11#", "大楼", "楼房" },
				desc: 		"");

			AObject Playground = new AObject(
				"操场", new[] { "足球场" },
				desc: 		"");

			AObject Garage = new AObject(
				"车库", new string[0],
				desc: 		"");

			AObject BasketballCourt = new AObject(
				"篮球场", new[] { "球场" },
				desc: 		"");

			// other items

			AObject GymPond = new AObject(
				"体育馆边的池塘", new[] { "池塘", "水池", "水" },
				desc: 		"池塘在体育馆东门外延伸成L形。略微浑浊的水面被风吹成网状褶皱，建筑的阴影在其中破碎。几只模糊的鱼在水中移动。");
			AObject SquarePond = new AObject(
				"教学楼边的池塘", new[] { "池塘", "水池", "水" },
				desc: 		"教学楼边的长方形池塘里，略微浑浊的水面被风吹成网状褶皱，天空的颜色在其中破碎。几只模糊的鱼在水中移动。");
			AObject Fish = new AObject(
				"鱼", new string[0],
				desc : "几只鱼模糊地在水中移动。在不明朗的光线中你分辨出它们的样子：红色，黑色，白色，它们的胡须，它们大而呆滞的眼睛。") { IsTakable = true };
			Fish.OnBeingSmelled = GymPond.OnBeingSmelled = (s, v) =>
			{
				Interactive.Print("闻起来像水。\n\n");
				return HandleResult.FullManaged;
			};
			Fish.OnTaking = Fish.OnBeingTouched = (s, v) =>
			{
				if (v.triedGetFish)
				{
					Interactive.Print("你已经徒劳地试过捉鱼了，并且不想再试一遍。\n\n");
					return HandleResult.Refused;
				}
				Interactive.Print("你走到池塘边，蹲下来把手伸进意外冰冷的水里。你的双手有些麻木，而鱼的反应速度比你更快；事实上，你烦躁地发觉自己完全无法从视觉上预测它们在水里的位置。");
				if (Utility.Chance(0.6))
				{
					Interactive.Print("毫无希望地，你缩回手站起来。\n\n");
					v.triedGetFish = true;
				} else
				{
					Interactive.Print("你把手伸得更远，试着跟上几条向远处游走的鱼，然后你突然失去了平衡。在你能够做出反应之前，你已经随着一声巨响被彻骨寒冷的水包围着渗入。你挣扎着想浮起来（你记得池塘看上去很浅），但水看上去充满了整个世界。你无法呼吸……你的视野在暗下来。\n\n");
					v.triedGetFish = true;
					v.dreamStop = true;
					v.stopReason = "water";
				}
				return HandleResult.FullManaged;
			};
			SquarePond.SubObjects.Add(Fish);
			GymPond.SubObjects.Add(Fish);

			AObject Flower = new AObject(
				"花坛", new[] { "花" },
				desc: 		"如迷宫般围绕台阶的花坛有你的一半高；从其中升起的花枝在雾中看起来非常微小，像显微镜下花瓣的纤毛。当然，运动是假象。");

			Campus.Objects.Clear();
			Campus.Objects.AddRange(new AObject[]
			{
				Building1, Building2, Building3, Building4, Building5, Building6, Building7, Building8, Building9, Building10, Building11, Playground, Garage, BasketballCourt,
				GymPond, SquarePond, Flower
			});

			// A = between; Hi = high; Lo = low; Fr = front of; Bk = back of;
			// NESW = n, e, s, w of; t = and; Tri = trivium; Sq = square; Rd = road;

			Area A7t8 = new Area();
			Area HiSq = new Area();
			Area FrHiSq = new Area();
			Area BkHiSq = new Area();
			Area S1Rd = new Area();
			Area TriWS1Rd = new Area();
			Area A1t5 = new Area();
			Area A1t4 = new Area();
			Area A3t5 = new Area();
			Area LoSq = new Area();
			Area A2t4 = new Area();
			Area FrRepHl = new Area();
			Area A7t4 = new Area();
			Area Fr7 = new Area();
			Area TriEGate = new Area();
			Area EA7t8 = new Area();
			Area ERepHl = new Area();
			Area EGym = new Area();
			Area FrGym = new Area();
			Area EPlg = new Area();
			Area A5tPlg = new Area();
			Area NA5tPlg = new Area();
			Area TriWGate = new Area();
			Area RdWGate = new Area();
			Area TriFrWGate = new Area();
			Area W6 = new Area();
			Area BkGym = new Area();
			Area SE6Brg = new Area();
			Campus.Areas.Clear();
			Campus.Areas.AddRange(new Area[] {
				A7t8, HiSq, FrHiSq, BkHiSq, S1Rd, TriWS1Rd, A1t5, A1t4, A3t5, LoSq, A2t4, FrRepHl, A7t4, Fr7, TriEGate, EA7t8, ERepHl, EGym, FrGym, EPlg, A5tPlg, NA5tPlg, TriWGate, RdWGate, TriFrWGate, W6, BkGym, SE6Brg, DarknessArea
			});

			AObject doorA7t8 = AObject.SimpleDoor(
				dest: 		LobbyNo8,
				name: 		"去8#寝室楼大厅的门",
				alias: 		new[] { "里面", "室内", "大门", "门", "门内", "寝室楼", "大厅" },
				desc: 		"通向室内的门。寝室楼大厅的灯光在里面苍白地亮着。",
				open: 		true,
				flopside: 	() => LobbyNo8.FindObjectInternal("外面"));
			Campus.Objects.Add(doorA7t8);
			SetupArea(ref A7t8, "食堂和寝室楼之间",
				desc: 		"你站在食堂[7#]与你的寝室楼[8#]之间的空地上。东侧的远处围栏前停着几辆车。西侧通往一个小广场，几棵灰暗的大树后面显示出体育馆高大的影子。",
				usable: 	new[] { Building7, Building8, doorA7t8 },
				notClear: 	new[] { Building6 },
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.W, HiSq },
							  { Direction.E, EA7t8} },
				noDesc: 		new[] { "车", "树", "影子" });

			SetupArea(ref HiSq, "寝室楼前的小广场",
				desc: 		"你站在小广场的树下，浓荫掩蔽了光线，南边周围是一大片花坛。这里还属于“高地”，所以去北侧的行政楼[1#]就要走下台阶。西侧就是体育馆[6#]，入口前有一个小池塘；在东边你能望见食堂[7#]和8#寝室楼。女生寝室楼[10#, 11#]在南边，还要上一段台阶才能走到。",
				usable: 	new[] { Building6, Building8, Flower },
				notClear: 	new[] { Building1, Building7, GymPond },
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.E, A7t8 },
							  { Direction.W, EGym },
							  { Direction.N, FrHiSq },
							  { Direction.S, BkHiSq } });

			SetupArea(ref BkHiSq, "台阶",
				desc: 		"你站在花坛之间的台阶上，被雾中漂浮的不明确微细枝条所包围。北边立着小广场中间巨大的树。花坛的小路之外，四幢寝室楼在南面和东面形成半包围的结构。",
				usable: 	new[] { Building1, Building7, Flower },
				notClear: 	new[] { Building6, Building8, Building10, GymPond },
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.N, HiSq } });

			SetupArea(ref FrHiSq, "台阶",
				desc: 		"你站在连接学校南北两部分的台阶上，向北面对行政楼[1#]，向南面对小广场。广场中央那棵巨大的树在这里看上去像一枚在雾霭中漂浮的榛子。",
				usable: 	new[] { Building1, Building7 },
				notClear: 	new[] { Building6 },
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.S, HiSq },
							  { Direction.N, S1Rd } });

			AObject doorS1Rd = AObject.SimpleDoor(
				dest: 		Building1F1,
				name: 		"行政楼的南门",
				alias: 		new[] { "里面", "室内", "大门", "门", "门内", "行政楼" },
				desc: 		"通向行政楼室内的门。",
				open: 		false,
				flopside: 	() => Building1F1.FindObjectInternal("南侧大门"));
			doorS1Rd.OnEntering = (s, v) =>
			{
				Building1F1.CurrentArea = Building1F1.FindArea("大厅南侧");
				return HandleResult.Continue;
			};
			Campus.Objects.Add(doorS1Rd);
			SetupArea(ref S1Rd, "道路",
				desc: 		"你站在一条东西走向的大路上。这条路很长；顺着西边望去能看见操场，东边的另一头能看见食堂[7#]。北边侧对着行政楼[1#]入口，南边有台阶通向高地，那里有寝室和体育馆。你也可以向东北拐到行政楼[1#]和外事公寓[4#]之间。在你的头顶上方，树荫遮住了一半天空，紫红色的微光从树叶缝隙里漏下来。西端体育馆那边的墙上爬着稀疏的藤蔓。",
				usable: 	new[] { Building1, Building7, doorS1Rd },
				notClear: 	new[] { Building6 },
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.S, FrHiSq },
							  { Direction.W, TriWS1Rd },
							  { Direction.E, Fr7 },
							  { Direction.NE, A1t4 } },
				noDesc: 		new[] { "藤蔓", "树" });

			SetupArea(ref TriWS1Rd, "三岔路口",
				desc: 		"你站在一条东西走向的大路上，西边延伸直到操场，东边的另一头延伸直到食堂[7#]。这个路口的北侧在行政楼[1#]和国际部[5#]之间分出一条过道。",
				usable: 	new[] { Building1, Building5 },
				notClear: 	new[] { Building6 },
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.E, S1Rd },
							  { Direction.W, EPlg },
							  { Direction.N, A1t5 } });

			AObject doorA1t5_1 = AObject.SimpleDoor(
				dest: 		null,
				name: 		"行政楼西面的小门",
				alias: 		new[] { "里面", "室内", "大门", "门", "门内", "行政楼" },
				desc: 		"行政楼侧面通往楼梯间的门。",
				open: 		false,
				locked: true);
			AObject doorA1t5_2 = AObject.SimpleDoor(
				dest: 		null,
				name: 		"国际部东门",
				alias: 		new[] { "里面", "室内", "大门", "门", "门内", "国际部" },
				desc: 		"通往国际部室内的大门。",
				open: 		false,
				locked: true);
			Campus.Objects.Add(doorA1t5_1);
			Campus.Objects.Add(doorA1t5_2);
			SetupArea(ref A1t5, "行政楼侧面",
				desc: 		"你站在行政楼[1#]和国际部[5#]之间的道路上。北边是初中教学楼[3#]下的空地，南边通向一条东西走向的大路。5#楼的大门在你西侧；东边有一个1#楼的侧门。",
				usable: 	new[] { Building1, Building5, doorA1t5_1, doorA1t5_2 },
				notClear: 	new[] { Building3 },
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.S, TriWS1Rd },
							  { Direction.N, A3t5 } });

			AObject doorA1t4_1 = AObject.SimpleDoor(
				dest: 		null,
				name: 		"行政楼东面的小门",
				alias: 		new[] { "里面", "室内", "大门", "门", "门内", "行政楼" },
				desc: 		"行政楼侧面通往楼梯间的门。",
				open: 		false,
				locked: true);
			AObject doorA1t4_2 = AObject.SimpleDoor(
				dest: 		null,
				name: 		"外事公寓西门",
				alias: 		new[] { "里面", "室内", "大门", "门", "门内", "外事公寓", "公寓" },
				desc: 		"通往外事公寓室内的大门。",
				open: 		false,
				locked: true);
			Campus.Objects.Add(doorA1t4_1);
			Campus.Objects.Add(doorA1t4_2);
			SetupArea(ref A1t4, "行政楼侧面",
				desc: 		"你站在行政楼[1#]和外事公寓[4#]之间的道路上。北边是高中教学楼[2#]和报告厅附近的空地，西南通向一条东西走向的大路，东南通向食堂一带。4#楼的大门在你东侧；西边有一个1#楼的侧门。",
				usable: 	new[] { Building1, Building4, doorA1t4_1, doorA1t4_2 },
				notClear: 	new[] { Building2 },
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.SW, S1Rd },
							  { Direction.SE, Fr7 },
							  { Direction.N, A2t4 } });

			AObject doorA3t5 = AObject.SimpleDoor(
				dest: 		null,
				name: 		"初中教学楼边门",
				alias: 		new[] { "里面", "室内", "大门", "门", "门内", "初中教学楼", "教学楼" },
				desc: 		"边门经过一段走廊通向高中教学楼。",
				open: 		false,
				locked: true);
			Campus.Objects.Add(doorA3t5);
			SetupArea(ref A3t5, "教学楼侧面",
				desc: 		"你站在初中教学楼[3#]和国际部[5#]之间宽阔的道路上；这里更像一块供人来往活动的空地。西边通向一条操场边南北走向的大路。在东边，教学楼三、四楼的天桥从你的上方经过进入行政楼[1#]；你可以向东从天桥下走向教学楼间的广场。南边有一条路通往行政楼[1#]和国际部[5#]之间。",
				usable: 	new[] { Building3, Building5, doorA3t5 },
				notClear: 	new[] { Building1 },
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.S, A1t5 },
							  { Direction.E, LoSq } });

			AObject doorLoSq_1 = AObject.SimpleDoor(
				dest: 		null,
				name: 		"初中教学楼正门",
				alias: 		new[] { "里面", "室内", "大门", "门", "门内", "初中教学楼", "教学楼" },
				desc: 		"楼边池塘上一座小桥后面，初中教学楼的正门。",
				open: 		false,
				locked: true);
			AObject doorLoSq_2 = AObject.SimpleDoor(
				dest: 		null,
				name: 		"高中教学楼正门",
				alias: 		new[] { "里面", "室内", "大门", "门", "门内", "高中教学楼", "教学楼" },
				desc: 		"楼边池塘上一座小桥后面，高中教学楼的正门。",
				open: 		false,
				locked: true);
			AObject doorLoSq_3 = AObject.SimpleDoor(
				dest: 		Building1F1,
				name: 		"行政楼的正门",
				alias: 		new[] { "里面", "室内", "大门", "门", "门内", "行政楼" },
				desc: 		"通向行政楼室内的门。",
				open: 		false,
				flopside: 	() => Building1F1.FindObjectInternal("北侧大门"));
			doorLoSq_3.OnEntering = (s, v) =>
			{
				Building1F1.CurrentArea = Building1F1.FindArea("大厅北侧");
				return HandleResult.Continue;
			};
			Campus.Objects.Add(doorLoSq_1);
			Campus.Objects.Add(doorLoSq_2);
			Campus.Objects.Add(doorLoSq_3);
			SetupArea(ref LoSq, "教学楼间的小广场",
				desc: 		"你在教学楼间平坦的广场上，无树的夜晚天空广阔地显现在你的头顶，投下奇异的光照。旗杆是这里唯一高的事物；你注意到上面没有旗帜。行政楼的正门在广场的南面；向西、向东分别可以从三、四楼的天桥下走向初中、高中教学楼的侧门；当然，广场前部侧面的教学楼正门也可进入。",
				usable: 	new[] { Building1, Building2, Building3, doorLoSq_1, doorLoSq_2, 
					doorLoSq_3, SquarePond },
				notClear: 	new AObject[0],
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.W, A3t5 },
							  { Direction.E, A2t4 } });

			AObject doorA2t4 = AObject.SimpleDoor(
				dest: 		null,
				name: 		"高中教学楼边门",
				alias: 		new[] { "里面", "室内", "大门", "门", "门内", "高中教学楼", "教学楼" },
				desc: 		"边门经过一段走廊通向高中教学楼。",
				open: 		false,
				locked: true);
			Campus.Objects.Add(doorA2t4);
			SetupArea(ref A2t4, "教学楼侧面",
				desc: 		"你站在高中教学楼[2#]和外事公寓[4#]之间宽阔的道路上；这里更像一块供人来往活动的空地。东侧不远处你能看见高中楼一楼附带的报告厅及其入口的方块形状（入口在外面）；再往东就到了围墙附近。在西边，教学楼三、四楼的天桥从你的上方经过进入行政楼[1#]；你可以向西从天桥下走向教学楼间的广场。南边有一条路通往行政楼[1#]和外事公寓[4#]之间。",
				usable: 	new[] { Building2, Building5, ReportHall, doorA2t4 },
				notClear: 	new[] { Building1 },
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.S, A1t4 },
							  { Direction.E, FrRepHl },
							  { Direction.W, LoSq } });

			AObject doorFrRepHl = AObject.SimpleDoor(
				dest: 		null,
				name: 		"报告厅大门",
				alias: 		new[] { "里面", "室内", "大门", "门", "门内", "报告厅", "大厅" },
				desc: 		"报告厅的大门大多数时候都是锁着的；透过玻璃你只能隐约可见几排昏暗的座椅。",
				open: 		false,
				locked: true);
			Campus.Objects.Add(doorFrRepHl);
			SetupArea(ref FrRepHl, "报告厅入口",
				desc: 		"你站在高中楼一楼报告厅的入口附近，高中教学楼[2#]外的空地中。外事公寓[4#]在你的南边显现出侧面；东边通往围墙边的道路。你也可以向西朝教学楼入口那边行走。",
				usable: 	new[] { Building2, Building5, ReportHall, doorFrRepHl },
				notClear: 	new[] { Building4 },
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.W, A2t4 },
							  { Direction.E, ERepHl } });

			AObject doorFr7 = AObject.SimpleDoor(
				dest: 		null,
				name: 		"食堂大门",
				alias: 		new[] { "里面", "室内", "大门", "门", "门内", "食堂" },
				desc: 		"一段台阶后面的食堂正门，透过玻璃只能看见比天色更为黑暗的一团方块。",
				open: 		false,
				locked: true);
			Campus.Objects.Add(doorFr7);
			SetupArea(ref Fr7, "食堂前的大路",
				desc: 		"你站在一条东西走向的大路上。你的南边是食堂[7#]的台阶和大门，在夜里看上去是比天色更为黑暗的一团方块；北边，外事公寓[4#]以其侧面阻挡了你的视线。西边，道路直通向操场方向；你也可以向东到达一个三岔路口，或者向西北拐到4#楼的另一面。",
				usable: 	new[] { Building4, Building7, doorFr7 },
				notClear: 	new[] { Building1 },
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.W, S1Rd },
							  { Direction.E, TriEGate },
							  { Direction.NW, A1t4 } });

			SetupArea(ref TriEGate, "东门边的三岔路口",
				desc: 		"你站在一条东西走向的大路的尽头，东边本应是学校的侧门，但此刻——虽然有路灯和天空的亮光照耀——那里只是一片令人不寒而栗的黑暗。南北两面各有围墙边的道路环绕学校；你也可以从西面走到食堂[7#]和外事公寓[4#]之间。",
				usable: 	new[] { Building7 },
				notClear: 	new[] { Building4 },
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.W, Fr7 },
							  { Direction.E, DarknessArea },
							  { Direction.N, ERepHl },
							  { Direction.S, EA7t8 } });

			SetupArea(ref EA7t8, "围墙边的道路",
				desc: 		"你站在一条南北走向的道路上，东边紧靠学校的围墙。西边，食堂[7#]和8#寝室楼之间有大路直通向寝室楼前的小广场。围墙外只有一片超现实、不透明的黑暗。",
				usable: 	new AObject[0],
				notClear: 	new[] { Building7, Building8 },
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.N, TriEGate },
							  { Direction.W, A7t8 } });

			SetupArea(ref ERepHl, "围墙边的道路",
				desc: 		"你站在一条南北走向的道路上，东边紧靠学校的围墙。西边，高中教学楼[2#]报告厅和食堂之间分岔出一块空地直通向行政楼[1#]一带。东边的围墙外只有一片超现实、不透明的黑暗。",
				usable: 	new AObject[0],
				notClear: 	new[] { Building4, ReportHall, Building2 },
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.S, TriEGate },
							  { Direction.W, FrRepHl } });
			// TODO: a few cars

			AObject doorEGym = AObject.SimpleDoor(
				dest: 		null,
				name: 		"体育馆的侧门",
				alias: 		new[] { "里面", "室内", "大门", "门", "门内", "体育馆" },
				desc: 		"体育馆的侧门，透过玻璃能看见苍白昏暗的楼梯间。",
				open: 		false,
				locked: true);
			Campus.Objects.Add(doorEGym);
			SetupArea(ref EGym, "体育馆边",
				desc: 		"你站在体育馆边上。小广场在你东侧，西边则是体育馆[6#]的侧门；门前有一个长方形池塘。你也可以往西北绕过侧门来到正门前的平地上，或是向南绕过花坛边缘，那边远处是一排寝室楼。",
				usable: 	new[] { Building6, GymPond, doorEGym },
				notClear: 	new[] { Building7, Building8, Building9, Building10, Flower },
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.E, HiSq },
							  { Direction.NW, FrGym },
							  { Direction.S, SE6Brg } });

			AObject doorFrGym = AObject.SimpleDoor(
				dest: 		null,
				name: 		"体育馆的大门",
				alias: 		new[] { "里面", "室内", "大门", "门", "门内", "体育馆" },
				desc: 		"一段台阶后是体育馆的大门；里面的宽阔空间没有开灯，透过玻璃看不清楚。",
				open: 		false,
				locked: true);
			Campus.Objects.Add(doorFrGym);
			SetupArea(ref FrGym, "体育馆前的平台",
				desc: 		"你站在体育馆[6#]前的平台上。这是石板铺成的一大块平地，北边的栏杆之外，你能看见行政楼[1#]和5#楼，它们处于低地，那边无法直接走上来。平地的西边有台阶下去通向大路，路的另一边就是操场和篮球场。你也可以向东南走到体育馆[6#]东边的侧门处，小广场附近。",
				usable: 	new[] { Building6, doorFrGym },
				notClear: 	new[] { Building1, Building5 },
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.SE, EGym },
							  { Direction.W, TriWGate } });

			AObject doorEPlg = AObject.SimpleDoor(
				dest: 		null,
				name: 		"操场入口",
				alias: 		new[] { "里面", "大门", "西", "入口", "操场" },
				desc: 		"环绕操场的围栏在这边入口处打开。",
				open: 		false, // TODO: open that!
				locked: true);
			doorEPlg.Information = doorEPlg.Description;
			doorEPlg.IsOpenable = false;
			Campus.Objects.Add(doorEPlg);
			SetupArea(ref EPlg, "操场入口边",
				desc: 		"你站在一条南北走向的大路上，操场的入口在你的西侧。另一边的道路直通向食堂[7#]。南边大路直通向篮球场。",
				usable: 	new[] { Playground, Building5, doorEPlg },
				notClear: 	new[] { BasketballCourt, Building6 },
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.E, TriWS1Rd },
							  { Direction.N, A5tPlg },
							  { Direction.S, TriWGate } });

			AObject doorA5tPlg = AObject.SimpleDoor(
				dest: 		null,
				name: 		"国际部的西门",
				alias: 		new[] { "里面", "室内", "大门", "门", "门内", "国际部" },
				desc: 		"通往国际部室内的门。",
				open: 		false,
				locked: true);
			Campus.Objects.Add(doorA5tPlg);
			SetupArea(ref A5tPlg, "操场边的道路",
				desc: 		"你站在一条南北走向的道路上，西边围栏之外就是操场，东边面对国际部[5#]大楼的后门。",
				usable: 	new[] { Playground, Building5, doorA5tPlg },
				notClear: 	new AObject[0],
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.S, EPlg },
							  { Direction.N, NA5tPlg } });

			SetupArea(ref NA5tPlg, "操场边的道路",
				desc: 		"你站在一条南北走向的道路上，西边围栏之外就是操场，东边在初中教学楼[3#]和国际部[5#]之间分岔出一条大路。",
				usable: 	new[] { Playground, Building5 },
				notClear: 	new AObject[0],
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.S, A5tPlg },
							  { Direction.E, A3t5 } });

			SetupArea(ref TriWGate, "岔路口",
				desc: 		"你站在一条南北走向的大路上，西边在足球场和篮球场之间分出另一条路通向学校西门，东边有一条坡道通往体育馆前的空地。",
				usable: 	new[] { Building6, Playground, BasketballCourt },
				notClear: 	new AObject[0],
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.E, FrGym },
							  { Direction.W, RdWGate },
							  { Direction.N, EPlg },
							  { Direction.S, W6 } });

			AObject doorRdWGate = AObject.SimpleDoor(
				dest: 		null,
				name: 		"操场南侧入口",
				alias: 		new[] { "里面", "大门", "北", "入口", "操场" },
				desc: 		"北边，一个侧面入口通向操场观众席的露天斜坡。",
				open: 		false, // TODO: should be true
				locked: true);
			doorRdWGate.Information = doorRdWGate.Description;
			doorRdWGate.IsOpenable = false;
			Campus.Objects.Add(doorRdWGate);
			SetupArea(ref RdWGate, "通向学校西门的道路",
				desc: 		"你站在足球场和篮球场之间的路上，西边通往你通常在那里搭校车回家的西门，但此刻——虽然有路灯和天空的亮光照耀——那里只是一片令人不寒而栗的黑暗。你也可以从东面退回学校。",
				usable: 	new[] { doorRdWGate },
				notClear: 	new[] { Playground, BasketballCourt },
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.W, TriFrWGate },
							  { Direction.E, TriWGate } });

			SetupArea(ref TriFrWGate, "西门前的三岔路口",
				desc: 		"你站在足球场和篮球场之间道路上的尽头，西边（那里原本是学校西门）紧邻着一片令人不寒而栗的黑暗。道路向东面延伸退回学校，另有一条小路在北面分岔到操场后面。",
				usable: 	new AObject[0],
				notClear: 	new[] { Playground, BasketballCourt },
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.W, DarknessArea },
							  { Direction.E, RdWGate } });

			AObject doorW6 = AObject.SimpleDoor(
				dest: 		null,
				name: 		"体育馆侧面楼梯",
				alias: 		new[] { "入口", "东", "楼梯", "体育馆" },
				desc: 		"体育馆侧面延伸到路边的楼梯，向上通往体育馆二楼。",
				open: 		false, // TODO: should be true
				locked: true);
			doorW6.Information = (s, v) => "";
			doorW6.IsOpenable = false;
			Campus.Objects.Add(doorW6);
			SetupArea(ref W6, "篮球场边的道路",
				desc: 		"你站在一条南北走向的道路上，西边围栏之外就是室外篮球场，东边面对体育馆[6#]侧面延伸到路边的楼梯。楼梯向上通往体育馆二楼；一楼没有侧门，只有几扇带防护栏的窗户。你也可以向东南拐到体育馆后面。",
				usable: 	new[] { BasketballCourt, Building6, doorW6 },
				notClear: 	new[] { Playground },
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.N, TriWGate },
							  { Direction.SE, BkGym } });

			AObject doorBkGym = AObject.SimpleDoor(
				dest: 		null,
				name: 		"体育馆的后门",
				alias: 		new[] { "里面", "室内", "大门", "门", "门内", "体育馆" },
				desc: 		"体育馆的后门，透过玻璃能看见苍白昏暗的楼梯间。",
				open: 		false,
				locked: true);
			Campus.Objects.Add(doorBkGym);
			SetupArea(ref BkGym, "体育馆后的空地",
				desc: 		"你站在体育馆[6#]南边；这是种有树木的一块平地，东边是一座向北通向体育馆侧面池塘上的木板桥的入口，西北通向篮球场边的道路。你也可以向南，横穿过花坛走向空地靠近初中女生寝室楼[11#]附近的一边。",
				usable: 	new[] { Building6, Flower, Building11, GymPond, doorBkGym },
				notClear: 	new[] { BasketballCourt, Building10 },
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.NW, W6 },
							  { Direction.E, SE6Brg } });

			SetupArea(ref SE6Brg, "木板桥上",
				desc: 		"你站在一座体育馆边池塘的木板桥上，北边通往体育馆侧面附近的地面，南边通向初中女生寝室楼[11#]附近。你也可以往西去向体育馆南边的空地。",
				usable: 	new[] { Building6, Flower, GymPond, Building11 },
				notClear: 	new[] { Building9, Building10 },
				godir: 		new Dictionary<Direction, Area>
							{ { Direction.N, EGym },
							  { Direction.W, BkGym }});
		}
	}
}