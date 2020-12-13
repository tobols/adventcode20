using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventCode13
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = ReadFile().ToArray();
            var timestamp = int.Parse(data[0]);
            var buses = data[1].Split(',');
            var nextBus = new int[] { 0, timestamp };

            foreach (var bus in buses.Where(b => b != "x"))
            {
                var b = int.Parse(bus);
                var a = timestamp % b;

                if (b - a < nextBus[1])
                {
                    nextBus[0] = b;
                    nextBus[1] = b - a;
                }
            }

            Console.WriteLine(nextBus[0] * nextBus[1]);


            var s = 0L;
            var p = 1L;

            for (int i = 0; i < buses.Length - 1; i++)
            {
                if (buses[i] == "x") continue;
                while ((s + i) % int.Parse(buses[i]) != 0)
                    s += p;
                p *= int.Parse(buses[i]);
            }

            Console.WriteLine(s);
        }


        private static IEnumerable<string> ReadFile()
        {
            var file = new StreamReader(Path.Combine(Environment.CurrentDirectory, "data.txt"));
            string line;
            while ((line = file.ReadLine()) != null)
                yield return line;
            file.Close();
        }
    }
}
