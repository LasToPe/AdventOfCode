using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2020.Day06
{
    // https://adventofcode.com/2020/day/6
    class Day06 : IDay
    {
        public void GetResults()
        {
            string[] input = File.ReadAllLines(@"Day06\input.txt");

            int task1 = SolveTask1(input);
            Console.WriteLine($"Task 1 - Sum of questions anyone answered with 'yes': {task1}");

            int task2 = SolveTask2(input);
            Console.WriteLine($"Task 2 - Sum of questions everyone answered with 'yes': {task2}");
        }

        private int SolveTask1(string[] input)
        {
            List<string> groups = new();
            for (int i = 0; i < input.Length; i++)
            {
                if (i == 0)
                {
                    groups.Add(input[i]);
                    continue;
                }

                if (input[i].Length == 0)
                {
                    groups.Add(string.Empty);
                }
                else
                {
                    string g = groups.Last();
                    groups.Remove(g);
                    g += input[i];
                    groups.Add(g);
                }
            }

            int sum = groups.Sum(x => x.Distinct().Count());

            return sum;
        }

        private int SolveTask2(string[] input)
        {
            List<List<string>> groups = new();
            for (int i = 0; i < input.Length; i++)
            {
                if (i == 0)
                {
                    groups.Add(new List<string> { input[i] });
                    continue;
                }

                if (input[i].Length == 0)
                {
                    groups.Add(new List<string>());
                }
                else
                {
                    groups.Last().Add(input[i]);
                }
            }

            //var letterRange = Enumerable.Range('a', 'z' - 'a' + 1).Select(i => (char)i);

            int count = 0;
            foreach (var group in groups)
            {
                IEnumerable<char> intersection = group[0].AsEnumerable();
                for (int i = 1; i < group.Count; i++)
                {
                    intersection = intersection.Intersect(group[i]);
                }

                count += intersection.Count();
            }

            return count;
        }
    }
}
