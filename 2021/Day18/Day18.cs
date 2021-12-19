using AoCCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2021.Day18
{
    // https://adventofcode.com/2021/day/18
    class Day18 : IDay
    {
        public void GetResults()
        {
            string[] input = File.ReadAllLines(@"Day18\input.txt");

            var task1 = SolveTask1(input);
            Console.WriteLine($"Task 1 - Magnitude for the final sum: {task1}");

            var task2 = SolveTask2(input);
            Console.WriteLine($"Task 2 - Highest magnitude for the sum of 2 numbers: {task2}");
        }

        private object SolveTask1(string[] input)
        {
            string currentSum = input[0];
            for (int i = 1; i < input.Length; i++)
            {
                currentSum = $"[{currentSum},{input[i]}]";
                currentSum = ReduceSnailfishNumber(currentSum);
            }

            long magnitude = GetMagnitude(currentSum);
            return magnitude;
        }

        private object SolveTask2(string[] input)
        {
            long highestMagnitude = 0;
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input.Length; j++)
                {
                    if (i == j) continue;
                    if (input[i] == input[j]) continue;

                    string result = ReduceSnailfishNumber($"[{input[i]},{input[j]}]");
                    long magnitude = GetMagnitude(result);

                    if (magnitude > highestMagnitude)
                        highestMagnitude = magnitude;
                }
            }

            return highestMagnitude;
        }

        private string ReduceSnailfishNumber(string number)
        {
            bool shouldExplode = ShouldExplode(number);
            bool shouldSplit = Regex.IsMatch(number, @"\d{2,}");
            while (shouldExplode || shouldSplit)
            {
                if (shouldExplode)
                {
                    string explodeString = new(number);
                    string pattern = GetExplodePattern(explodeString);
                    Match explodingMatch = Regex.Match(explodeString, pattern);
                    int left = int.Parse(explodingMatch.Groups[2].Value);
                    int right = int.Parse(explodingMatch.Groups[3].Value);
                    string[] parts = new string[]
                    {
                        explodeString.Substring(0, explodingMatch.Groups[1].Index),
                        explodeString[(explodingMatch.Groups[1].Index + explodingMatch.Groups[1].Length)..]
                    };

                    Match previousMatch = Regex.Match(parts[0], @".*(\,|\[)(\d+)");
                    if (previousMatch.Success)
                    {
                        int previous = int.Parse(previousMatch.Groups[2].Value) + left;
                        parts[0] = parts[0].Remove(previousMatch.Groups[2].Index, previousMatch.Groups[2].Length).Insert(previousMatch.Groups[2].Index, previous.ToString());
                    }
                    Match followingMatch = Regex.Match(parts[1], @".*?(\d+)");
                    if (followingMatch.Success)
                    {
                        int following = int.Parse(followingMatch.Groups[1].Value) + right;
                        parts[1] = parts[1].Remove(followingMatch.Groups[1].Index, followingMatch.Groups[1].Length).Insert(followingMatch.Groups[1].Index, following.ToString());
                    }

                    explodeString = string.Join("", parts[0], "0", parts[1]);
                    number = explodeString;
                }
                else if (shouldSplit)
                {
                    string splitString = new(number);
                    Match splitMatch = Regex.Match(number, @"(\d{2,})");
                    int left = (int)Math.Floor(double.Parse(splitMatch.Groups[1].Value) / 2);
                    int right = (int)Math.Ceiling(double.Parse(splitMatch.Groups[1].Value) / 2);

                    splitString = splitString.Remove(splitMatch.Groups[1].Index, splitMatch.Groups[1].Length).Insert(splitMatch.Groups[1].Index, $"[{left},{right}]");
                    number = splitString;
                }

                shouldExplode = ShouldExplode(number);
                shouldSplit = Regex.IsMatch(number, @"\d{2,}");
            }

            return number;
        }
        private bool ShouldExplode(string number)
        {
            bool shouldExplode = false;

            Queue<char> explodeQueue = new(number);
            Stack<char> explodeStack = new();
            while (explodeQueue.Any())
            {
                char c = explodeQueue.Dequeue();
                if (c == '[') explodeStack.Push(c);
                else if (c == ']') explodeStack.Pop();

                if (explodeStack.Count == 5)
                {
                    shouldExplode = true;
                    break;
                }
            }

            return shouldExplode;
        }
        private string GetExplodePattern(string number)
        {
            string explodePattern = string.Empty;

            Queue<char> explodeQueue = new(number);
            Stack<char> explodeStack = new();
            while (explodeQueue.Any())
            {
                char c = explodeQueue.Dequeue();
                if (c == '[')
                {
                    explodeStack.Push(c);
                    if (explodeStack.Count == 5)
                    {
                        explodePattern += @"(\[(\d+)\,(\d+)\])";
                        break;
                    }
                    else
                    {
                        explodePattern += $@"\{c}.*?";
                    }
                }
                else if (c == ']')
                {
                    explodeStack.Pop();
                    explodePattern += $@"\{c}.*?";
                }
            }

            return explodePattern;
        }
        private long GetMagnitude(string number)
        {
            string magnitude = new(number);

            while (magnitude.Contains('['))
            {
                foreach(Match match in Regex.Matches(magnitude, @"(\[(\d+)\,(\d+)\])"))
                {
                    int left = int.Parse(match.Groups[2].Value) * 3;
                    int right = int.Parse(match.Groups[3].Value) * 2;

                    var ex = new Regex($@"(\[({match.Groups[2].Value})\,({match.Groups[3].Value})\])");
                    magnitude = ex.Replace(magnitude, $"{left + right}", 1);
                }
            }

            return long.Parse(magnitude);
        }
    }
}
