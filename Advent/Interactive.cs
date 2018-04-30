using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent
{
	static class Interactive
	{
		public static void Print(string text) { Console.Write(text); }

		public static string Input(string prompt = "")
		{
			Console.Write(prompt);
			return Console.ReadLine();
		}

		public static void Clear()
		{
			Console.Clear();
		}
	}
}
