using AoCCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2021.Day05
{
    // https://adventofcode.com/2021/day/5
    class Day05 : IDay
    {
        public void GetResults()
        {
            string[] input = File.ReadAllLines(@"Day05\input.txt");

            int task1 = SolveTask1(input);
            Console.WriteLine($"Task 1 - Overlaps: {task1}");

            int task2 = SolveTask2(input);
            Console.WriteLine($"Task 2 - Overlaps: {task2}");
        }

        private int SolveTask1(string[] input)
        {
            return SolveTask1(input, out var _);
        }

        private int SolveTask1(string[] input, out int[,] diagram)
        {
            var verticalLines = GetVerticalLines(input);
            var horizontalLines = GetHorizontalLines(input);

            int maxX = input.Select(s => Regex.Matches(s, @"(\d+)").Select(m => int.Parse(m.Value)).ToArray()).Max(a => Math.Max(a[0], a[2])) + 1;
            int maxY = input.Select(s => Regex.Matches(s, @"(\d+)").Select(m => int.Parse(m.Value)).ToArray()).Max(a => Math.Max(a[1], a[3])) + 1;
            diagram = new int[maxX, maxY];

            int overlaps = 0;
            foreach (var line in verticalLines)
            {
                foreach (var point in line)
                {
                    diagram[point.X, point.Y] += 1;

                    if (diagram[point.X, point.Y] == 2) overlaps += 1;
                }
            }
            foreach (var line in horizontalLines)
            {
                foreach (var point in line)
                {
                    diagram[point.X, point.Y] += 1;

                    if (diagram[point.X, point.Y] == 2) overlaps += 1;
                }
            }

            return overlaps;
        }

        private int SolveTask2(string[] input)
        {
            int overlaps = SolveTask1(input, out int[,] diagram);
            var diagonalLines = GetDiagonalLines(input);

            foreach (var line in diagonalLines)
            {
                foreach (var point in line)
                {
                    diagram[point.X, point.Y] += 1;

                    if (diagram[point.X, point.Y] == 2) overlaps += 1;
                }
            }

            #region Visualize diagram in text file
            List<string> text = new();
            for (int i = 0; i < diagram.GetLength(1); i++)
            {
                string line = string.Empty;
                for (int j = 0; j < diagram.GetLength(0); j++)
                {
                    line += diagram[j, i];
                }
                text.Add(line);
            }
            File.WriteAllLines(@"Day5\output.txt", text);
            #endregion

            return overlaps;
        }

        private IEnumerable<IEnumerable<Point>> GetVerticalLines(string[] input)
        {
            return input.Select(s => Regex.Matches(s, @"(\d+)").Select(m => int.Parse(m.Value)).ToArray())
                        .Where(a => a[0] == a[2])
                        .Select(a => Enumerable.Range(Math.Min(a[1], a[3]), Math.Max(a[1], a[3]) - Math.Min(a[1], a[3]) + 1).Select(y => new Point(a[0], y)));
        }

        private IEnumerable<IEnumerable<Point>> GetHorizontalLines(string[] input)
        {
            return input.Select(s => Regex.Matches(s, @"(\d+)").Select(m => int.Parse(m.Value)).ToArray())
                        .Where(a => a[1] == a[3])
                        .Select(a => Enumerable.Range(Math.Min(a[0], a[2]), Math.Max(a[0], a[2]) - Math.Min(a[0], a[2]) + 1).Select(x => new Point(x, a[1])));
        }

        private IEnumerable<IEnumerable<Point>> GetDiagonalLines(string[] input)
        {
            var diagonals = input.Select(s => Regex.Matches(s, @"(\d+)").Select(m => int.Parse(m.Value)).ToArray())
                                 .Where(a => a[0] != a[2] && a[1] != a[3]);

            List<List<Point>> lines = new();
            
            foreach (var diagonal in diagonals)
            {
                List<int> xVals = new();
                if (diagonal[0] < diagonal[2])
                {
                    xVals = Enumerable.Range(diagonal[0], diagonal[2] - diagonal[0] + 1).ToList();
                }
                else
                {
                    for (int i = diagonal[0]; i >= diagonal[2]; i--)
                    {
                        xVals.Add(i);
                    }
                }

                List<int> yVals = new();
                if (diagonal[1] < diagonal[3])
                {
                    yVals = Enumerable.Range(diagonal[1], diagonal[3] - diagonal[1] + 1).ToList();
                }
                else
                {
                    for (int i = diagonal[1]; i >= diagonal[3]; i--)
                    {
                        yVals.Add(i);
                    }
                }

                List<Point> points = new();
                for (int i = 0; i < xVals.Count; i++)
                {
                    points.Add(new Point(xVals[i], yVals[i]));
                }
                lines.Add(points);
            }

            return lines;
        }
    }

    class Point
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            return obj is Point point
                && point.X == X
                && point.Y == Y;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
