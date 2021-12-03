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
        }
    }
}
