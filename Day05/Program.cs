using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day05
{
	internal class Program
	{
		private static List<string> badSafetyManualUpdates = [];

		static void Main(string[] args)
		{
			Console.WriteLine("Advent of Code 2024: Day 5");
			var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();

			var pageOrderingRules = puzzleInputRaw.Where(x => x.Contains('|')).ToList();
			var safetyManualUpdates = puzzleInputRaw.Where(x => !string.IsNullOrWhiteSpace(x) && !x.Contains('|')).ToList();

			PartA(pageOrderingRules, safetyManualUpdates);
			PartB(pageOrderingRules, badSafetyManualUpdates);
		}

		private static void PartA(List<string> pageOrderingRules, List<string> safetyManualUpdates)
		{
			Console.WriteLine("\r\n**********");
			Console.WriteLine("* Part A");

			var middlePageSum = 0;

			foreach (var update in safetyManualUpdates)
			{
				var pageList = update.Split(',').ToList();
				var updateIsValid = IsUpdateValid(pageList, pageOrderingRules);

				Console.WriteLine($"** Update [{update}] is valid? {updateIsValid}");

				if (updateIsValid)
				{
					var middlePageIndex = (pageList.Count / 2);
					middlePageSum += int.Parse(pageList[middlePageIndex]);
				}
				else
					badSafetyManualUpdates.Add(update);
			}

			Console.WriteLine($"* Middle page number sum: {middlePageSum:N0}");
		}

		private static bool IsUpdateValid(List<string> updatePageList, List<string> pageOrderingRules)
		{
			var updateIsValid = true;

			for (int i = 0; i < updatePageList.Count; i++)
			{
				// check what falls after the page for matching rules
				if (i < updatePageList.Count - 1)
				{
					for (int j = i + 1; j < updatePageList.Count; j++)
					{
						var rule = $"{updatePageList[i]}|{updatePageList[j]}";
						if (!pageOrderingRules.Contains(rule))
						{
							updateIsValid = false;
							break;
						}
					}
				}

				if (!updateIsValid)
					break;

				// check what falls before the page for matching rules
				if (i > 0)
				{
					for (int j = 0; j < i; j++)
					{
						var rule = $"{updatePageList[j]}|{updatePageList[i]}";
						if (!pageOrderingRules.Contains(rule))
						{
							updateIsValid = false;
							break;
						}
					}
				}
			}

			return updateIsValid;
		}

		private static void PartB(List<string> pageOrderingRules, List<string> badSafetyManualUpdates)
		{
			Console.WriteLine("\r\n**********");
			Console.WriteLine("* Part B");

			var middlePageSum = 0;

			foreach (var update in badSafetyManualUpdates)
			{
				Console.WriteLine($"** Update: {update}");

				var pageList = update.Split(',').ToList();
				var pass = 0;

				while (!IsUpdateValid(pageList, pageOrderingRules))
				{
					pass++;

					for (int i = 0; i < (pageList.Count - 1); i++)
					{
						var rule = $"{pageList[i]}|{pageList[i + 1]}";

						// if the first page isn't supposed to be before the second page, swap them
						if (!pageOrderingRules.Contains(rule))
						{
							(pageList[i], pageList[i + 1]) = (pageList[i + 1], pageList[i]);
						}
					}

					Console.WriteLine($"*** Pass {pass:N0}: {string.Join(',', pageList)}");
				}

				var middlePageIndex = (pageList.Count / 2);
				middlePageSum += int.Parse(pageList[middlePageIndex]);
			}

			Console.WriteLine($"* Middle page number sum: {middlePageSum:N0}");
		}
	}
}
