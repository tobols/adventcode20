using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventCode19
{
    class Program
    {
        static Dictionary<int, int[][]> rules;

        static void Main(string[] args)
        {
            rules = GetRules();
            var data = ReadFile("data").ToList();

            var sum = 0;
            foreach (var str in data)
                if (TestRule2(0, str).Any(v => v == str.Length))
                    sum++;

            Console.WriteLine(sum);


            rules[8] = new int[][] { new int[] { 42 }, new int[] { 42, 8 } };
            rules[11] = new int[][] { new int[] { 42, 31 }, new int[] { 42, 11, 31 } };


            sum = 0;
            foreach (var str in data)
                if (TestRule2(0, str).Any(v => v == str.Length))
                    sum++;

            Console.WriteLine(sum);
        }


        private static List<int> TestRule2(int id, string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return arr(-10000000);

            var rule = rules[id];
            if (rule[0][0] == -1)
                return str[0] == 'a' ? arr(1): arr(-10000000);
            if (rule[0][0] == -2)
                return str[0] == 'b' ? arr(1) : arr(-10000000);

            List<int> paths = new List<int>();
            List<int> ruleSetPaths;
            List<int> discoveredPaths;

            for (int i = 0; i < rule.Length; i++)
            {
                ruleSetPaths = new List<int> { 0 };
                for (int j = 0; j < rule[i].Length; j++)
                {
                    discoveredPaths = new List<int>();

                    for (int k = 0; k < ruleSetPaths.Count(); k++)
                    {
                        var ps = TestRule2(rule[i][j], str.Substring(ruleSetPaths[k]));
                        discoveredPaths.AddRange(ps.Select(p => ruleSetPaths[k] + p));
                    }

                    ruleSetPaths = discoveredPaths.Where(p => p > 0 && p <= str.Length).ToList();
                    if (ruleSetPaths.Count() == 0)
                        break;
                }

                paths.AddRange(ruleSetPaths);
            }

            return paths;
        }


        private static List<int> arr(params int[] a)
        {
            return a.ToList();
        }


        private static Dictionary<int, int[][]> GetRules()
        {
            var rules = new Dictionary<int, int[][]>();
            var lines = ReadFile("rules");

            foreach (var line in lines)
            {
                var l = line.Split(':');

                var y = l[1];
                var x = l[1].Trim().Split(" ");

                if (l[1].Contains('a'))
                    rules.Add(int.Parse(l[0]), new int[][] { new int[] { -1 } });
                else if (l[1].Contains('b'))
                    rules.Add(int.Parse(l[0]), new int[][] { new int[] { -2 } });
                else
                    rules.Add(int.Parse(l[0]), l[1].Trim().Split('|').Select(s => s.Trim().Split(" ").Select(int.Parse).ToArray()).ToArray());
            }

            return rules;
        }


        private static IEnumerable<string> ReadFile(string name)
        {
            var file = new StreamReader(Path.Combine(Environment.CurrentDirectory, $"{name}.txt"));
            string line;
            while ((line = file.ReadLine()) != null)
                yield return line;
            file.Close();
        }
    }
}
