using AoCCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2020.Day02
{
    // https://adventofcode.com/2020/day/2
    class Day02 : IDay
    {
        public void GetResults()
        {
            string[] input = File.ReadAllLines(@"Day02\input.txt");

            int task1 = SolveTask1(input);
            Console.WriteLine($"Task 1 - Number of valid passwords: {task1}");

            var task2 = SolveTask2(input);
            Console.WriteLine($"Task 2 - Number of valid passwords: {task2}");
        }

        private int SolveTask1(string[] input)
        {
            int count = 0;
            foreach (string line in input)
            {
                string[] parts = line.Split(':').Select(x => x.Trim()).ToArray();

                int min = int.Parse(Regex.Matches(parts[0], @"(\d+)").Select(m => m.Value).First());
                int max = int.Parse(Regex.Matches(parts[0], @"(\d+)").Select(m => m.Value).Last());
                char character = Regex.Match(parts[0], @"([a-z])").Value[0];

                if (parts[1].Count(x => x == character) >= min
                    && parts[1].Count(x => x == character) <= max)
                {
                    count += 1;
                }
            }

            return count;
        }

        private int SolveTask2(string[] input)
        {
            int count = 0;
            foreach (string line in input)
            {
                string[] parts = line.Split(':').Select(x => x.Trim()).ToArray();

                int pos1 = int.Parse(Regex.Matches(parts[0], @"(\d+)").Select(m => m.Value).First()) - 1;
                int pos2 = int.Parse(Regex.Matches(parts[0], @"(\d+)").Select(m => m.Value).Last()) - 1;
                char character = Regex.Match(parts[0], @"([a-z])").Value[0];

                if ((parts[1][pos1] == character && parts[1][pos2] != character)
                    || (parts[1][pos1] != character && parts[1][pos2] == character))
                {
                    count += 1;
                }
            }

            return count;
        }
    }
}
