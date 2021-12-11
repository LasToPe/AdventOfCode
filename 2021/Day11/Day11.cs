using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day11
{
    // https://adventofcode.com/2021/day/11
    class Day11 : IDay
    {
        public void GetResults()
        {
            string[] input = File.ReadAllLines(@"Day11\input.txt");

            int task1 = SolveTask1(input);
            Console.WriteLine($"Task 1 - Total flashes after 100 steps: {task1}");

            int task2 = SolveTask2(input);
            Console.WriteLine($"Task 2 - First step with all octopi flashing: {task2}");
        }

        private int SolveTask1(string[] input)
        {
            List<Octopus> octopi = BuildOctopusSwarm(input);

            int flashes = 0;
            for (int i = 0; i < 100; i++)
            {
                octopi.ForEach(o => o.IncreaseEnergy());
                flashes += octopi.Count(o => o.IsFlashing);
                octopi.ForEach(o => o.Reset());
            }

            return flashes;
        }

        private int SolveTask2(string[] input)
        {
            List<Octopus> octopi = BuildOctopusSwarm(input);

            bool allFlashing = false;
            int step = 1;
            while (!allFlashing)
            {
                octopi.ForEach(o => o.IncreaseEnergy());
                allFlashing = octopi.All(o => o.IsFlashing);
                octopi.ForEach(o => o.Reset());

                step += allFlashing ? 0 : 1;
            }

            return step;
        }

        private List<Octopus> BuildOctopusSwarm(string[] input)
        {
            List<Octopus> octopi = new();
            for (int row = 0; row < input.Length; row++)
            {
                for (int col = 0; col < input[row].Length; col++)
                {
                    octopi.Add(new Octopus(col, row, int.Parse($"{input[row][col]}")));

                    var neighbours = octopi.Where(o => (o.X == col && o.Y == row - 1)               // Above
                                                        || (o.X == col + 1 && o.Y == row - 1)       // Above Right
                                                        || (o.X == col + 1 && o.Y == row)           // Right
                                                        || (o.X == col + 1 && o.Y == row + 1)       // Below Right
                                                        || (o.X == col && o.Y == row + 1)           // Below
                                                        || (o.X == col - 1 && o.Y == row + 1)       // Below Left
                                                        || (o.X == col - 1 && o.Y == row)           // Left
                                                        || (o.X == col - 1 && o.Y == row - 1));     // Above Left

                    octopi.Last().AddNeighbours(neighbours);
                }
            }

            return octopi;
        }
    }

    class Octopus
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int EnergyLevel { get; private set; }
        public List<Octopus> Neighbours { get; }

        public bool IsFlashing => EnergyLevel > 9;

        public Octopus(int x, int y, int initialEnergy)
        {
            X = x;
            Y = y;
            EnergyLevel = initialEnergy;
            Neighbours = new();
        }

        public void AddNeighbour(Octopus octopus)
        {
            Neighbours.Add(octopus);
            octopus.Neighbours.Add(this);
        }

        public void AddNeighbours(IEnumerable<Octopus> octopi)
        {
            foreach (Octopus octopus in octopi)
            {
                AddNeighbour(octopus);
            }
        }

        public void IncreaseEnergy()
        {
            if (IsFlashing)
            {
                return;
            }

            EnergyLevel += 1;

            if (IsFlashing)
            {
                Neighbours.ForEach(o => o.IncreaseEnergy());
            }
        }

        public void Reset()
        {
            if (IsFlashing)
            {
                EnergyLevel = 0;
            }
        }
    }
}
