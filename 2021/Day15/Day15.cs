using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

            var task2 = SolveTask2(input);
        }

        private object SolveTask1(string[] input)
        {
            List<Point> pointMap = BuildPointMap(input);

            Point start = pointMap.Last(p => p.Y == 0 && p.X == 0);
            start.MinCostToStart = 0;
            Point end = pointMap.Last(p => p.Y == 99 && p.X == 99);

            var shortestPath = GetShortestPath(start, end);

            return shortestPath.Last().MinCostToStart;
        }

        private object SolveTask2(string[] input)
        {
            //List<Point> pointMap = BuildPointMap(input);
            //GeneratorPoint start = new GeneratorPoint(pointMap.First()) { MinCostToStart = 0 };
            //GeneratorPoint end = new GeneratorPoint(499, 499, pointMap.Last().Risk + 8);

            //var shortestPath = GetShortestPathGenerate(start, end, pointMap);

            throw new NotImplementedException();
        }

        private List<Point> GetShortestPath(Point start, Point end)
        {
            List<Point> prioQueue = new() { start };
            while (prioQueue.Any())
            {
                prioQueue = prioQueue.OrderBy(x => x.MinCostToStart + x.StraightLineDistanceToEnd).ToList();
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

        private List<GeneratorPoint> GetShortestPathGenerate(GeneratorPoint start, GeneratorPoint end, List<Point> smallPointMap)
        {
            List<GeneratorPoint> pointMap = new() { start };
            List<GeneratorPoint> prioQueue = new() { start };
            while (prioQueue.Any())
            {
                prioQueue = prioQueue.OrderBy(x => x.MinCostToStart + x.StraightLineDistanceToEnd).ToList();
                GeneratorPoint node = prioQueue.First();
                prioQueue.Remove(node);

                if (node.Y <= smallPointMap.Max(p => p.Y) && node.X <= smallPointMap.Max(p => p.X))
                {
                    node.Neighbors = smallPointMap.Where(p => (p.Y == node.Y - 1 && p.X == node.X)
                                                              || (p.Y == node.Y && p.X == node.X - 1)
                                                              || (p.Y == node.Y + 1 && p.X == node.X)
                                                              || (p.Y == node.Y && p.X == node.X + 1))
                                                  .Select(p => new GeneratorPoint(p))
                                                  .ToList();
                    foreach (GeneratorPoint neighbor in node.Neighbors)
                    {
                        if (!pointMap.Any(p => p.Y == neighbor.Y && p.X == neighbor.X))
                            pointMap.Add(neighbor);
                    }
                }
                else
                {
                    int yRef = node.Y % 100;
                    int xRef = node.X % 100;
                    int yMod = node.Y / 100;
                    int xMod = node.X / 100;

                    node.Neighbors = pointMap.Where(p => (p.Y == node.Y - 1 && p.X == node.X)
                                                              || (p.Y == node.Y && p.X == node.X - 1)
                                                              || (p.Y == node.Y + 1 && p.X == node.X)
                                                              || (p.Y == node.Y && p.X == node.X + 1)).ToList();

                    node.Neighbors.AddRange(smallPointMap.Where(p => (p.Y == yRef - 1 && p.X == xRef)
                                                                     || (p.Y == yRef && p.X == xRef - 1)
                                                                     || (p.Y == yRef + 1 && p.X == xRef)
                                                                     || (p.Y == yRef && p.X == xRef + 1))
                                                         .Select(p => new GeneratorPoint(yRef + yMod * 100,
                                                                                  xRef + xMod * 100,
                                                                                  p.Risk + yMod + xMod))
                                                         .Where(p => !node.Neighbors.Any(n => n.Y == p.Y && n.X == p.X)));

                    foreach (GeneratorPoint neighbor in node.Neighbors)
                    {
                        if (!pointMap.Any(p => p.Y == neighbor.Y && p.X == neighbor.X))
                            pointMap.Add(neighbor);
                    }
                }

                foreach (GeneratorPoint point in node.Neighbors)
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

            List<GeneratorPoint> shortestPath = new() { end };
            while (!shortestPath.Contains(start))
            {
                GeneratorPoint node = shortestPath.Last();
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
            for (int line = 0; line < yMax; line++)
            {
                for (int x = 0; x < xMax; x++)
                {
                    Point point = new Point(line, x, int.Parse(input[line].Substring(x, 1)));
                    point.AddNeighbors(points.Where(p => (p.Y == line - 1 && p.X == x)          // above
                                                         || (p.Y == line && p.X == x - 1)));    // left
                    points.Add(point);
                }
            }

            if (times == 1) return points;

            for (int i = 0; i < points.Count; i++)
            {
                Point point = points[i];
                for (int y = 0; y < times; y++)
                {
                    int yVal = point.Y + y * yMax;
                    for (int x = 0; x < times; x++)
                    {
                        if (y == 0 && x == 0) break;

                        int xVal = point.X + x * xMax;
                        Point newPoint = new Point(yVal, xVal, point.Risk + y + x);
                        newPoint.AddNeighbors(points.Where(p => (p.Y == yVal - 1 && p.X == xVal)          // above
                                                                || (p.Y == yVal && p.X == xVal - 1)));    // left

                        points.Add(point);
                    }
                }
            }

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
        public double StraightLineDistanceToEnd => Math.Sqrt(Math.Pow(99 - X, 2) + Math.Pow(99 - Y, 2));
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
    }

    class GeneratorPoint
    {
        public int Y { get; private set; }
        public int X { get; private set; }
        public int Risk { get; private set; }

        public bool Visited { get; set; }
        public int? MinCostToStart { get; set; }
        public double StraightLineDistanceToEnd => Math.Sqrt(Math.Pow(499 - X, 2) + Math.Pow(499 - Y, 2));
        public List<GeneratorPoint> Neighbors { get; set; } = new();
        public GeneratorPoint NearestToStart { get; set; }

        public GeneratorPoint(int y, int x, int risk)
        {
            Y = y;
            X = x;
            Risk = risk;
        }
        public GeneratorPoint(Point point)
        {
            Y = point.Y;
            X = point.X;
            Risk = point.Risk;
        }

        //public void AddNeighbors(IEnumerable<Point> points)
        //{
        //    foreach (Point point in points)
        //    {
        //        AddNeighbor(point);
        //    }
        //}
        //public void AddNeighbor(Point point)
        //{
        //    if (!Neighbors.Contains(point))
        //    {
        //        Neighbors.Add(point);
        //        point.AddNeighbor(this);
        //    }
        //}
    }
}
