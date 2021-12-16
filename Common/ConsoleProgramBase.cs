using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AoCCommon
{
    public class ConsoleProgramBase
    {
        public static void Run(string[] args)
        {
            if (args.Any())
            {
                if (Assembly.GetEntryAssembly().GetTypes().FirstOrDefault(x => x.Name == args[0]) is Type type)
                {
                    IDay day = Activator.CreateInstance(type) as IDay;
                    day.GetResults();
                }
            }
            else
            {
                foreach (Type type in Assembly.GetEntryAssembly().GetTypes().Where(x => x.Name.StartsWith("Day")).OrderBy(x => x.Name))
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
