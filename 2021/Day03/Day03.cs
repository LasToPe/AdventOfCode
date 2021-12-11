using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day03
{
    // https://adventofcode.com/2021/day/3
    class Day03 : IDay
    {
        public void GetResults()
        {
            string[] input = File.ReadAllLines(@"Day03\input.txt");

            int task1 = CalculatePowerConsumption(input);
            Console.WriteLine($"Task 1 - Power Consumption: {task1}");

            int task2 = CalculateLifeSupportRating(input);
            Console.WriteLine($"Task 2 - Life Support Rating: {task2}");
        }

        private int CalculatePowerConsumption(string[] input)
        {
            int binaryLength = input[0].Length;
            string gamma = string.Empty;
            string epsilon = string.Empty;

            for (int i = 0; i < binaryLength; i++)
            {
                gamma += input.Count(x => x.Substring(i, 1) == "1") > input.Length / 2 ? "1" : "0";
                epsilon += input.Count(x => x.Substring(i, 1) == "1") < input.Length / 2 ? "1" : "0";
            }

            int gammaDec = Convert.ToInt32(gamma, 2);
            int epsilonDec = Convert.ToInt32(epsilon, 2);

            return gammaDec * epsilonDec;
        }

        private int CalculateLifeSupportRating(string[] input)
        {
            int binaryLength = input[0].Length;

            string oxygenGeneratorRating = string.Empty;
            string[] filteredOxygenValues = input;
            for (int i = 0; i < binaryLength; i++)
            {
                string mostCommonBit = filteredOxygenValues.Count(x => x.Substring(i, 1) == "1") >= filteredOxygenValues.Length / 2f ? "1" : "0";
                filteredOxygenValues = filteredOxygenValues.Where(x => x.Substring(i, 1) == mostCommonBit).ToArray();

                if (filteredOxygenValues.Length == 1)
                {
                    oxygenGeneratorRating = filteredOxygenValues.First();
                    break;
                }
            }

            string co2ScrubberRating = string.Empty;
            string[] filteredCo2Values = input;
            for (int i = 0; i < binaryLength; i++)
            {
                string leastCommonBit = filteredCo2Values.Count(x => x.Substring(i, 1) == "1") < filteredCo2Values.Length / 2f ? "1" : "0";
                filteredCo2Values = filteredCo2Values.Where(x => x.Substring(i, 1) == leastCommonBit).ToArray();

                if (filteredCo2Values.Length == 1)
                {
                    co2ScrubberRating = filteredCo2Values.First();
                    break;
                }
            }

            int oxygenDec = Convert.ToInt32(oxygenGeneratorRating, 2);
            int co2Dec = Convert.ToInt32(co2ScrubberRating, 2);

            return oxygenDec * co2Dec;
        }
    }
}
