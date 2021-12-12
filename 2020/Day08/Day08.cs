using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2020.Day08
{
    // https://adventofcode.com/2020/day/8
    class Day08 : IDay
    {
        public void GetResults()
        {
            string[] input = File.ReadAllLines(@"Day08\input.txt");

            int task1 = SolveTask1(input);
            Console.WriteLine($"Task 1 - Accumulator value immediately before entering infinite loop: {task1}");

            int task2 = SolveTask2(input);
            Console.WriteLine($"Task 2 - Accumulator value after finishing fixed version: {task2}");
        }

        private int SolveTask1(string[] input)
        {
            int accumulator = 0;
            List<int> completedSteps = new();

            string[] instruction;
            int operationStep = 0;
            string operation;
            while (!completedSteps.Contains(operationStep))
            {
                completedSteps.Add(operationStep);
                instruction = input[operationStep].Split(' ');
                operation = instruction[0];

                if (operation == "acc")
                {
                    accumulator += int.Parse(instruction[1]);
                    operationStep += 1;
                }

                if (operation == "jmp")
                {
                    operationStep += int.Parse(instruction[1]);
                }

                if (operation == "nop")
                {
                    operationStep += 1;
                }
            }

            return accumulator;
        }

        private int SolveTask2(string[] input)
        {
            List<int> jmpOrNopInstructions = new();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i].StartsWith("jmp") || input[i].StartsWith("nop"))
                {
                    jmpOrNopInstructions.Add(i);
                }
            }

            foreach (int step in jmpOrNopInstructions)
            {
                List<string> testInput = new(input);
                testInput[step] = testInput[step].StartsWith("jmp") ? testInput[step].Replace("jmp", "nop") : testInput[step].Replace("nop", "jmp");

                int accumulator = 0;
                List<int> completedSteps = new();

                string[] instruction;
                int operationStep = 0;
                string operation;
                while (!completedSteps.Contains(operationStep) && operationStep < testInput.Count)
                {
                    completedSteps.Add(operationStep);
                    instruction = testInput[operationStep].Split(' ');
                    operation = instruction[0];

                    if (operation == "acc")
                    {
                        accumulator += int.Parse(instruction[1]);
                        operationStep += 1;
                    }

                    if (operation == "jmp")
                    {
                        operationStep += int.Parse(instruction[1]);
                    }

                    if (operation == "nop")
                    {
                        operationStep += 1;
                    }
                }

                if (operationStep == testInput.Count)
                    return accumulator;
            }

            throw new ArgumentException("Input program could not terminate with a single jmp/nop change");
        }
    }
}
