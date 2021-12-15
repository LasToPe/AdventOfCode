using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AoC2021.Day15
{
    // https://adventofcode.com/2021/day/15
    class Day15 : IDay
    {
        public void GetResults()
        {
            string[] input = File.ReadAllLines(@"Day15\input.txt");

            var task1 = SolveTask1(input);
            Console.WriteLine($"Task 1 - Lowest total risk: {task1}");

            ConsoleSpinner spinner = new();
            spinner.Start("Working on Task 2");
            var task2 = SolveTask2(input);
            spinner.Stop();
            Console.WriteLine($"Task 2 - Lowest total risk full map(!): {task2}");
        }

        private object SolveTask1(string[] input)
        {
            List<Point> pointMap = BuildPointMap(input);

            Point start = pointMap.First(p => p.Y == pointMap.Min(p => p.Y) && p.X == pointMap.Min(p => p.X));
            start.MinCostToStart = 0;
            Point end = pointMap.Last(p => p.Y == pointMap.Max(p => p.Y) && p.X == pointMap.Max(p => p.X));

            var shortestPath = GetShortestPath(start, end);

            return shortestPath.Last().MinCostToStart;
        }

        private object SolveTask2(string[] input)
        {
            List<Point> pointMap = BuildPointMap(input, 5);

            Point start = pointMap.First(p => p.Y == pointMap.Min(p => p.Y) && p.X == pointMap.Min(p => p.X));
            start.MinCostToStart = 0;
            Point end = pointMap.Last(p => p.Y == pointMap.Max(p => p.Y) && p.X == pointMap.Max(p => p.X));

            var shortestPath = GetShortestPath(start, end);

            return shortestPath.Last().MinCostToStart;
        }

        private List<Point> GetShortestPath(Point start, Point end)
        {
            List<Point> prioQueue = new() { start };
            while (prioQueue.Any())
            {
                prioQueue = prioQueue.OrderBy(x => x.MinCostToStart + x.GetStraightLineDistanceTo(end)).ToList();
                Point node = prioQueue.First();
                prioQueue.Remove(node);

                foreach (Point point in node.Neighbors)
                {
                    if (point.Visited) continue;

                    if (point.MinCostToStart == null
                        || node.MinCostToStart + point.Risk < point.MinCostToStart)
                    {
                        point.MinCostToStart = node.MinCostToStart + point.Risk;
                        point.NearestToStart = node;

                        if (!prioQueue.Contains(point)) prioQueue.Add(point);
                    }
                }
                node.Visited = true;
                if (node == end) break;
            }

            List<Point> shortestPath = new() { end };
            while (!shortestPath.Contains(start))
            {
                Point node = shortestPath.Last();
                shortestPath.Add(node.NearestToStart);
            }
            shortestPath.Reverse();

            return shortestPath;
        }

        private List<Point> BuildPointMap(string[] input, int times = 1)
        {
            int yMax = input.Length;
            int xMax = input[0].Length;

            List<Point> points = new();
            for (int yTime = 0; yTime < times; yTime++)
            {
                for (int line = 0; line < yMax; line++)
                {
                    int yVal = line + yTime * yMax;
                    for (int xTime = 0; xTime < times; xTime++)
                    {
                        for (int x = 0; x < xMax; x++)
                        {
                            int xVal = x + xTime * xMax;

                            int risk = int.Parse(input[line].Substring(x, 1)) + yTime + xTime;
                            if (risk > 9)
                                risk -= 9;

                            Point point = new Point(yVal, xVal, risk);
                            points.Add(point);
                        } 
                    }
                } 
            }

            Parallel.ForEach(points, point =>
            {
                var neighbors = points.Where(p => (p.Y == point.Y - 1 && p.X == point.X)
                                                   || (p.Y == point.Y && p.X == point.X + 1)
                                                   || (p.Y == point.Y + 1 && p.X == point.X)
                                                   || (p.Y == point.Y && p.X == point.X - 1));
                point.AddNeighbors(neighbors);
            });

            return points;
        }
    }

    class Point
    {
        public int Y { get; private set; }
        public int X { get; private set; }
        public int Risk { get; private set; }

        public bool Visited { get; set; }
        public int? MinCostToStart { get; set; }
        public List<Point> Neighbors { get; } = new();
        public Point NearestToStart { get; set; }

        public Point(int y, int x, int risk)
        {
            Y = y;
            X = x;
            Risk = risk;
        }

        public void AddNeighbors(IEnumerable<Point> points)
        {
            foreach (Point point in points)
            {
                AddNeighbor(point);
            }
        }
        public void AddNeighbor(Point point)
        {
            if (!Neighbors.Contains(point))
            {
                Neighbors.Add(point);
                point.AddNeighbor(this);
            }
        }

        public double GetStraightLineDistanceTo(Point end)
        {
            return Math.Sqrt(Math.Pow(end.X - X, 2) + Math.Pow(end.Y - Y, 2));
        }
    }
}
