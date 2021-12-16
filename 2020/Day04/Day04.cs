using AoCCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2020.Day04
{
    // https://adventofcode.com/2020/day/4
    class Day04 : IDay
    {
        public void GetResults()
        {
            string[] input = File.ReadAllLines(@"Day04\input.txt");
            List<string> batches = new();
            for (int i = 0; i < input.Length; i++)
            {
                if (i == 0)
                {
                    batches.Add(input[0]);
                    continue;
                }

                if (input[i] == string.Empty)
                {
                    batches.Add(string.Empty);
                }
                else
                {
                    string latest = batches.Last();
                    batches.Remove(latest);
                    latest += " " + input[i];
                    batches.Add(latest.Trim());
                }
            }

            List<string> requiredFields = new List<string>
            {
                "byr", // (Birth Year)
                "iyr", // (Issue Year)
                "eyr", // (Expiration Year)
                "hgt", // (Height)
                "hcl", // (Hair Color)
                "ecl", // (Eye Color)
                "pid", // (Passport ID)
                //"cid", // (Country ID)
            };

            int task1 = SolveTask1(batches, requiredFields);
            Console.WriteLine($"Task 1 - Valid passports, no missing fields: {task1}");

            var task2 = SolveTask2(batches, requiredFields);
            Console.WriteLine($"Task 2 - Valid passports, no missing fields or invalid values: {task2}");
        }

        private int SolveTask1(List<string> batches, List<string> requiredFields)
        {
            int invalidCount = 0;

            foreach (string passport in batches)
            {
                foreach (string field in requiredFields)
                {
                    if (!passport.Contains(field + ":"))
                    {
                        invalidCount += 1;
                        break;
                    }
                }
            }

            return batches.Count - invalidCount;
        }

        private object SolveTask2(List<string> batches, List<string> requiredFields)
        {
            int invalidCount = 0;
            List<string> invalid = new();

            foreach (string passport in batches)
            {
                foreach (string _field in requiredFields)
                {
                    string value = Regex.Match(passport, $@"{_field}\:(\S+)").Groups[1].Value;

                    if (string.IsNullOrWhiteSpace(value))
                    {
                        invalidCount += 1;
                        invalid.Add(passport);
                        break;
                    }

                    if (_field == "byr")
                    {
                        if (value.Length != 4)
                        {
                            invalidCount += 1;
                            invalid.Add(passport);
                            break;
                        }
                        if (int.TryParse(value, out int year))
                        {
                            if (year < 1920 || year > 2002)
                            {
                                invalidCount += 1;
                                invalid.Add(passport);
                                break;
                            }
                        }
                    }

                    if (_field == "iyr")
                    {
                        if (value.Length != 4)
                        {
                            invalidCount += 1;
                            invalid.Add(passport);
                            break;
                        }
                        if (int.TryParse(value, out int year))
                        {
                            if (year < 2010 || year > 2020)
                            {
                                invalidCount += 1;
                                invalid.Add(passport);
                                break;
                            }
                        }
                    }

                    if (_field == "eyr")
                    {
                        if (value.Length != 4)
                        {
                            invalidCount += 1;
                            invalid.Add(passport);
                            break;
                        }
                        if (int.TryParse(value, out int year))
                        {
                            if (year < 2020 || year > 2030)
                            {
                                invalidCount += 1;
                                invalid.Add(passport);
                                break;
                            }
                        }
                    }

                    if (_field == "hgt")
                    {
                        Match heightMatch = Regex.Match(value, @"(\d+)(cm|in)");
                        if (!heightMatch.Success)
                        {
                            invalidCount += 1;
                            invalid.Add(passport);
                            break;
                        }
                        
                        int height = int.Parse(heightMatch.Groups[1].Value);
                        if (heightMatch.Groups[2].Value == "cm")
                        {
                            if (height < 150 || height > 193)
                            {
                                invalidCount += 1;
                                invalid.Add(passport);
                                break;
                            }
                        }
                        if (heightMatch.Groups[2].Value == "in")
                        {
                            if (height < 59 || height > 76)
                            {
                                invalidCount += 1;
                                invalid.Add(passport);
                                break;
                            }
                        }
                    }

                    if (_field == "hcl")
                    {
                        if (value.Length != 7 || !Regex.IsMatch(value, @"#[0-9a-f]{6}"))
                        {
                            invalidCount += 1;
                            invalid.Add(passport);
                            break;
                        }
                    }

                    if (_field == "ecl")
                    {
                        if (!Regex.IsMatch(value, @"(amb|blu|brn|gry|grn|hzl|oth)"))
                        {
                            invalidCount += 1;
                            invalid.Add(passport);
                            break;
                        }
                    }

                    if (_field == "pid")
                    {
                        if (value.Length > 9 || !Regex.IsMatch(value, @"\d{9}"))
                        {
                            invalidCount += 1;
                            invalid.Add(passport);
                            break;
                        }
                    }

                }
            }

            return batches.Count - invalidCount;
        }
    }
}
