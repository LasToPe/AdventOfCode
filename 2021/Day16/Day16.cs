using AoCCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day16
{
    // https://adventofcode.com/2021/day/16
    class Day16 : IDay
    {
        private readonly Dictionary<char, string> _hexToBinaryMap = new()
        {
            { '0', "0000" },
            { '1', "0001" },
            { '2', "0010" },
            { '3', "0011" },
            { '4', "0100" },
            { '5', "0101" },
            { '6', "0110" },
            { '7', "0111" },
            { '8', "1000" },
            { '9', "1001" },
            { 'A', "1010" },
            { 'B', "1011" },
            { 'C', "1100" },
            { 'D', "1101" },
            { 'E', "1110" },
            { 'F', "1111" },
        };
        public void GetResults()
        {
            string input = File.ReadAllLines(@"Day16\input.txt")[0];

            var task1 = SolveTask1(input);
            Console.WriteLine($"Task 1 - Sum of version numbers of all packets: {task1}");

            var task2 = SolveTask2(input);
            Console.WriteLine($"Task 2 - Result of transmission: {task2}");
        }

        private object SolveTask1(string input)
        {
            string binary = string.Join(string.Empty, input.Select(c => _hexToBinaryMap[c]));
            
            var packets = ProcessQueue(new Queue<char>(binary));

            long versionNumberSum = 0;
            foreach (Packet packet in packets)
            {
                versionNumberSum += packet.GetVersionNumberSumRecursive();
            }

            return versionNumberSum;
        }

        private object SolveTask2(string input)
        {
            string binary = string.Join(string.Empty, input.Select(c => _hexToBinaryMap[c]));

            var packets = ProcessQueue(new Queue<char>(binary));

            long evaluatedValue = packets.First().GetOperationValueRecursive();

            return evaluatedValue;
        }

        private List<Packet> ProcessQueue(Queue<char> queue, int packetsContained = int.MaxValue)
        {
            List<Packet> packets = new();

            while (queue.Any() && packets.Count < packetsContained)
            {
                if (queue.All(c => c == '0'))
                {
                    queue.Dequeue(queue.Count);
                    continue;
                }

                string version = string.Join(string.Empty, queue.Dequeue(3));
                string typeId = string.Join(string.Empty, queue.Dequeue(3));

                Packet packet = new(Convert.ToInt32(version, 2), Convert.ToInt32(typeId, 2));

                if (packet.TypeId == 4) // Litteral
                {
                    string value = string.Empty;
                    char? firstBit;
                    do
                    {
                        string littleral = string.Join(string.Empty, queue.Dequeue(5));
                        firstBit = littleral.First();
                        value += littleral.Substring(1, 4);
                    }
                    while (firstBit != '0');

                    packet.LitleralValue = Convert.ToInt64(value, 2);
                }
                else
                {
                    char lengthTypeId = queue.Dequeue();
                    if (lengthTypeId == '0')
                    {
                        int length = Convert.ToInt32(string.Join(string.Empty, queue.Dequeue(15)), 2);
                        List<Packet> containedPackets = ProcessQueue(new Queue<char>(queue.Dequeue(length)));
                        packet.SubPackets.AddRange(containedPackets);
                    }
                    else
                    {
                        int count = Convert.ToInt32(string.Join(string.Empty, queue.Dequeue(11)), 2);
                        List<Packet> containedPackets = ProcessQueue(queue, count);
                        packet.SubPackets.AddRange(containedPackets);
                    }
                }

                packets.Add(packet);
            }

            return packets;
        }
    }

    class Packet
    {
        public int Version { get; private set; }
        public int TypeId { get; private set; }
        public long LitleralValue { get; set; }
        public string Binary { get; set; }
        public List<Packet> SubPackets { get; }

        public Packet(int version, int type)
        {
            Version = version;
            TypeId = type;
            SubPackets = new();
        }

        public long GetVersionNumberSumRecursive()
        {
            long value = Version;
            foreach (Packet packet in SubPackets)
            {
                value += packet.GetVersionNumberSumRecursive();
            }
            return value;
        }

        public long GetOperationValueRecursive()
        {
            return TypeId switch
            {
                0 => SubPackets.Sum(p => p.GetOperationValueRecursive()),
                1 => SubPackets.Product(p => p.GetOperationValueRecursive()),
                2 => SubPackets.Min(p => p.GetOperationValueRecursive()),
                3 => SubPackets.Max(p => p.GetOperationValueRecursive()),
                4 => LitleralValue,
                5 => SubPackets[0].GetOperationValueRecursive() > SubPackets[1].GetOperationValueRecursive() ? 1 : 0,
                6 => SubPackets[0].GetOperationValueRecursive() < SubPackets[1].GetOperationValueRecursive() ? 1 : 0,
                7 => SubPackets[0].GetOperationValueRecursive() == SubPackets[1].GetOperationValueRecursive() ? 1 : 0,
                _ => 0
            };
        }
    }

    static class Extensions
    {
        public static IEnumerable<T> Dequeue<T>(this Queue<T> queue, int count)
        {
            List<T> values = new();
            for (int i = 0; i < count; i++)
            {
                values.Add(queue.Dequeue());
            }
            return values;
        }

        public static long Product<T>(this IEnumerable<T> enumerable, Func<T, long> selector)
        {
            long value = 1;
            foreach (T item in enumerable)
            {
                value *= selector(item);
            }
            return value;
        }
    }
}
