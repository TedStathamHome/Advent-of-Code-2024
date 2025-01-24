using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day05
{
	internal class PageDetail
	{
		public string PageNo;
		public List<string> EarlierPages;
	}
	
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Advent of Code 2024: Day 5");
			var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();

			var pageOrderingRules = puzzleInputRaw.Where(x => x.Contains('|')).ToList();
			var safetyManualUpdates = puzzleInputRaw.Where(x => !string.IsNullOrWhiteSpace(x) && !x.Contains('|')).ToList();

			//List<PageDetail> pageDetails = new List<PageDetail>();
			
			//foreach (var rule in pageOrderingRules)
			//{
			//	var ruleDetails = rule.Split('|');
			//	var detailIndex = pageDetails.FindIndex(x => x.PageNo == ruleDetails[1]);
				
			//	if (detailIndex < 0)    // we couldn't find it
			//	{
			//		pageDetails.Add(new PageDetail
			//		{
			//			PageNo = ruleDetails[1],
			//			EarlierPages = { ruleDetails[0] }
			//		});
			//	}
			//	else 
			//	{
			//		pageDetails[detailIndex].EarlierPages.Add(ruleDetails[0]);
			//	}
			//}

			PartA(pageOrderingRules, safetyManualUpdates);
			PartB();
		}

		private static void PartA(List<string> pageOrderingRules, List<string> safetyManualUpdates)
		{
			Console.WriteLine("\r\n**********");
			Console.WriteLine("* Part A");

			var middlePageSum = 0;

			foreach (var update in safetyManualUpdates)
			{
				var pageList = update.Split(',');
				var updateIsValid = true;
				
				for (int i = 0; i < pageList.Length; i++)
				{
					// check what falls after the page for matching rules
					if (i < pageList.Length - 1)
					{
						for (int j = i + 1; j < pageList.Length; j++)
						{
							var rule = $"{pageList[i]}|{pageList[j]}";
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
						for(int j = 0; j < i; j++)
						{
							var rule = $"{pageList[j]}|{pageList[i]}";
							if (!pageOrderingRules.Contains(rule))
							{
								updateIsValid = false;
								break;
							}
						}
					}
				}

				Console.WriteLine($"** Update [{update}] is valid? {updateIsValid}");

				if (updateIsValid)
				{
					var middlePageIndex = (pageList.Length / 2);
					middlePageSum += int.Parse(pageList[middlePageIndex]);
				}
			}

			Console.WriteLine($"* Middle page number sum: {middlePageSum:N0}");
		}

		private static void PartB()
		{
			Console.WriteLine("\r\n**********");
			Console.WriteLine("* Part B");
		}
	}
}
