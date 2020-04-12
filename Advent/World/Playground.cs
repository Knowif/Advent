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

			SetupArea(ref Entrance, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

			SetupArea(ref NEntrance, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

			SetupArea(ref FdInstr, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

			SetupArea(ref Court, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

			SetupArea(ref NCourt, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

			SetupArea(ref SCourt, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

			SetupArea(ref TrackN, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

			SetupArea(ref TrackE, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

			SetupArea(ref TrackS, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

			SetupArea(ref TrackW, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

			SetupArea(ref TrackNE, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

			SetupArea(ref TrackNW, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

			SetupArea(ref TrackSE, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

			SetupArea(ref TrackSW, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

			SetupArea(ref NTrackEnd, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

			SetupArea(ref STrackEnd, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

			SetupArea(ref FdNTrack, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

			SetupArea(ref HighPlt, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

			SetupArea(ref NSpectPlt, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

			SetupArea(ref SSpectPlt, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

			SetupArea(ref SpectEndPlt, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

			SetupArea(ref UdHighPlt, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

			SetupArea(ref UdNSpectPlt, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

			SetupArea(ref UdSSpectPlt, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

			SetupArea(ref NPassage, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

			SetupArea(ref SPassage, "",
				desc: "",
				usable: new AObject[] {  },
				notClear: new AObject[0],
				godir: new Dictionary<Direction, Area>
					{ });

		}
	}
}