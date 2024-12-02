using System;
using System.IO;
using System.Linq;

namespace Day02
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Advent of Code 2024: Day 2");
			var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();

			PartA(puzzleInputRaw);
			PartB(puzzleInputRaw);
		}

		private static void PartA(List<string> reports)
		{
			Console.WriteLine("\r\n**********");
			Console.WriteLine("* Part A");

			var safeReports = 0;

			foreach (var report in reports)
			{
                var isSafe = true;
                var reportValues = report.Split(' ').Select(x => int.Parse(x)).ToList();
                var increasingValues = string.Join(' ', reportValues.Order().Select(x => $"{x:D}"));
				var decreasingValues = string.Join(' ', reportValues.OrderDescending().Select(x => $"{x:D}"));

				if (report == increasingValues)
				{
                    for (var i = 0; i < (reportValues.Count - 1); i++)
                    {
                        var levelIncrease = reportValues[i + 1] - reportValues[i];
                        isSafe = (levelIncrease >= 1 && levelIncrease <= 3);
                        if (!isSafe)
                            break;
                    }
                }
                else if (report == decreasingValues)
				{
                    for (var i = 0; i < (reportValues.Count - 1); i++)
                    {
                        var levelDecrease = reportValues[i] - reportValues[i + 1];
                        isSafe = (levelDecrease >= 1 && levelDecrease <= 3);
                        if (!isSafe)
                            break;
                    }
                }
				else
					isSafe = false;

                if (isSafe)
                    safeReports++;
                
                Console.WriteLine($"** Report: {report} - {(isSafe ? "Safe" : "Unsafe")}");
            }

			Console.WriteLine($"*** Safe reports: {safeReports:N0}");
		}

		private static void PartB(List<string> reports)
		{
			Console.WriteLine("\r\n**********");
			Console.WriteLine("* Part B");

            var safeReports = 0;

            foreach (var report in reports)
            {
                var atLeastOneVersionIsSafe = false;
                var reportValuesBase = report.Split(' ').Select(x => int.Parse(x)).ToList();

                Console.WriteLine();

                for (var b = -1; b < reportValuesBase.Count; b++)
                {
                    var reportValues = reportValuesBase.Select(x => x).ToList();
                    if (b >= 0)
                        reportValues.RemoveAt(b);

                    var isSafe = true;
                    var modifiedReport = string.Join(' ', reportValues.Select(x => $"{x:D}"));
                    var increasingValues = string.Join(' ', reportValues.Order().Select(x => $"{x:D}"));
                    var decreasingValues = string.Join(' ', reportValues.OrderDescending().Select(x => $"{x:D}"));

                    if (modifiedReport == increasingValues)
                    {
                        for (var i = 0; i < (reportValues.Count - 1); i++)
                        {
                            var levelIncrease = reportValues[i + 1] - reportValues[i];
                            isSafe = (levelIncrease >= 1 && levelIncrease <= 3);
                            if (!isSafe)
                                break;
                        }
                    }
                    else if (modifiedReport == decreasingValues)
                    {
                        for (var i = 0; i < (reportValues.Count - 1); i++)
                        {
                            var levelDecrease = reportValues[i] - reportValues[i + 1];
                            isSafe = (levelDecrease >= 1 && levelDecrease <= 3);
                            if (!isSafe)
                                break;
                        }
                    }
                    else
                        isSafe = false;

                    Console.WriteLine($"**{(modifiedReport == report ? "*" : "")} Report: {modifiedReport} - {(isSafe ? "Safe" : "Unsafe")}");

                    atLeastOneVersionIsSafe = atLeastOneVersionIsSafe || isSafe;
                    if (atLeastOneVersionIsSafe)
                    {
                        Console.WriteLine("*** Exit early from finding a safe version");
                        break;      // exit early
                    }
                }

                if (atLeastOneVersionIsSafe)
                    safeReports++;
            }

            Console.WriteLine($"*** Safe reports: {safeReports:N0}");
        }
    }
}
