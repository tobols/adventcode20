using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventCode16
{
    class Program
    {
        static void Main(string[] args)
        {
            var myTicket = new int[] { 89, 139, 79, 151, 97, 67, 71, 53, 59, 149, 127, 131, 103, 109, 137, 73, 101, 83, 61, 107 };

            List<Rule> rules = new List<Rule>();
            foreach (var rule in ReadFile("rules"))
            {
                Rule r = new Rule();
                var x = rule.Split(':');
                r.Name = x[0];
                var intervals = x[1].Split("or");

                foreach (var interval in intervals)
                {
                    var nums = interval.Trim().Split('-');
                    r.Intervals.Add(new int[] { int.Parse(nums[0]), int.Parse(nums[1]) });
                }

                rules.Add(r);
            }

            List<int[]> tickets = new List<int[]>();
            foreach (var ticket in ReadFile("tickets").ToArray())
                tickets.Add(ticket.Split(',').Select(n => int.Parse(n)).ToArray());

            var errorRate = 0;
            foreach (var ticket in tickets)
                for (int i = 0; i < ticket.Length; i++)
                    if (!rules.Any(r => r.IsValid(ticket[i])))
                        errorRate += ticket[i];

            Console.WriteLine(errorRate);


            tickets = tickets.Where(ticket => !ticket.Any(i => !rules.Any(r => r.IsValid(i)))).ToList();
            Dictionary<int, List<string>> matches = new Dictionary<int, List<string>>();
            for (int i = 0; i < tickets[0].Length; i++)
            {
                var y = rules.Where(r =>
                {
                    var x = tickets.Select(t => t[i]);
                    return x.All(r.IsValid);
                });

                matches.Add(i, y.Select(x => x.Name).ToList());
            }

            while (matches.Any(m => m.Value.Count() > 1))
                foreach (var match in matches)
                    if (match.Value.Count() == 1)
                        foreach(var m in matches.Where(mx => mx.Value.Contains(match.Value.First()) && mx.Key != match.Key))
                            m.Value.Remove(match.Value.First());

            Console.WriteLine(matches.Where(m => m.Value.First().StartsWith("departure")).Aggregate(1L, (l, m) => l * myTicket[m.Key]));
        }

        private static IEnumerable<string> ReadFile(string fileName)
        {
            var file = new StreamReader(Path.Combine(Environment.CurrentDirectory, $"{fileName}.txt"));
            string line;
            while ((line = file.ReadLine()) != null)
                yield return line;
            file.Close();
        }


        private class Rule
        {
            public string Name;
            public List<int[]> Intervals = new List<int[]>();

            public bool IsValid(int n)
            {
                foreach (var i in Intervals)
                    if (n >= i[0] && n <= i[1])
                        return true;
                return false;
            }
        }
    }
}
