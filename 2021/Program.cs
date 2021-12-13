using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Any())
            {
                if (Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(x => x.Name == args[0]) is Type type)
                {
                    IDay day = Activator.CreateInstance(type) as IDay;
                    day.GetResults();
                }
            }
            else
            {
                foreach (Type type in Assembly.GetExecutingAssembly().GetTypes().Where(x => x.Name.StartsWith("Day")).OrderBy(x => x.Name))
                {
                    IDay day = Activator.CreateInstance(type) as IDay;
                    Console.WriteLine(type.Name);

                    day.GetResults();

                    Console.WriteLine();
                }
                Console.ReadLine();
            }
        }
    }
}
