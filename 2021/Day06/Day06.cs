using AoCCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day06
{
    // https://adventofcode.com/2021/day/6
    class Day06 : IDay
    {
        public void GetResults()
        {
            string[] input = File.ReadAllLines(@"Day06\input.txt");

            ulong task1 = SolveTask(input, 80);
            Console.WriteLine($"Task 1 - Lanternfish count after 80 days: {task1}");

            ulong task2 = SolveTask(input, 256);
            Console.WriteLine($"Task 2 - Lanternfish count after 256 days: {task2}");
        }

        private ulong SolveTask(string[] input, int days)
        {
            List<sbyte> initialState = input[0].Split(',').Select(s => sbyte.Parse(s)).ToList();

            Dictionary<sbyte, ulong> initialCounts = Enumerable.Range(0, 9).ToDictionary(k => (sbyte)k, v => (ulong)0);
            initialState.ForEach(f => initialCounts[f] += 1);
            List<Dictionary<sbyte, ulong>> dayCounts = new List<Dictionary<sbyte, ulong>> { initialCounts };

            for (int i = 1; i < days + 1; i++)
            {
                var dayBefore = dayCounts[i - 1];
                Dictionary<sbyte, ulong> currentDay = new();
                
                foreach (sbyte key in dayBefore.Keys)
                {
                    if (key == 8)
                    {
                        currentDay[key] = dayBefore[0];
                    }
                    else if (key == 6)
                    {
                        currentDay[key] = dayBefore[(sbyte)(key + 1)] + dayBefore[0];
                    }
                    else
                    {
                        currentDay[key] = dayBefore[(sbyte)(key + 1)];
                    }
                }

                dayCounts.Add(currentDay);
            }

            ulong count = 0;
            foreach (ulong c in dayCounts.Last().Values)
            {
                count += c;
            }

            return count;
        }
    }
}
