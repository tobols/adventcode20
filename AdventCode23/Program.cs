using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode23
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] data = { 1, 9, 8, 7, 5, 3, 4, 6, 2 };

            var cups = CrabShuffle(data.ToList(), 100);

            var p = 1;
            var result = new List<int>();
            for (int i = 0; i < cups.Count; i++)
            {
                result.Add(cups[p]);
                p = cups[p];
            }

            Console.WriteLine(string.Join("", result.ToArray()));


            var c = data.ToList();
            var x = c.Max();
            while (c.Count < 1000000)
                c.Add(++x);

            cups = CrabShuffle(c, 10000000);

            var a = (long)cups[1];
            var b = (long)cups[(int)a];
            long r = a * b;

            Console.WriteLine(r);
        }


        private static Dictionary<int, int> CrabShuffle(List<int> list, int moves)
        {
            var cups = new Dictionary<int, int>();
            for (int i = 0; i < list.Count - 1; i++)
                cups.Add(list[i], list[i + 1]);
            cups.Add(list.Last(), list.First());

            var pointer = cups.First().Key;
            var max = list.Max();

            for (int i = 1; i <= moves; i++)
            {
                var a = cups[pointer];
                var b = cups[a];
                var c = cups[b];
                cups[pointer] = cups[c];

                var val = pointer;
                do
                {
                    val--;
                    if (val <= 0)
                        val = max;
                } while (val == a || val == b || val == c);

                cups[c] = cups[val];
                cups[val] = a;
                pointer = cups[pointer];
            }

            return cups;
        }
    }
}
