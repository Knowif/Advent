using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent
{
	class Utility
	{
		static public string Midstring(string toSub, int startIndex, int length = -1)
		{
			byte[] subbyte = Encoding.Default.GetBytes(toSub);
			if (length == -1) length = subbyte.Length - startIndex;
			string Sub = Encoding.Default.GetString(subbyte, startIndex, length);

			return Sub;
		}


	}
}
