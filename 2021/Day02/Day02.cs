using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2021.Day02
{
    // https://adventofcode.com/2021/day/2
    class Day02 : IDay
    {
        public void GetResults()
        {
            string[] input = File.ReadAllLines(@"Day02\input.txt");

            int task1 = Task1Result(input);
            Console.WriteLine($"Task 1 - Result: {task1}");

            int task2 = Task2Result(input);
            Console.WriteLine($"Task 2 - Result: {task2}");
        }

        private int Task1Result(string[] input)
        {
            int horizontalChangeSum = input.Where(x => Regex.IsMatch(x, "forward")).Sum(x => int.Parse(Regex.Match(x, @"(\d+)").Groups[1].Value));
            int depthChangeSum = input.Where(x => Regex.IsMatch(x, "down")).Sum(x => int.Parse(Regex.Match(x, @"(\d+)").Groups[1].Value));
            depthChangeSum -= input.Where(x => Regex.IsMatch(x, "up")).Sum(x => int.Parse(Regex.Match(x, @"(\d+)").Groups[1].Value));

            return horizontalChangeSum * depthChangeSum;
        }

        private int Task2Result(string[] input)
        {
            int horizontalChangeSum = 0;
            int depthChangeSum = 0;
            int aim = 0;

            foreach (string command in input)
            {
                Match down = Regex.Match(command, @"down (\d+)");
                if (down.Success)
                {
                    aim += int.Parse(down.Groups[1].Value);
                    continue;
                }

                Match up = Regex.Match(command, @"up (\d+)");
                if (up.Success)
                {
                    aim -= int.Parse(up.Groups[1].Value);
                    continue;
                }

                Match forward = Regex.Match(command, @"forward (\d+)");
                if (forward.Success)
                {
                    int horizontalChange = int.Parse(forward.Groups[1].Value);
                    horizontalChangeSum += horizontalChange;
                    depthChangeSum += aim * horizontalChange;
                }
            }

            return horizontalChangeSum * depthChangeSum;
        }
    }
}
