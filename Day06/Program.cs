using System;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;

namespace Day06
{
	internal class Program
	{
		const char Guard = '^';

		static void Main(string[] args)
		{
			Console.WriteLine("Advent of Code 2024: Day 6");
			var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();

			PartA(puzzleInputRaw);
			PartB();
		}

		private static void PartA(List<string> puzzleInputRaw)
		{
			Console.WriteLine("\r\n**********");
			Console.WriteLine("* Part A");

			List<string> guardPositions = [];
			int guardX = -1;	// the character in the line the guard is on; 0 is left
			int guardY = -1;    // the line the guard is on; 0 is top, so going up decreases Y
			int limitRight = puzzleInputRaw[0].Length;	// how far right from 0 can the guard go?
			int limitBottom = puzzleInputRaw.Count;     // how far down from 0 can the guard go?
			string guardDirection = "up";       // can be up, down, left, right
			int spotAheadX;
			int spotAheadY;

			// find the guard's starting position
			guardY = puzzleInputRaw.FindIndex(x => x.Contains(Guard));
			guardX = puzzleInputRaw[guardY].IndexOf(Guard);

			guardPositions.Add($"{guardX:N0}|{guardY:N0}");

			// loop until the guard goes out of bounds
			while (guardX > -1 && guardX < limitRight && guardY > -1 && guardY < limitBottom)
			{
				switch (guardDirection)
				{
					case "up":
						spotAheadX = guardX;
						spotAheadY = guardY - 1;
						break;
					case "down":
						spotAheadX = guardX;
						spotAheadY = guardY + 1;
						break;
					case "left":
						spotAheadX = guardX - 1;
						spotAheadY = guardY;
						break;
					case "right":
						spotAheadX = guardX + 1;
						spotAheadY = guardY;
						break;
					default:
						// don't know which direction they were going, so throw them off the map
						spotAheadX = -2;
						spotAheadY = -2;
						break;
				}

				if (spotAheadX > -1 && spotAheadX < limitRight && spotAheadY > -1 && spotAheadY < limitBottom)
				{
					if (puzzleInputRaw[spotAheadY][spotAheadX] != '#')
					{
						guardX = spotAheadX;
						guardY = spotAheadY;
						var positionToCheck = $"{guardX:N0}|{guardY:N0}";

						if (!guardPositions.Contains(positionToCheck))
						{
							guardPositions.Add(positionToCheck);
						}
					}
					else
					{
						// turn the guard 90 degrees clockwise
						guardDirection = guardDirection switch
						{
							"up" => "right",
							"right" => "down",
							"down" => "left",
							"left" => "up",
							_ => "???",
						};
					}
				}
				else
					break;
			}

			Console.WriteLine($"** Guard exited room going {guardDirection} from {guardX:N0}, {guardY:N0}");
			Console.WriteLine($"** Guard stepped on {guardPositions.Count:N0} spaces.");
		}

		private static void PartB()
		{
			Console.WriteLine("\r\n**********");
			Console.WriteLine("* Part B");
		}
	}
}
