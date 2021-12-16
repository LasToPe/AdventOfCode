using AoCCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2020.Day03
{
    // https://adventofcode.com/2020/day/3
    class Day03 : IDay
    {
        public void GetResults()
        {
            string[] input = File.ReadAllLines(@"Day03\input.txt");

            int task1 = SolveTask1(input);
            Console.WriteLine($"Task 1 - Number of trees: {task1}");

            long task2 = SolveTask2(input);
            Console.WriteLine($"Task 2 - Product of number of trees in each slope: {task2}");
        }

        private int SolveTask1(string[] input)
        {
            int treeCount = 0;

            int horizontalPos = 0;
            for (int row = 1; row < input.Length; row++)
            {
                horizontalPos += 3;

                string line = input[row];
                while (line.Length <= horizontalPos)
                {
                    line += line;
                }

                if (line[horizontalPos] == '#') treeCount += 1;
            }

            return treeCount;
        }

        private long SolveTask2(string[] input)
        {
            long treeCountProduct = 0;

            List<int[]> slopes = new List<int[]>
            {
                new int[] { 1, 1 },
                new int[] { 3, 1 },
                new int[] { 5, 1 },
                new int[] { 7, 1 },
                new int[] { 1, 2 },
            };

            foreach (int[] slope in slopes)
            {
                int treeCount = 0;
                int horizontalPos = 0;
                for (int row = slope[1]; row < input.Length; row += slope[1])
                {
                    horizontalPos += slope[0];

                    string line = input[row];
                    while (line.Length <= horizontalPos)
                    {
                        line += line;
                    }

                    if (line[horizontalPos] == '#') treeCount += 1;
                }

                if (treeCountProduct == 0) treeCountProduct += treeCount;
                else treeCountProduct *= treeCount;
            }

            return treeCountProduct;
        }
    }
}
