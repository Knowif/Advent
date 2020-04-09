using System.Text;
using System;

namespace Advent
{
	class Utility
	{
		static readonly Random rnd = new Random();

		static public string Midstring(string toSub, int startIndex, int length = -1)
		{
			byte[] subbyte = Encoding.Default.GetBytes(toSub);
			if (length == -1) length = subbyte.Length - startIndex;
			string Sub = Encoding.Default.GetString(subbyte, startIndex, length);

			return Sub;
		}

		static public bool Chance(double probability)
		{
			return rnd.NextDouble() < probability;
		}
	}
}
