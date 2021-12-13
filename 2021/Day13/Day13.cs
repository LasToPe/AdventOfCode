using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2021.Day13
{
    // https://adventofcode.com/2021/day/13
    class Day13 : IDay
    {
        private readonly string outputFilePath = @"Day13\output.txt";
        public void GetResults()
        {
            string[] input = File.ReadAllLines(@"Day13\input.txt");
            var pointValues = input.Take(input.ToList().IndexOf(input.First(x => x.Length == 0)));
            var instructions = input.Where(x => !pointValues.Contains(x) && x.Length != 0);

            int task1 = SolveTask1(pointValues, instructions.First());
            Console.WriteLine($"Task 1 - Number of points visible after first instruction: {task1}");

            SolveTask2(pointValues, instructions);
            Console.WriteLine("Task 2 - Code for activating system:");
            foreach(string line in File.ReadAllLines(outputFilePath))
            {
                Console.WriteLine(line);
            }
        }

        private int SolveTask1(IEnumerable<string> points, string instruction)
        {
            bool[,] dotArray = GetDotArray(points);

            Match m = Regex.Match(instruction, @"(x|y)=(\d+)");
            bool[,] newArray = m.Groups[1].Value == "x"
                ? new bool[dotArray.GetLength(0), int.Parse(m.Groups[2].Value)]
                : new bool[int.Parse(m.Groups[2].Value), dotArray.GetLength(1)];

            for (int row = 0; row < dotArray.GetLength(0); row++)
            {
                for (int col = 0; col < dotArray.GetLength(1); col++)
                {
                    if (row == newArray.GetLength(0) || col == newArray.GetLength(1))
                    {
                        continue;
                    }

                    if (row > newArray.GetLength(0))
                    {
                        int newRow = newArray.GetLength(0) - Math.Abs(newArray.GetLength(0) - row);
                        newArray[newRow, col] = newArray[newRow, col] || dotArray[row, col];
                    }
                    else if (col > newArray.GetLength(1))
                    {
                        int newCol = newArray.GetLength(1) - Math.Abs(newArray.GetLength(1) - col);
                        newArray[row, newCol] = newArray[row, newCol] || dotArray[row, col];
                    }
                    else
                    {
                        newArray[row, col] = dotArray[row, col];
                    }
                }
            }

            int count = 0;
            foreach (bool point in newArray)
            {
                if (point) count += 1;
            }

            return count;
        }

        private void SolveTask2(IEnumerable<string> points, IEnumerable<string> instructions)
        {
            bool[,] dotArray = GetDotArray(points);

            foreach (string instruction in instructions)
            {
                Match m = Regex.Match(instruction, @"(x|y)=(\d+)");
                bool[,] newArray = m.Groups[1].Value == "x"
                    ? new bool[dotArray.GetLength(0), int.Parse(m.Groups[2].Value)]
                    : new bool[int.Parse(m.Groups[2].Value), dotArray.GetLength(1)];

                for (int row = 0; row < dotArray.GetLength(0); row++)
                {
                    for (int col = 0; col < dotArray.GetLength(1); col++)
                    {
                        if (row == newArray.GetLength(0) || col == newArray.GetLength(1))
                        {
                            continue;
                        }

                        if (row > newArray.GetLength(0))
                        {
                            int newRow = newArray.GetLength(0) - Math.Abs(newArray.GetLength(0) - row);
                            newArray[newRow, col] = newArray[newRow, col] || dotArray[row, col];
                        }
                        else if (col > newArray.GetLength(1))
                        {
                            int newCol = newArray.GetLength(1) - Math.Abs(newArray.GetLength(1) - col);
                            newArray[row, newCol] = newArray[row, newCol] || dotArray[row, col];
                        }
                        else
                        {
                            newArray[row, col] = dotArray[row, col];
                        }
                    }
                }

                dotArray = new bool[newArray.GetLength(0), newArray.GetLength(1)];
                dotArray = newArray;
            }

            List<string> output = new();
            for (int row = 0; row < dotArray.GetLength(0); row++)
            {
                string line = string.Empty;
                for (int col = 0; col < dotArray.GetLength(1); col++)
                {
                    line += dotArray[row, col] ? "█" : " ";
                }
                output.Add(line);
            }

            File.WriteAllLines(outputFilePath, output);
        }

        private bool[,] GetDotArray(IEnumerable<string> points)
        {
            bool[,] arr = new bool[points.Max(x => int.Parse(x.Split(',')[1])) + 1, points.Max(x => int.Parse(x.Split(',')[0])) + 1];
            foreach (string point in points)
            {
                arr[int.Parse(point.Split(',')[1]), int.Parse(point.Split(',')[0])] = true;
            }
            return arr;
        }
    }
}
