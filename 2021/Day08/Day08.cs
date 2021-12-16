using AoCCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2021.Day08
{
    // https://adventofcode.com/2021/day/8
    class Day08 : IDay
    {
        public void GetResults()
        {
            string[] input = File.ReadAllLines(@"Day08\input.txt");

            int task1 = SolveTask1(input);
            Console.WriteLine($"Task 1 - Simple numbers count: {task1}");

            int task2 = SolveTask2(input);
            Console.WriteLine($"Task 2 - Output value sum: {task2}");
        }

        private int SolveTask1(string[] input)
        {
            string[] outputValues = input.Select(x => x.Split('|')[1].Trim()).ToArray();

            int[] lengths = new int[] { 2, 4, 3, 7 };

            int count = 0;
            foreach (string value in outputValues)
            {
                string[] values = Regex.Matches(value, @"(\w+)").Select(m => m.Value).ToArray();
                count += values.Count(x => lengths.Contains(x.Length));
            }

            return count;
        }

        private int SolveTask2(string[] input)
        {
            string[] wiringInfo = input.Select(x => x.Split('|')[0].Trim()).ToArray();
            string[] outputValues = input.Select(x => x.Split('|')[1].Trim()).ToArray();

            int sum = 0;
            for (int i = 0; i < input.Length; i++)
            {
                List<string> info = Regex.Matches(wiringInfo[i], @"(\w+)").Select(m => m.Value).ToList();
                List<string> actualValues = GetValues(info);

                string[] segmentValues = Regex.Matches(outputValues[i], @"(\w+)").Select(m => m.Value).ToArray();

                string intString = string.Empty;
                foreach (string segmentValue in segmentValues)
                {
                    intString += actualValues.IndexOf(actualValues.First(x => x.Intersect(segmentValue).Count() == segmentValue.Length && x.Length == segmentValue.Length));
                }

                int value = int.Parse(intString);
                sum += value;
            }

            return sum;
        }

        private List<string> GetValues(List<string> info)
        {
            // simple values
            string one = info.First(x => x.Length == 2);
            info.Remove(one);
            string four = info.First(x => x.Length == 4);
            info.Remove(four);
            string seven = info.First(x => x.Length == 3);
            info.Remove(seven);
            string eight = info.First(x => x.Length == 7);
            info.Remove(eight);
            // complex value
            string three = info.Where(x => x.Length == 5).First(x => x.Intersect(seven).Count() == seven.Length);
            info.Remove(three);
            string nine = info.Where(x => x.Length == 6).First(x => x.Intersect(three).Count() == three.Length);
            info.Remove(nine);
            string five = info.Where(x => x.Length == 5).First(x => x.Intersect(nine).Count() == 5);
            info.Remove(five);
            string two = info.First(x => x.Length == 5);
            info.Remove(two);
            string six = info.Where(x => x.Length == 6).First(x => x.Intersect(five).Count() == five.Length);
            info.Remove(six);
            string zero = info.Last();
            info.Remove(zero);

            return new List<string> { zero, one, two, three, four, five, six, seven, eight, nine };
        }
    }
}
