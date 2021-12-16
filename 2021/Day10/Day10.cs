using AoCCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day10
{
    // https://adventofcode.com/2021/day/10
    class Day10 : IDay
    {
        public void GetResults()
        {
            string[] input = File.ReadAllLines(@"Day10\input.txt");
            Dictionary<char, char> pairs = new Dictionary<char, char>
            {
                { '(', ')' },
                { '[', ']' },
                { '{', '}' },
                { '<', '>' },
            };

            int task1 = SolveTask1(input, pairs);
            Console.WriteLine($"Task 1 - Syntax error score: {task1}");

            long task2 = SolveTask2(input, pairs);
            Console.WriteLine($"Task 2 - Middle score: {task2}");
        }

        private int SolveTask1(string[] input, Dictionary<char, char> pairs)
        {
            return SolveTask1(input, pairs, out var _);
        }

        private int SolveTask1(string[] input, Dictionary<char, char> pairs, out List<string> corruptedLines)
        {
            Dictionary<char, int> errorScores = new Dictionary<char, int>
            {
                { ')', 3 },
                { ']', 57 },
                { '}', 1197 },
                { '>', 25137 },
            };

            corruptedLines = new List<string>();
            int syntaxErrorScore = 0;
            foreach (string line in input)
            {
                Stack<char> characterStack = new Stack<char>();
                foreach (char c in line)
                {
                    if (pairs.Keys.Any(x => x == c))
                    {
                        characterStack.Push(c);
                    }
                    else if (pairs[characterStack.Peek()] == c)
                    {
                        _ = characterStack.Pop();
                    }
                    else
                    {
                        syntaxErrorScore += errorScores[c];
                        corruptedLines.Add(line);
                        break;
                    }
                }
            }

            return syntaxErrorScore;
        }

        private long SolveTask2(string[] input, Dictionary<char, char> pairs)
        {
            Dictionary<char, int> scores = new Dictionary<char, int>
            {
                { ')', 1 },
                { ']', 2 },
                { '}', 3 },
                { '>', 4 },
            };

            SolveTask1(input, pairs, out var corruptedLines);
            string[] incompleteLines = input.Where(x => !corruptedLines.Contains(x)).ToArray();
            List<long> lineScores = new();

            foreach (string line in incompleteLines)
            {
                Stack<char> characterStack = new Stack<char>();
                foreach (char c in line)
                {
                    if (pairs.Keys.Any(x => x == c))
                    {
                        characterStack.Push(c);
                    }
                    else if (pairs[characterStack.Peek()] == c)
                    {
                        _ = characterStack.Pop();
                    }
                }

                long lineScore = 0;
                while (characterStack.Any())
                {
                    lineScore *= 5;
                    lineScore += scores[pairs[characterStack.Pop()]];
                }
                lineScores.Add(lineScore);
            }

            lineScores = lineScores.OrderBy(x => x).ToList();
            long middleScore = lineScores.ElementAt(lineScores.Count / 2);

            return middleScore;
        }
    }
}
