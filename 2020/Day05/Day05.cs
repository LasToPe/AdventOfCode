using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2020.Day05
{
    // https://adventofcode.com/2020/day/5
    class Day05 : IDay
    {
        public void GetResults()
        {
            string[] input = File.ReadAllLines(@"Day05\input.txt");

            int task1 = SolveTask1(input);
            Console.WriteLine($"Task 1 - Highest seat Id: {task1}");

            var task2 = SolveTask2(input);
            Console.WriteLine($"Task 2 - My seat Id: {task2}");
        }

        private int SolveTask1(string[] input)
        {
            return SolveTask1(input, out var _);
        }

        private int SolveTask1(string[] input, out List<int> seatIds)
        {
            seatIds = new();
            foreach (string line in input)
            {
                int minRow = 0;
                int maxRow = 127;
                int minSeat = 0;
                int maxSeat = 7;

                int row = 0;
                int seat = 0;

                foreach (char c in line)
                {
                    switch (c)
                    {
                        case 'F':
                            if (maxRow - minRow > 1)
                                maxRow -= (int)Math.Round((maxRow - minRow) / 2d);
                            else
                                row = minRow;
                            break;
                        case 'B':
                            if (maxRow - minRow > 1)
                                minRow += (int)Math.Round((maxRow - minRow) / 2d);
                            else
                                row = maxRow;
                            break;
                        case 'L':
                            if (maxSeat - minSeat > 1)
                                maxSeat -= (int)Math.Round((maxSeat - minSeat) / 2d);
                            else
                                seat = minSeat;
                            break;
                        case 'R':
                            if (maxSeat - minSeat > 1)
                                minSeat += (int)Math.Round((maxSeat - minSeat) / 2d);
                            else
                                seat = maxSeat;
                            break;
                        default: break;
                    }
                }

                seatIds.Add(row * 8 + seat);
            }

            return seatIds.Max();
        }

        private int SolveTask2(string[] input)
        {
            SolveTask1(input, out List<int> seatIds);
            var orderedIds = seatIds.OrderBy(x => x).ToList();

            int mySeatId = 0;
            for (int i = 0; i < orderedIds.Count; i++)
            {
                if (orderedIds[i] == orderedIds[i + 1] - 2)
                {
                    mySeatId = orderedIds[i] + 1;
                    break;
                }
            }

            return mySeatId;
        }
    }
}
