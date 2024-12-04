using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day03
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Advent of Code 2024: Day 3");
			var puzzleInputRaw = File.ReadAllText($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt");

			PartA(puzzleInputRaw);
			PartB(puzzleInputRaw);
		}

		private static void PartA(string puzzleInputRaw)
		{
			Console.WriteLine("\r\n**********");
			Console.WriteLine("* Part A");

			var regexPattern = @"mul\(\d{1,3},\d{1,3}\)";
			var regexOptions = RegexOptions.Multiline;

			var sumOfMuls = 0;
			foreach (Match match in Regex.Matches(puzzleInputRaw, regexPattern, regexOptions))
			{
				Console.WriteLine($"** '{match.Value}' found at index {match.Index:N0}");
				string[] values = match.Value.Replace("mul(", "").Replace(")", "").Split(',').ToArray();

				sumOfMuls += (int.Parse(values[0]) * int.Parse(values[1]));
			}

			Console.WriteLine($"*** Sum of mul() values multiplied is {sumOfMuls:N0}");
		}

		private static void PartB(string puzzleInputRaw)
		{
			Console.WriteLine("\r\n**********");
			Console.WriteLine("* Part B");

			var regexPatternMul = @"mul\(\d{1,3},\d{1,3}\)";
			var regexPatternDoDont = @"do\(\)|don't\(\)";
			var regexOptions = RegexOptions.Multiline;
			var doDontMatches = Regex.Matches(puzzleInputRaw, regexPatternDoDont, regexOptions);
			var isMulActive = true;
            var sumOfMuls = 0;
            var currentDoDont = 0;
            var doDontType = doDontMatches[currentDoDont].Value;
            var doDontIndex = doDontMatches[currentDoDont].Index;

			foreach (Match match in Regex.Matches(puzzleInputRaw, regexPatternMul, regexOptions))
			{
				if (match.Index > doDontIndex)
				{
					isMulActive = (doDontType == "do()");
					currentDoDont++;
					
					if (currentDoDont > (doDontMatches.Count - 1))
					{
						doDontIndex = puzzleInputRaw.Length;
					}
					else
					{
						doDontType = doDontMatches[currentDoDont].Value;
                        doDontIndex = doDontMatches[currentDoDont].Index;
                    }
				}

				Console.WriteLine($"** '{match.Value}' found at index {match.Index:N0} - {(isMulActive ? "active" : "inactive")}");

				if (isMulActive)
				{
                    string[] values = match.Value.Replace("mul(", "").Replace(")", "").Split(',').ToArray();
                    sumOfMuls += (int.Parse(values[0]) * int.Parse(values[1]));
                }
            }

			Console.WriteLine($"*** Sum of active mul() values multiplied is {sumOfMuls:N0}");
		}
	}
}
