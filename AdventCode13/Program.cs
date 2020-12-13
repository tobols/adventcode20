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
            var n = 0;

            for (int i = 0; i < buses.Length - 1; i++)
            {
                if (buses[i] == "x") continue;

                if (n == i)
                    for (int j = i + 1; j < buses.Length; j++)
                        if (buses[j] != "x")
                        {
                            n = j;
                            break;
                        }

                p *= int.Parse(buses[i]);
                while ((s + n) % int.Parse(buses[n]) != 0)
                    s += p;
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
