using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventCode22
{
    class Program
    {

        static void Main(string[] args)
        {
            RegularCombat();
            RecursiveCombat();
        }


        private static void RecursiveCombat()
        {
            Queue<int> p1 = new Queue<int>();
            Queue<int> p2 = new Queue<int>();
            string player = "";

            foreach (var line in ReadFile("data"))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.StartsWith("Player"))
                {
                    player = line;
                    continue;
                }

                if (player == "Player 1:")
                    p1.Enqueue(int.Parse(line));

                if (player == "Player 2:")
                    p2.Enqueue(int.Parse(line));
            }

            var winner = SubGame(p1, p2);
            var s = GetScore(winner == 0 || winner == 3 ? p1 : p2);
            Console.WriteLine(s);
        }


        private static int SubGame(Queue<int> p1, Queue<int> p2, int level = 0)
        {
            List<string> confs = new List<string>();
            while (p1.Count() > 0 && p2.Count() > 0)
            {
                var c = GetConf(p1, p2);
                if (confs.Contains(GetConf(p1, p2)))
                    return 0;
                confs.Add(c);

                int winner;
                var c1 = p1.Dequeue();
                var c2 = p2.Dequeue();

                if (p1.Count() >= c1 && p2.Count() >= c2)
                {
                    winner = SubGame(new Queue<int>(p1.Take(c1)), new Queue<int>(p2.Take(c2)), level++);
                }
                else
                    winner = c1 > c2 ? 0 : 1;

                if (winner == 0)
                {
                    p1.Enqueue(c1);
                    p1.Enqueue(c2);
                }
                else
                {
                    p2.Enqueue(c2);
                    p2.Enqueue(c1);
                }
            }

            return p1.Count() > 0 ? 0 : 1;
        }
        
        
        private static string GetConf(Queue<int> p1, Queue<int> p2)
        {
            return string.Join(',', p1.ToArray()) + ":" + string.Join(',', p2.ToArray());
        }


        private static void RegularCombat()
        {
            Queue<int> p1 = new Queue<int>();
            Queue<int> p2 = new Queue<int>();
            string player = "";

            foreach (var line in ReadFile("data"))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.StartsWith("Player"))
                {
                    player = line;
                    continue;
                }

                if (player == "Player 1:")
                    p1.Enqueue(int.Parse(line));

                if (player == "Player 2:")
                    p2.Enqueue(int.Parse(line));
            }

            int c1, c2;
            while (p1.Count() > 0 && p2.Count() > 0)
            {
                c1 = p1.Dequeue();
                c2 = p2.Dequeue();

                if (c1 > c2)
                {
                    p1.Enqueue(c1);
                    p1.Enqueue(c2);
                }

                if (c2 > c1)
                {
                    p2.Enqueue(c2);
                    p2.Enqueue(c1);
                }
            }

            var winner = p1.Count() > p2.Count() ? p1 : p2;
            Console.WriteLine(GetScore(winner));
        }


        private static int GetScore(Queue<int> q)
        {
            var c = new Queue<int>(q);
            int score = 0;
            int cards = c.Count();
            for (int i = cards; i > 0; i--)
                score += c.Dequeue() * i;
            return score;
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
