using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2021.Day4
{
    class Day4 : IDay
    {
        public void GetResults()
        {
            string[] input = File.ReadAllLines(@"Day4\input.txt");
            int[] draws = input[0].Split(',').Select(x => int.Parse(x)).ToArray();

            int task1 = SolveTask1(draws, input);
            Console.WriteLine($"Task 1 - Win first final score: {task1}");

            int task2 = SolveTask2(draws, input);
            Console.WriteLine($"Task 2 - Win last final score: {task2}");
        }

        private int SolveTask1(int[] draws, string[] input)
        {
            List<BingoBoard> boards = GetBoards(input);

            int score = 0;
            foreach (int draw in draws)
            {
                boards.ForEach(b => b.MarkValues(draw));

                if (boards.FirstOrDefault(x => x.HasWon) is BingoBoard board)
                {
                    score = board.UnmarkedSum * draw;
                    break;
                }
            }

            return score;
        }

        private int SolveTask2(int[] draws, string[] input)
        {
            List<BingoBoard> boards = GetBoards(input);

            int score = 0;
            foreach (int draw in draws)
            {
                boards.ForEach(b => b.MarkValues(draw));

                if (boards.Any(x => x.HasWon))
                {
                    if (boards.Count > 1)
                    {
                        boards.RemoveAll(x => x.HasWon);
                    }
                    else
                    {
                        score = boards.LastOrDefault(x => x.HasWon).UnmarkedSum * draw;
                        break;
                    }
                }
            }

            return score;
        }

        private List<BingoBoard> GetBoards(string[] input)
        {
            List<BingoBoard> boards = new List<BingoBoard>();
            for (int i = 2; i < input.Length; i += 6)
            {
                List<BingoValue[]> values = new List<BingoValue[]>
                {
                    Regex.Matches(input[i], @"(\d+)").Select(m => new BingoValue(int.Parse(m.Groups[1].Value))).ToArray(),
                    Regex.Matches(input[i + 1], @"(\d+)").Select(m => new BingoValue(int.Parse(m.Groups[1].Value))).ToArray(),
                    Regex.Matches(input[i + 2], @"(\d+)").Select(m => new BingoValue(int.Parse(m.Groups[1].Value))).ToArray(),
                    Regex.Matches(input[i + 3], @"(\d+)").Select(m => new BingoValue(int.Parse(m.Groups[1].Value))).ToArray(),
                    Regex.Matches(input[i + 4], @"(\d+)").Select(m => new BingoValue(int.Parse(m.Groups[1].Value))).ToArray(),
                };
                boards.Add(new BingoBoard(values));
            }
            return boards;
        }
    }

    class BingoBoard
    {
        public BingoValue[,] Board { get; private set; }
        public bool HasWon
        {
            get
            {
                // Check columns
                for (int i = 0; i < Board.GetLength(0); i++)
                {
                    if (Enumerable.Range(0, Board.GetLength(0)).Select(x => Board[x, i]).All(x => x.IsMarked))
                    {
                        return true;
                    }
                }

                // Check rows
                for (int i = 0; i < Board.GetLength(1); i++)
                {
                    if (Enumerable.Range(0, Board.GetLength(1)).Select(x => Board[i, x]).All(x => x.IsMarked))
                    {
                        return true;
                    }
                }

                return false;
            }
        }
        public int UnmarkedSum
        {
            get
            {
                int sum = 0;
                foreach (BingoValue bingoValue in Board)
                {
                    if (!bingoValue.IsMarked)
                    {
                        sum += bingoValue.Value;
                    }
                }
                return sum;
            }
        }

        public BingoBoard(List<BingoValue[]> values)
        {
            Board = new BingoValue[5, 5];
            for (int i = 0; i < values.Count; i++)
            {
                for (int j = 0; j < values[i].Length; j++)
                {
                    Board[i, j] = values[i][j];
                }
            }
        }

        public void MarkValues(int value)
        {
            foreach (BingoValue bingoValue in Board)
            {
                if (bingoValue.Value == value)
                {
                    bingoValue.IsMarked = true;
                }
            }
        }
        public void Reset()
        {
            foreach (BingoValue bingoValue in Board)
            {
                bingoValue.IsMarked = false;
            }
        }
    }

    class BingoValue
    {
        public int Value { get; private set; }
        public bool IsMarked { get; set; } = false;

        public BingoValue(int value)
        {
            Value = value;
        }
    }
}
