using System;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;

namespace Day06
{
    internal class TurningPoint
    {
        public char IncomingDirection;
        public int Row;
        public int Col;

        public TurningPoint(char incomingDirection, int row, int col)
        {
            IncomingDirection = incomingDirection;
            Row = row;
            Col = col;
        }
    }

    internal class Program
    {
        const char Guard = '^';
        const char Obstacle = '#';

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2024: Day 6");
            var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();

            PartA(puzzleInputRaw);
            PartB(puzzleInputRaw);
        }

        private static void PartA(List<string> puzzleInputRaw)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part A");

            List<string> guardPositions = [];
            int guardX = -1;    // the character in the line the guard is on; 0 is left
            int guardY = -1;    // the line the guard is on; 0 is top, so going up decreases Y
            int limitRight = puzzleInputRaw[0].Length;  // how far right from 0 can the guard go?
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
                    case "right":
                        spotAheadX = guardX + 1;
                        spotAheadY = guardY;
                        break;
                    case "down":
                        spotAheadX = guardX;
                        spotAheadY = guardY + 1;
                        break;
                    case "left":
                        spotAheadX = guardX - 1;
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
                    if (puzzleInputRaw[spotAheadY][spotAheadX] != Obstacle)
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

        private static void PartB(List<string> puzzleInputRaw)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part B");

            List<string> newObstacles = [];
            int limitRight = puzzleInputRaw[0].Length;  // how far right from 0 can the guard go?
            int limitBottom = puzzleInputRaw.Count;     // how far down from 0 can the guard go?
            int guardX = -1;    // the character in the line the guard is on; 0 is left
            int guardY = -1;    // the line the guard is on; 0 is top, so going up decreases Y
            char guardDirection = 'U';       // can be U, D, L, R
            char rightTurnDirection = 'R';
            int spotAheadX;
            int spotAheadY;

            List<TurningPoint> turningPoints = DetermineTurningPoints(puzzleInputRaw);

            // find the guard's starting position
            guardY = puzzleInputRaw.FindIndex(x => x.Contains(Guard));
            guardX = puzzleInputRaw[guardY].IndexOf(Guard);

            // loop until the guard goes out of bounds
            while (guardX > -1 && guardX < limitRight && guardY > -1 && guardY < limitBottom)
            {
                // determine if there is at least one turning point in the direction ahead
                // determine if that turning point puts the guard on an existing path in the same direction

                switch (guardDirection)
                {
                    case 'U':
                        spotAheadX = guardX;
                        spotAheadY = guardY - 1;
                        break;
                    case 'R':
                        spotAheadX = guardX + 1;
                        spotAheadY = guardY;
                        break;
                    case 'D':
                        spotAheadX = guardX;
                        spotAheadY = guardY + 1;
                        break;
                    case 'L':
                        spotAheadX = guardX - 1;
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
                    if (puzzleInputRaw[spotAheadY][spotAheadX] != Obstacle)
                    {
                        guardX = spotAheadX;
                        guardY = spotAheadY;
                    }
                    else
                    {
                        // turn the guard 90 degrees clockwise
                        guardDirection = guardDirection switch
                        {
                            'U' => 'R',
                            'R' => 'D',
                            'D' => 'L',
                            'L' => 'U',
                            _ => '?',
                        };

                        // based on the guard's new direction, what is the direction 90 degrees clockwise from it?
                        rightTurnDirection = guardDirection switch
                        {
                            'U' => 'R',
                            'R' => 'D',
                            'D' => 'L',
                            'L' => 'U',
                            _ => '?',
                        };
                    }
                }
                else
                    break;
            }

            Console.WriteLine($"** Guard exited room going {guardDirection} from {guardX:N0}, {guardY:N0}");
        }

        private static List<TurningPoint> DetermineTurningPoints(List<string> puzzleInputRaw)
        {
            var turningPoints = new List<TurningPoint>();
            int obstacles = 0;
            int limitRight = puzzleInputRaw[0].Length;  // how far right from 0 can the guard go?
            int limitBottom = puzzleInputRaw.Count;     // how far down from 0 can the guard go?

            for (int r = 0; r < limitBottom; r++)
            {
                int c = 0;
                while (c < limitRight)
                {
                    c = puzzleInputRaw[r].IndexOf(Obstacle, c);
                    if (c < 0)
                        break;

                    obstacles++;

                    // check to see if the spaces around the current obstacle are also obstacles,
                    // and if they are not, add them to the list of turning points

                    // look left
                    if ((c > 0) && (puzzleInputRaw[r][c - 1] != Obstacle))
                    {
                        turningPoints.Add(new TurningPoint('L', r, c));
                    }

                    // look right
                    if ((c < (limitRight - 1)) && (puzzleInputRaw[r][c + 1] != Obstacle))
                    {
                        turningPoints.Add(new TurningPoint('R', r, c));
                    }

                    // look up
                    if ((r > 0) && (puzzleInputRaw[r - 1][c] != Obstacle))
                    {
                        turningPoints.Add(new TurningPoint('U', r, c));
                    }

                    // look down
                    if ((r < (limitBottom - 1)) && (puzzleInputRaw[r + 1][c] != Obstacle))
                    {
                        turningPoints.Add(new TurningPoint('D', r, c));
                    }

                    c++;
                }
            }

            Console.WriteLine($"*** Found {obstacles:N0} obstacles, providing {turningPoints.Count:N0} turning points.");

            return turningPoints;
        }
    }
}
