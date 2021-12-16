using AoCCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2020.Day01
{
    // https://adventofcode.com/2020/day/1
    class Day01 : IDay
    {
        public void GetResults()
        {
            string[] input = File.ReadAllLines(@"Day01\input.txt");

            long task1 = SolveTask1(input);
            Console.WriteLine($"Task 1 - Product of 2 entries adding up to 2020: {task1}");

            long task2 = SolveTask2(input);
            Console.WriteLine($"Task 2 - Product of 3 entries adding up to 2020: {task2}");
        }

        private long SolveTask1(string[] input)
        {
            int[] values = input.Select(x => int.Parse(x)).ToArray();

            long product = 0;
            foreach (int val in values)
            {
                if (values.Any(x => x + val == 2020))
                {
                    product = values.First(x => x + val == 2020) * val;
                    break;
                }
            }

            return product;
        }

        private long SolveTask2(string[] input)
        {
            int[] values = input.Select(x => int.Parse(x)).ToArray();

            long product = 0;
            foreach (int val in values)
            {
                foreach (int val2 in values.Where(x => x != val))
                {
                    if (values.Any(x => x + val + val2 == 2020))
                    {
                        product = values.First(x => x + val + val2 == 2020) * val * val2;
                        break;
                    }
                }
                if (product != 0) break;
            }

            return product;
        }
    }
}
