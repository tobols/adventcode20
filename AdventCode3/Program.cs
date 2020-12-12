using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode3
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int[]> valuePairs = new List<int[]>
            {
                new[] {1, 1},
                new[] {3, 1},
                new[] {5, 1},
                new[] {7, 1},
                new[] {1, 2}
            };

            var sum = valuePairs.Aggregate(1L, (result, vp) => result * CountTrees(vp[0], vp[1]));
            Console.WriteLine($"This is a sum: {sum}");
        }

        private static long CountTrees(int right, int down)
        {
            var trees = 0L;
            var counter = 0;
            var lineCount = 0;
            string line;
            var file = new System.IO.StreamReader(@"C:\Repos\temp\AdventCode\AdventCode3\data.txt");

            while ((line = file.ReadLine()) != null)
            {
                if (lineCount++ % down != 0)
                    continue;

                if (line[counter % line.Length] == '#')
                    trees++;
                counter += right;
            }

            file.Close();
            Console.WriteLine($"For right {right} and down {down}, there are {trees} trees");

            return trees;
        }
    }
}
