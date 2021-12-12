using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day12
{
    // https://adventofcode.com/2021/day/12
    class Day12 : IDay
    {
        public void GetResults()
        {
            string[] input = File.ReadAllLines(@"Day12\input.txt");

            int task1 = SolveTask1(input);
            Console.WriteLine($"Task 1 - Number of paths visiting small caves once at most: {task1}");

            int task2 = SolveTask2(input);
            Console.WriteLine($"Task 2 - Number of paths visiting a single small cave twice at most: {task2}");
        }

        private int SolveTask1(string[] input)
        {
            List<Cave> caves = BuildCaveSystem(input);
            List<List<Cave>> paths = GetPathsSmallAvailbleOnceRecursive("start", "end", caves);
            return paths.Count;
        }

        private int SolveTask2(string[] input)
        {
            var caves = BuildCaveSystem(input);
            List<List<Cave>> paths = GetPathsSmallAvailableTwiceRecursive("start", "end", caves);
            return paths.Count;
        }

        private List<Cave> BuildCaveSystem(string[] input)
        {
            List<Cave> caves = new();
            foreach (string[] lineParts in input.Select(x => x.Split('-')))
            {
                Cave start;
                if (caves.Any(c => c.Identifier == lineParts[0]))
                {
                    start = caves.Single(c => c.Identifier == lineParts[0]);
                }
                else
                {
                    start = new Cave(lineParts[0], lineParts[0].All(c => char.IsLower(c)));
                    caves.Add(start);
                }

                Cave end;
                if (caves.Any(c => c.Identifier == lineParts[1]))
                {
                    end = caves.Single(c => c.Identifier == lineParts[1]);
                }
                else
                {
                    end = new Cave(lineParts[1], lineParts[1].All(c => char.IsLower(c)));
                    caves.Add(end);
                }

                start.AddConnection(end);
            }
            return caves;
        }

        private List<List<Cave>> GetPathsSmallAvailbleOnceRecursive(string start, string end, List<Cave> caves, List<Cave> existingPath = null)
        {
            Cave startCave = caves.First(x => x.Identifier == start);
            List<List<Cave>> paths = new();

            if (existingPath == null)
            {
                existingPath = new();
            }

            IEnumerable<Cave> excluded = existingPath.Where(x => x.IsSmall);

            foreach (Cave cave in startCave.ConnectedCaves.Where(c => !excluded.Contains(c)))
            {
                var currentPath = new List<Cave>(existingPath);

                if (cave.Identifier == end)
                {
                    currentPath.Add(startCave);
                    currentPath.Add(cave);

                    paths.Add(currentPath);
                    continue;
                }

                currentPath.Add(startCave);
                paths.AddRange(GetPathsSmallAvailbleOnceRecursive(cave.Identifier, end, caves, currentPath));
            }

            return paths;
        }

        private List<List<Cave>> GetPathsSmallAvailableTwiceRecursive(string start, string end, List<Cave> caves, List<Cave> existingPath = null, Cave smallCaveAvailableTwice = null)
        {
            Cave startCave = caves.First(x => x.Identifier == start);
            List<List<Cave>> paths = new();

            if (existingPath == null)
            {
                existingPath = new();
            }

            List<Cave> excluded = existingPath.Where(x => x.IsSmall && x != smallCaveAvailableTwice).ToList();
            if (existingPath.Count(x => x == smallCaveAvailableTwice) == 2)
            {
                excluded.Add(smallCaveAvailableTwice);
            }

            List<Task> tasks = new();

            foreach (Cave cave in startCave.ConnectedCaves.Where(c => !excluded.Contains(c)))
            {
                var currentPath = new List<Cave>(existingPath);

                if (cave.Identifier == end)
                {
                    currentPath.Add(startCave);
                    currentPath.Add(cave);

                    paths.Add(currentPath);
                    continue;
                }

                currentPath.Add(startCave);

                if (smallCaveAvailableTwice == null && startCave.IsSmall && startCave.Identifier != "start")
                {
                    tasks.Add(Task.Run(() =>
                    {
                        List<List<Cave>> newPaths = GetPathsSmallAvailableTwiceRecursive(cave.Identifier, end, caves, currentPath, startCave);
                        foreach (List<Cave> path in GetPathsSmallAvailableTwiceRecursive(cave.Identifier, end, caves, currentPath, null))
                        {
                            if (newPaths.Any(p => p.SequenceEqual(path)))
                            {
                                continue;
                            }
                            newPaths.Add(path);
                        }

                        paths.AddRange(newPaths);
                    }));
                }
                else
                {
                    tasks.Add(Task.Run(() =>
                    {
                        paths.AddRange(GetPathsSmallAvailableTwiceRecursive(cave.Identifier, end, caves, currentPath, smallCaveAvailableTwice));
                    }));
                }
            }

            Task.WaitAll(tasks.ToArray());
            return paths;
        }
    }

    class Cave
    {
        public string Identifier { get; private set; }
        public bool IsSmall { get; private set; }
        public List<Cave> ConnectedCaves { get; }

        public Cave(string identifier, bool isSmall)
        {
            Identifier = identifier;
            IsSmall = isSmall;
            ConnectedCaves = new();
        }

        public void AddConnection(Cave cave)
        {
            if (!ConnectedCaves.Contains(cave))
            {
                ConnectedCaves.Add(cave);
                cave.AddConnection(this);
            }
        }
    }
}
