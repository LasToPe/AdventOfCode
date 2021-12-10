using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day09
{
    class Day09 : IDay
    {
        public void GetResults()
        {
            string[] input = File.ReadAllLines(@"Day09\input.txt");

            int task1 = SolveTask1(input);
            Console.WriteLine($"Task 1 - Risk level sum: {task1}");

            int task2 = SolveTask2(input);
            Console.WriteLine($"Task 2 - Product of largest 3 basin counts: {task2}");
        }

        private int SolveTask1(string[] input)
        {
            int[,] heightMap = GetHeightMap(input);

            List<int> lowPoints = new();
            for (int row = 0; row < heightMap.GetLength(0); row++)
            {
                for (int col = 0; col < heightMap.GetLength(1); col++)
                {
                    int candidate = heightMap[row, col];

                    try
                    {
                        if (row == 0)
                        {
                            if (col == 0
                                && candidate < heightMap[row, col + 1]      // right
                                && candidate < heightMap[row + 1, col])     // below
                            {
                                lowPoints.Add(candidate);
                                continue;
                            }
                            if (col == heightMap.GetLength(1) - 1
                                && candidate < heightMap[row, col - 1]     // left
                                && candidate < heightMap[row + 1, col])    // below
                            {
                                lowPoints.Add(candidate);
                                continue;
                            }
                            if (candidate < heightMap[row, col - 1]        // left
                                && candidate < heightMap[row, col + 1]     // right
                                && candidate < heightMap[row + 1, col])    // below
                            {
                                lowPoints.Add(candidate);
                                continue;
                            }
                        }
                        else if (row == heightMap.GetLength(0) - 1)
                        {
                            if (col == 0
                                && candidate < heightMap[row, col + 1]     // right
                                && candidate < heightMap[row - 1, col])    // above
                            {
                                lowPoints.Add(candidate);
                                continue;
                            }
                            if (col == heightMap.GetLength(1) - 1
                                && candidate < heightMap[row, col - 1]     // left
                                && candidate < heightMap[row - 1, col])    // above
                            {
                                lowPoints.Add(candidate);
                                continue;
                            }
                            if (candidate < heightMap[row, col - 1]        // left
                                && candidate < heightMap[row, col + 1]     // right
                                && candidate < heightMap[row - 1, col])    // above
                            {
                                lowPoints.Add(candidate);
                                continue;
                            }
                        }
                        else
                        {
                            if (col == 0
                                && candidate < heightMap[row, col + 1]     // right
                                && candidate < heightMap[row - 1, col]     // above
                                && candidate < heightMap[row + 1, col])    // below
                            {
                                lowPoints.Add(candidate);
                                continue;
                            }
                            if (col == heightMap.GetLength(1) - 1
                                && candidate < heightMap[row, col - 1]     // left
                                && candidate < heightMap[row - 1, col]     // above
                                && candidate < heightMap[row + 1, col])    // below
                            {
                                lowPoints.Add(candidate);
                                continue;
                            }
                            if (candidate < heightMap[row, col - 1]        // left
                                && candidate < heightMap[row, col + 1]     // right
                                && candidate < heightMap[row - 1, col]     // above
                                && candidate < heightMap[row + 1, col])    // below
                            {
                                lowPoints.Add(candidate);
                                continue;
                            }
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        continue;
                    }
                }
            }

            return lowPoints.Sum(x => x + 1);
        }

        private int SolveTask2(string[] input)
        {
            int[,] heightMap = GetHeightMap(input);

            List<List<dynamic>> basins = new();
            for (int row = 0; row < heightMap.GetLength(0); row++)
            {
                for (int col = 0; col < heightMap.GetLength(1); col++)
                {
                    if (heightMap[row, col] == 9) continue;

                    try
                    {
                        if (row == 0)
                        {
                            if (col == 0)
                            {
                                basins.Add(new List<dynamic> { new { Row = row, Column = col } });
                                continue;
                            }

                            if (col == heightMap.GetLength(1) - 1)
                            {
                                if (basins.Any(b => b.Contains(new { Row = row, Column = col - 1 })))
                                {
                                    basins.Single(b => b.Contains(new { Row = row, Column = col - 1 })).Add(new { Row = row, Column = col });
                                }
                                else
                                {
                                    basins.Add(new List<dynamic> { new { Row = row, Column = col } });
                                }
                                continue;
                            }

                            if (basins.Any(b => b.Contains(new { Row = row, Column = col - 1 })))
                            {
                                basins.Single(b => b.Contains(new { Row = row, Column = col - 1 })).Add(new { Row = row, Column = col });
                            }
                            else
                            {
                                basins.Add(new List<dynamic> { new { Row = row, Column = col } });
                            }
                            continue;
                        }
                        else
                        {
                            if (col == 0)
                            {
                                if (basins.Any(b => b.Contains(new { Row = row - 1, Column = col })))
                                {
                                    basins.Single(b => b.Contains(new { Row = row - 1, Column = col })).Add(new { Row = row, Column = col });
                                }
                                else
                                {
                                    basins.Add(new List<dynamic> { new { Row = row, Column = col } });
                                }
                                continue;
                            }

                            if (col == heightMap.GetLength(1) - 1)
                            {
                                if (basins.Any(b => b.Contains(new { Row = row, Column = col - 1 })))
                                {
                                    basins.Single(b => b.Contains(new { Row = row, Column = col - 1 })).Add(new { Row = row, Column = col });
                                }
                                else if (basins.Any(b => b.Contains(new { Row = row - 1, Column = col })))
                                {
                                    basins.Single(b => b.Contains(new { Row = row - 1, Column = col })).Add(new { Row = row, Column = col });
                                }
                                else
                                {
                                    basins.Add(new List<dynamic> { new { Row = row, Column = col } });
                                }
                                continue;
                            }

                            if (basins.Any(b => b.Contains(new { Row = row, Column = col - 1 }))
                                || basins.Any(b => b.Contains(new { Row = row - 1, Column = col })))
                            {
                                var existing = basins.Where(b => b.Contains(new { Row = row, Column = col - 1 }) || b.Contains(new { Row = row - 1, Column = col }));
                                if (existing.Count() > 1)
                                {
                                    var basin = basins.SingleOrDefault(b => b.Contains(new { Row = row - 1, Column = col })) ?? basins.Single(b => b.Contains(new { Row = row, Column = col - 1 }));
                                    var basin2 = basins.Single(b => b.Contains(new { Row = row, Column = col - 1 }));

                                    basins.Remove(basin2);

                                    basin.AddRange(basin2);
                                    basin.Add(new { Row = row, Column = col });
                                }
                                else if (basins.Any(b => b.Contains(new { Row = row, Column = col - 1 })))
                                {
                                    basins.Single(b => b.Contains(new { Row = row, Column = col - 1 })).Add(new { Row = row, Column = col });
                                }
                                else
                                {
                                    basins.Single(b => b.Contains(new { Row = row - 1, Column = col })).Add(new { Row = row, Column = col });
                                }
                            }
                            else
                            {
                                basins.Add(new List<dynamic> { new { Row = row, Column = col } });
                            }
                            continue;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        continue;
                    }

                }
            }

            var orderedBasinCounts = basins.OrderByDescending(x => x.Count).Select(x => x.Count).ToArray();

            return orderedBasinCounts[0] * orderedBasinCounts[1] * orderedBasinCounts[2];
        }

        private int[,] GetHeightMap(string[] input)
        {
            int[,] heightMap = new int[input[0].Length, input.Length];
            for (int row = 0; row < input.Length; row++)
            {
                for (int col = 0; col < input[row].Length; col++)
                {
                    heightMap[row, col] = int.Parse(new string(input[row][col], 1));
                }
            }
            return heightMap;
        }
    }
}
