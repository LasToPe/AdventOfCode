using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day7
{
    class Day7 : IDay
    {
        public void GetResults()
        {
            string[] input = File.ReadAllLines(@"Day7\input.txt");

            int task1 = SolveTask1(input);
            Console.WriteLine($"Task 1 - Minimum fuel use: {task1}");

            int task2 = SolveTask2(input);
            Console.WriteLine($"Task 2 - Minimum fuel use: {task2}");
        }

        private int SolveTask1(string[] input)
        {
            int[] positions = input[0].Split(',').Select(i => int.Parse(i)).ToArray();
            int maxPos = positions.Max();
            int minPos = positions.Min();

            Dictionary<int, int> fuelUseDict = new();
            for (int i = minPos; i < maxPos + 1; i++)
            {
                fuelUseDict.Add(i, positions.Sum(x => Math.Abs(x - i)));
            }

            return fuelUseDict.Values.Min();
        }

        private int SolveTask2(string[] input)
        {
            int[] positions = input[0].Split(',').Select(i => int.Parse(i)).ToArray();
            int maxPos = positions.Max();
            int minPos = positions.Min();

            Dictionary<int, int> fuelUseDict = new();
            for (int i = minPos; i < maxPos + 1; i++)
            {
                int fuel = 0;
                foreach (int pos in positions)
                {
                    for (int j = 1; j < Math.Abs(pos - i) + 1; j++)
                    {
                        fuel += j;
                    }
                }

                fuelUseDict.Add(i, fuel);
            }

            return fuelUseDict.Values.Min();
        }
    }
}
