using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day14
{
    class Day14 : IDay
    {
        private readonly string _inputPath = @"Day14\input.txt";
        private readonly Dictionary<string, string> _lookup = new();

        public void GetResults()
        {
            string[] input = File.ReadAllLines(_inputPath);
            string initial = input[0];
            for (int i = 2; i < input.Length; i++)
            {
                string[] val = input[i].Split("->");
                _lookup.Add(val[0].Trim(), val[0].Trim().Insert(1, val[1].Trim()));
            }

            int task1 = SolveTask1(initial);
            Console.WriteLine($"Task 1 - Most common - least common characters in polymer after 10 steps: {task1}");

            ulong task2 = SolveTask2(initial);
            Console.WriteLine($"Task 2 - Most common - least common characters in polymer after 40 steps: {task2}");
        }

        private int SolveTask1(string initial)
        {
            string polymer = new string(initial);

            for (int i = 0; i < 10; i++)
            {
                string newStr = string.Empty;
                for (int c = 0; c < polymer.Length - 1; c++)
                {
                    string str = polymer.Substring(c, 2);
                    newStr += _lookup[str].Substring(0, 2);
                }
                newStr += polymer.Last();
                polymer = newStr;
            }

            var counts = polymer.GroupBy(x => x).Select(x => x.Count()).OrderByDescending(x => x);

            return counts.First() - counts.Last();
        }

        private ulong SolveTask2(string initial)
        {
            string polymer = new string(initial);

            Dictionary<string, ulong> pairs = new();
            for (int i = 0; i < polymer.Length - 1; i++)
            {
                if (pairs.ContainsKey(polymer.Substring(i, 2))) pairs[polymer.Substring(i, 2)] += 1;
                else pairs.Add(polymer.Substring(i, 2), 1);
            }

            for (int i = 0; i < 40; i++)
            {
                Dictionary<string, ulong> newPairs = new();
                foreach (var pair in pairs)
                {
                    string val = _lookup[pair.Key];
                    string leftNode = val.Substring(0, 2);
                    string rightNode = val.Substring(1, 2);

                    if (newPairs.ContainsKey(leftNode))
                        newPairs[leftNode] += pair.Value;
                    else
                        newPairs.Add(leftNode, pair.Value);

                    if (newPairs.ContainsKey(rightNode))
                        newPairs[rightNode] += pair.Value;
                    else
                        newPairs.Add(rightNode, pair.Value);
                }

                pairs = newPairs;
            }

            var counts = pairs.Select(x => new { c = x.Key.First(), val = x.Value })
                              .GroupBy(x => x.c)
                              .Select(x => new { c = x.Key, val = x.Sum(v => (decimal)v.val) })
                              .OrderByDescending(v => v.val);

            ulong count = (ulong)(counts.First().val - counts.Last().val);
            if (counts.First().c == polymer.Last())
            {
                count += 1;
            }
            else if (counts.Last().c == polymer.Last())
            {
                count -= 1;
            }

            return count;
        }
    }
}
