using AoCCommon;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2021.Day17
{
    // https://adventofcode.com/2021/day/17
    class Day17 : IDay
    {
        public void GetResults()
        {
            string input = File.ReadAllLines(@"Day17\input.txt")[0];
            Match xMatch = Regex.Match(input, @"x=(\d+)\.\.(\d+)");
            Match yMatch = Regex.Match(input, @"y=(-\d+)\.\.(-\d+)");
            
            List<(int x, int y)> targetArea = new();
            foreach (int x in Enumerable.Range(int.Parse(xMatch.Groups[1].Value), int.Parse(xMatch.Groups[2].Value) - int.Parse(xMatch.Groups[1].Value) + 1))
            {
                foreach (int y in Enumerable.Range(int.Parse(yMatch.Groups[1].Value), int.Parse(yMatch.Groups[2].Value) - int.Parse(yMatch.Groups[1].Value) + 1))
                {
                    targetArea.Add((x, y));
                }
            }

            var (task1, task2) = SolveTasks(targetArea);
            Console.WriteLine($"Task 1 - Highest possible y position: {task1}");
            Console.WriteLine($"Task 2 - Distinct initial velocities: {task2}");
        }

        private (int maxY, int count) SolveTasks(List<(int x, int y)> targetArea)
        {
            ConcurrentBag<(int x, int y, int maxY)> velocities = new();

            Parallel.For(1, targetArea.Max(t => t.x) + 1, xInititalVelocity =>
            {
                for (int yInitialVelocity = targetArea.Max(t => t.x); yInitialVelocity >= targetArea.Min(t => t.y); yInitialVelocity--)
                {
                    (int x, int y) velocity = (xInititalVelocity, yInitialVelocity);
                    (int x, int y) startingPoint = (0, 0);
                    List<(int x, int y)> points = new() { startingPoint };

                    while ((velocity.x > 0 || (points.Last().x >= targetArea.Min(t => t.x) && points.Last().x <= targetArea.Max(t => t.x)))
                        && points.Last().x + velocity.x <= targetArea.Max(t => t.x)
                        && points.Last().y + velocity.y >= targetArea.Min(t => t.y))
                    {
                        points.Add((points.Last().x + velocity.x, points.Last().y + velocity.y));
                        if (velocity.x > 0)
                            velocity.x -= 1;
                        velocity.y -= 1;
                    }

                    if (targetArea.Any(t => t.x == points.Last().x && t.y == points.Last().y))
                    {
                        velocities.Add((xInititalVelocity, yInitialVelocity, points.Max(p => p.y)));
                    }
                }
            });

            return (velocities.Max(v => v.maxY), velocities.Count);
        }
    }
}
