using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2020.Day09
{
    // https://adventofcode.com/2020/day/9
    class Day09 : IDay
    {
        public void GetResults()
        {
            string[] input = File.ReadAllLines(@"Day09\input.txt");

            long task1 = SolveTask1(input);
            Console.WriteLine($"Task 1 - First number that is not the sum of two of the previous 25 numbers: {task1}");

            long task2 = SolveTask2(input);
            Console.WriteLine($"Task 2 - Sum of smallest and largest numbers in contiguous set that adds up to {task1}: {task2}");
        }

        private long SolveTask1(string[] input)
        {
            var values = input.Select(x => long.Parse(x)).ToArray();

            long number = 0;
            for (int i = 25; i < values.Length; i++)
            {
                long value = values[i];

                bool valid = false;
                for (int j = i - 25; j < i; j++)
                {
                    if (values.Any(x => x + values[j] == value))
                    {
                        valid = true;
                        break;
                    }
                }

                if (!valid)
                {
                    number = value;
                    break;
                }
            }

            return number;
        }

        private long SolveTask2(string[] input)
        {
            long targetValue = SolveTask1(input);
            var values = input.Select(x => long.Parse(x)).ToArray();

            List<List<long>> contiguousSets = new();
            for (int i = 0; i < values.Length; i++)
            {
                List<long> set = new() { values[i] };
                for (int j = i + 1; j < values.Length; j++)
                {
                    set.Add(values[j]);
                    if (set.Sum() == targetValue)
                    {
                        contiguousSets.Add(set);
                        break;
                    }
                    if (set.Sum() > targetValue)
                    {
                        break;
                    }
                }
            }

            var firstSet = contiguousSets.First();
            return firstSet.Min() + firstSet.Max();
        }
    }
}
