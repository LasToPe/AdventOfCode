using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2020.Day07
{
    class Day07 : IDay
    {
        public void GetResults()
        {
            string[] input = File.ReadAllLines(@"Day07\input.txt");

            int task1 = SolveTask1(input);
            Console.WriteLine($"Task 1 - Number of bags that can contain at least 1 shine gold bag: {task1}");

            int task2 = SolveTask2(input);
            Console.WriteLine($"Task 2 - Number of bags total that a shiny gold bag must contain: {task2}");
        }

        private int SolveTask1(string[] input)
        {
            IEnumerable<string> bags = GetBagsThatCanContainBagRecursive("shiny gold", input);
            int count = bags.Count();
            return count;
        }

        private int SolveTask2(string[] input)
        {
            IEnumerable<string> bags = GetBagsThatMustBeInBagRecursive("shiny gold", input);
            return bags.Count();
        }

        private IEnumerable<string> GetBagsThatCanContainBagRecursive(string bag, IEnumerable<string> rules)
        {
            List<string> newBags = new();

            foreach (string rule in rules)
            {
                string[] ruleParts = rule.Split("contain");
                if (ruleParts[1].Contains(bag))
                {
                    newBags.Add(ruleParts[0].Replace("bags", null).Trim());
                }
            }

            List<string> recursiveBags = new();
            foreach (string newBag in newBags)
            {
                recursiveBags.AddRange(GetBagsThatCanContainBagRecursive(newBag, rules));
            }

            newBags.AddRange(recursiveBags);
            return newBags.Distinct();
        }

        private IEnumerable<string> GetBagsThatMustBeInBagRecursive(string bag, IEnumerable<string> rules)
        {
            List<string> newBags = new();

            string rule = rules.First(r => r.StartsWith(bag));
            MatchCollection matches = Regex.Matches(rule.Split("contain")[1], @"(\d+)(.*?)(bag|bags)");

            foreach (Match match in matches)
            {
                for (int i = 0; i < int.Parse(match.Groups[1].Value); i++)
                {
                    newBags.Add(match.Groups[2].Value.Trim());
                }
            }

            List<string> recursiveBags = new();
            foreach (string newBag in newBags)
            {
                recursiveBags.AddRange(GetBagsThatMustBeInBagRecursive(newBag, rules));
            }

            newBags.AddRange(recursiveBags);
            return newBags;
        }
    }
}
