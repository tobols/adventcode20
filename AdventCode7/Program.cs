using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode7
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = GetAllLines().ToArray();
            var nodes = CreateNodes(lines);
            var bags = GetContainingBags("shiny gold", nodes).Distinct();
            var sum = GetRequiredNumberOfBags("shiny gold", nodes);
            Console.WriteLine(bags.Count());
            Console.WriteLine(sum);
        }

        private static Dictionary<string, Dictionary<string, int>> CreateNodes(string[] lines)
        {
            var nodes = new Dictionary<string, Dictionary<string, int>>();

            for (int i = 0; i < lines.Length; i++)
                nodes.Add(lines[i][0..lines[i].IndexOf(" bag")], null);

            for (int i = 0; i < lines.Length; i++)
            {
                var conns = lines[i].Substring(lines[i].IndexOf("contain") + 8);
                if (conns.StartsWith("no"))
                    continue;

                nodes[lines[i][0..lines[i].IndexOf(" bag")]] = conns.Split(',').Select(s => s.Trim()).ToDictionary(cKey => cKey[(cKey.IndexOf(" ")+1)..cKey.IndexOf(" bag")], cVal => int.Parse(cVal[0..cVal.IndexOf(" ")]));
            }

            return nodes;
        }

        private static List<string> GetContainingBags(string node, Dictionary<string, Dictionary<string, int>> nodes)
        {
            var subNodes = nodes.Where(n => n.Value != null && n.Value.ContainsKey(node)).ToDictionary(kv => kv.Key, kv => kv.Value);
            var bags = subNodes.Keys.Select(k => k).ToList();

            foreach (var n in subNodes)
                bags.AddRange(GetContainingBags(n.Key, nodes));

            return bags;
        }

        private static int GetRequiredNumberOfBags(string node, Dictionary<string, Dictionary<string, int>> nodes)
        {
            var nod = nodes[node];
            if (nod == null)
                return 0;

            var sum = nod.Values.Aggregate(0, (r, c) => r + c);
            foreach (var key in nod.Keys)
                sum += nod[key] * GetRequiredNumberOfBags(key, nodes);
            
            return sum;
        }

        private static IEnumerable<string> GetAllLines()
        {
            var file = new System.IO.StreamReader(@"C:\Repos\temp\AdventCode\AdventCode7\data.txt");
            string line;
            while ((line = file.ReadLine()) != null)
                yield return line;
            file.Close();
        }
    }
}
