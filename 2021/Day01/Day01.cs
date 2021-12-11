using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day01
{
    // https://adventofcode.com/2021/day/1
    class Day01 : IDay
    {
        public void GetResults()
        {
            string[] input = File.ReadAllLines(@"Day01\input.txt");

            int task1 = Task1Result(input);
            Console.WriteLine($"Task 1 - Increases: {task1}");

            int task2 = Task2Result(input);
            Console.WriteLine($"Task 2 - Increases: {task2}");
        }

        private int Task1Result(string[] input)
        {
            int[] values = input.Select(x => int.Parse(x)).ToArray();

            int increases = 0;
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] > values[i - 1])
                {
                    increases += 1;
                }
            }

            return increases;
        }

        private int Task2Result(string[] input)
        {
            int[] values = input.Select(x => int.Parse(x)).ToArray();
            
            List<int> slidingWindowValues = new();
            for (int i = 0; i < values.Length - 2; i++)
            {
                slidingWindowValues.Add(values[i] + values[i + 1] + values[i + 2]);
            }

            int increases = 0;
            for (int i = 1; i < slidingWindowValues.Count; i++)
            {
                if (slidingWindowValues[i] > slidingWindowValues[i - 1])
                {
                    increases += 1;
                }
            }

            return increases;
        }
    }
}
