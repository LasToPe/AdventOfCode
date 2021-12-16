using AoCCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2020.Day10
{
    // https://adventofcode.com/2020/day/10
    class Day10 : IDay
    {
        public void GetResults()
        {
            string[] input = File.ReadAllLines(@"Day10\input.txt");

            var task1 = SolveTask1(input);
            Console.WriteLine($"Task 1 - 1-jolt differences * 3-jolt differences: {task1}");

            var task2 = SolveTask2(input);
        }

        private object SolveTask1(string[] input)
        {
            var adapters = input.Select(s => int.Parse(s)).OrderBy(i => i).ToArray();

            int oneJoltDiffs = 0;
            int threeJoltDiffs = 1;
            for (int i = 0; i < adapters.Length; i++)
            {
                int comparator = i == 0 ? 0 : adapters[i - 1];
                if (adapters[i] - 1 == comparator) oneJoltDiffs++;
                if (adapters[i] - 3 == comparator) threeJoltDiffs++;
            }

            return oneJoltDiffs * threeJoltDiffs;
        }

        private object SolveTask2(string[] input)
        {
            var adapters = input.Select(s => int.Parse(s)).ToList();
            adapters.Add(0);
            adapters.Add(adapters.Max() + 3);
            adapters = adapters.OrderBy(i => i).ToList();

            List<ulong> counts = new() { 1 };

            for (int i = 1; i < adapters.Count; i++)
            {
                ulong current = 0;
                for (int j = i - 1; j >= 0 && adapters[i] - adapters[j] < 4; j--)
                {
                    current += counts[j];
                }
                counts.Add(current);
            }

            return counts.Last();
        }
    }
}
