using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace Advent
{
	// A software-implemented screen buffer with a header support. When porting to other platforms hopefully this might be the only thing that gets rewritten...
	public static class Interactive
	{
		private static readonly string[] buf;
		private static readonly string padding;
		private static string header = "";
		private static int curX = 0;
		public static bool UnixColoring { get; set; } = true;

		static Interactive()
		{
			buf = new string[WindowHeight - 4];
			for (int i = 0; i < buf.Length; i++)
				buf[i] = "";
			for (int i = 0; i < WindowWidth / 20; i++)
				padding += " ";
		}

		// Puts the software buffer on the screen after clearing.
		public static void Flush()
		{
			string str = ""; // header + "\n";
			for (int i = 0; i < buf.Length; i++)
				if (i == buf.Length - 1)
					str += padding + buf[i];
				else
					str += padding + buf[i] + "\n";
			Console.Clear();
			BackgroundColor = ConsoleColor.White;
			ForegroundColor = ConsoleColor.Black;
			WriteLine(header);
			ResetColor();
			Out.Write(str);
			Out.Flush();
		}

		public static int TextLength(string text, bool styled = true)
		{
			if (text == null)
				throw new ArgumentNullException(nameof(text));
			int textlen = 0;
			foreach (char c in text)
				if (c == '_' && styled) continue;
				else if (c > 127) textlen += 2;
				else textlen += 1;
			return textlen;
		}

		// Set the header with the text in the center. Doesn't flush immediately.
		public static void SetHeader(string text)
		{
			int textlen = TextLength(text);
			int offset = WindowWidth / 2 - textlen / 2;
			if (offset < 0) return;
			header = "";
			for (int i = 0; i < offset; i++)
				header += " ";
			header += text;
			for (int i = 0; i < WindowWidth - offset - textlen; i++)
				header += " ";
		}

		public static void PrintCentered(string text, bool emphasize = false)
		{
			if (text == null)
				throw new ArgumentNullException(nameof(text));
			string[] lines = text.Split('\n');
			for (int i = 0; i < buf.Length - lines.Length; i++)
				buf[i] = buf[i + lines.Length];
			int ix = buf.Length - lines.Length;
			foreach (string line in lines)
			{
				int textlen = TextLength(line, false);
				int offset = WindowWidth / 2 - textlen / 2 - padding.Length;
				buf[ix] = "";
				if (offset < 0) return;
				for (int i = 0; i < offset; i++)
					buf[ix] += " ";
				if (emphasize && UnixColoring) buf[ix] += "\x1b[1;4m";
				buf[ix] += line;
				if (emphasize && UnixColoring) buf[ix] += "\x1b[0m";
				ix++;
			}
			if (emphasize && !UnixColoring)
			{
				string l = "";
				for (int i = 0; i < TextLength(lines.Last()) + 4; i++)
					l += "-";
				PrintCentered(l);
			}
			//Print("\n");
		}

		// Prints something to the software buffer. Doesn't flush immediately.
		public static void Print(string text)
		{
			if (text == null)
				throw new ArgumentNullException(nameof(text));
			if (text.Length == 0) return;

			List<string> temp = new List<string>(buf);
			int pos = curX;
			bool underline = false;
			foreach (char c in text)
			{
				if (c == '_')
				{
					underline = !underline;
					if (UnixColoring)
						temp[temp.Count - 1] += underline ? "\x1b[4m" : "\x1b[0m";
					continue;
				}
				if (c == '\r') continue;
				if (c == '\n') { temp.Add(""); pos = 0; continue; }

				// is it out of screen?
				pos++; if (c > 127) pos++;
				if (pos > WindowWidth - padding.Length * 2) { pos = 1; temp.Add(""); }

				// append it to our temporary buffer
				temp[temp.Count - 1] += c;
			}
			curX = pos;

			// move temp to buffer
			int j = temp.Count - 1;
			for (int i = buf.Length - 1; i >= 0; i--)
			{
				buf[i] = temp[j];
				j -= 1;
				if (j < 0) buf[i] = "";
			}
		}

		/// <summary>
		/// Flushes the buffer and then gets a line of user input.
		/// </summary>
		public static string Input(string prompt = "")
		{
			Print(prompt);
			Flush();
			string line = ReadLine();
			Print(line + "\n");
			Flush();
			return line;
		}

		public static void Pause(string prompt = "")
		{
			Print(prompt);
			Flush();
			//if (UnixColoring) Write("\x1b[?25l");
			CursorVisible = false;
			ReadKey();
			CursorVisible = true;
			//if (UnixColoring) Write("\x1b[?25h");
		}

		/// <summary>
		/// Clears the buffer but doesn't flush immediately.
		/// </summary>
		public static void Clear()
		{
			for (int i = 0; i < buf.Length; i++)
				buf[i] = "";
		}
	}
}
