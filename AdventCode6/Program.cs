using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode6
{
    class Program
    {
        private static string alphabet = "abcdefghijklmnopqrstuvwxyz";

        static void Main(string[] args)
        {
            var groups = ReadFile();
            Console.WriteLine(groups.Sum(g => HammingWeight(g.Aggregate(0, (current, row) => current | row))));
            Console.WriteLine(groups.Sum(g => HammingWeight(g.Aggregate(67108863, (current, row) => current & row))));
        }

        private static int HammingWeight(int v)
        {
            v -= (v >> 1) & 1431655765;
            v = (v & 858993459) + ((v >> 2) & 858993459);
            return (((v + (v >> 4)) & 252645135) * 16843009) >> 24;
        }

        private static List<List<int>> ReadFile()
        {
            var file = new System.IO.StreamReader(@"C:\Repos\temp\AdventCode\AdventCode6\data.txt");
            string line;

            List<List<int>> groups = new List<List<int>>();
            List<int> group = new List<int>();

            while ((line = file.ReadLine()) != null)
            {
                if (line == string.Empty)
                {
                    groups.Add(group);
                    group = new List<int>();
                    continue;
                }

                var row = new int[26];
                foreach (char c in line)
                    row[alphabet.IndexOf(c)] = 1;

                group.Add(Convert.ToInt32(string.Join("", row), 2));
            }

            groups.Add(group);
            file.Close();
            return groups;
        }
    }
}
