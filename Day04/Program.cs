using System;
using System.IO;
using System.Linq;

namespace Day04
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Advent of Code 2024: Day 4");
			var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();

			PartA(puzzleInputRaw);
			PartB(puzzleInputRaw);
		}

		private static void PartA(List<string> puzzleInputRaw)
		{
			Console.WriteLine("\r\n**********");
			Console.WriteLine("* Part A");

			var phraseToFind = "XMAS";
			var linesToCheck = puzzleInputRaw.ToList();

			// build the vertical search lines
			for (var c = 0; c < puzzleInputRaw[0].Length; c++)
			{
				var verticalLine = string.Empty;
				
				for (var r = 0; r < puzzleInputRaw.Count; r++)
				{
					verticalLine += puzzleInputRaw[r][c].ToString();
				}

				linesToCheck.Add(verticalLine);
			}

			// build the diagonal search lines / (the top-left half)
			for (var r = 0; r < puzzleInputRaw.Count; r++)
			{
				var diagonalLine = string.Empty;
				var c = 0;
				
				for (var r2 = r; r2 >= 0; r2--)
				{
					diagonalLine += puzzleInputRaw[r2][c].ToString();
					c++;
				}

				if (diagonalLine.Length >= phraseToFind.Length)
					linesToCheck.Add(diagonalLine);
			}

			// build the diagonal search lines / (the bottom-right half)
			for (var c = 1; c < puzzleInputRaw[0].Length; c++)
			{
				var diagonalLine = string.Empty;
				var r = puzzleInputRaw.Count - 1;

				for (var c2 = c; c2 < puzzleInputRaw[0].Length; c2++)
				{
					diagonalLine += puzzleInputRaw[r][c2].ToString();
					r--;
				}

				if (diagonalLine.Length >= phraseToFind.Length)
					linesToCheck.Add(diagonalLine);
			}

			// build the diagonal search lines \ (the bottom-left half)
			for (var r = 0; r < puzzleInputRaw.Count; r++)
			{
				var diagonalLine = string.Empty;
				var c = 0;

				for (var r2 = r; r2 < puzzleInputRaw.Count; r2++)
				{
					diagonalLine += puzzleInputRaw[r2][c].ToString();
					c++;
				}

				if (diagonalLine.Length >= phraseToFind.Length)
					linesToCheck.Add(diagonalLine);
			}

			// build the diagonal search lines \ (the top-right half)
			for (var c = 1; c < puzzleInputRaw[0].Length; c++)
			{
				var diagonalLine = string.Empty;
				var r = 0;

				for (var c2 = c; c2 < puzzleInputRaw[0].Length; c2++)
				{
					diagonalLine += puzzleInputRaw[r][c2].ToString();
					r++;
				}

				if (diagonalLine.Length >= phraseToFind.Length)
					linesToCheck.Add(diagonalLine);
			}

			var phraseAppearances = 0;

			foreach (var line in linesToCheck)
			{
				var forwardPhrases = line.Split(phraseToFind).Length - 1;
				var backwardPhrases = line.Split((new string(phraseToFind.ToCharArray().Reverse().ToArray()))).Length - 1;
				// Console.WriteLine($"** {line} -> >>{forwardPhrases:N0} - <<{backwardPhrases:N0}");

				phraseAppearances += (forwardPhrases + backwardPhrases);
			}

			Console.WriteLine($"** Searching for [{phraseToFind}] and [{(new string(phraseToFind.ToCharArray().Reverse().ToArray()))}]...");
			Console.WriteLine($"*** Phrase [{phraseToFind}] appears {phraseAppearances:N0} times");
		}

		private static void PartB(List<string> puzzleInputRaw)
		{
			Console.WriteLine("\r\n**********");
			Console.WriteLine("* Part B");

			// only 4 possible patterns:
			// - M S   S M   M M   S S
			// -  A     A     A     A
			// - M S   S M   S S   M M
			//
			// Pulling the 4 corner characters and the center character, you get the following possible matches for each character square:
			// - MSAMS
			// - SMASM
			// - MMASS
			// - SSAMM

			var patterns = new List<string>() { "MSAMS", "SMASM", "MMASS", "SSAMM" };
			var xMasCount = 0;

			for (var r = 0; r < (puzzleInputRaw.Count - 2); r++)
			{
				for (var c = 0; c < (puzzleInputRaw[0].Length - 2); c++)
				{
					var pattern = $"{puzzleInputRaw[r][c]}{puzzleInputRaw[r][c + 2]}{puzzleInputRaw[r + 1][c + 1]}{puzzleInputRaw[r + 2][c]}{puzzleInputRaw[r + 2][c + 2]}";
					if (patterns.Contains(pattern))
					{
						Console.WriteLine($"** Found [{pattern}] at {r}, {c}");
						xMasCount++;
					}
				}
			}

			Console.WriteLine($"*** Found X-MAS {xMasCount:N0} times");
		}
	}
}
