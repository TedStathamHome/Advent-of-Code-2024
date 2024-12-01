using System;
using System.IO;
using System.Linq;
using System.Xml.Schema;

namespace Day01
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Advent of Code 2024: Day 1");
			var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();

            PartA(puzzleInputRaw);
			PartB(puzzleInputRaw);
		}

		private static void PartA(List<string> puzzleInputRaw)
		{
			Console.WriteLine("\r\n**********");
			Console.WriteLine("* Part A");

			List<string[]> listValues = puzzleInputRaw.Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToList();
			int[] leftList = listValues.Select(x => int.Parse(x[0])).Order().ToArray();
			int[] rightList = listValues.Select(x => int.Parse(x[1])).Order().ToArray();
			int distanceSum = 0;

			for (int i = 0; i < leftList.Length; i++)
			{
				distanceSum += Math.Abs(leftList[i] - rightList[i]);
			}

			Console.WriteLine($"*** The two lists contain {listValues.Count:N0} entries.");
			Console.WriteLine($"*** The sum of distances between corrpesponding entries is {distanceSum:N0}.");
        }

		private static void PartB(List<string> puzzleInputRaw)
		{
			Console.WriteLine("\r\n**********");
			Console.WriteLine("* Part B");

            List<string[]> listValues = puzzleInputRaw.Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToList();
            var leftList = listValues.Select(x => int.Parse(x[0])).ToList();
            var rightList = listValues.Select(x => int.Parse(x[1])).ToList();

			var similarityCounts = leftList.Select(l => new List<int>() { l, rightList.Count(r => r == l)}).ToList();
			var similarityScore = similarityCounts.Sum(c => c[0] * c[1]);
            Console.WriteLine($"*** The two lists contain {listValues.Count:N0} entries.");
            Console.WriteLine($"*** The similiarty score is {similarityScore:N0}.");
        }
    }
}
