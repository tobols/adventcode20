using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode10
{
    class Program
    {
        static void Main(string[] args)
        {
            var nums = ReadFile().OrderBy(n => n).ToArray();
            var xNum = new int[3];
            xNum[nums[0] - 1]++;
            xNum[2]++;

            for (int i = 0; i < nums.Length - 1; i++)
                xNum[(nums[i + 1] - nums[i]) - 1]++;

            var nodes = new Node[] { new Node { }, new Node { }, new Node { In = 1 } };
            for (int i = 0; i < nums.Length; i++)
            {
                var node = new Node { Val = nums[i] };

                for (int k = 0; k < 3; k++)
                    if (nodes[k].Val >= nums[i] - 3)
                        node.In += nodes[k].In;

                nodes[0] = nodes[1];
                nodes[1] = nodes[2];
                nodes[2] = node;
            }

            Console.WriteLine(xNum[0] * xNum[2]);
            Console.WriteLine(nodes[2].In);
        }

        private static IEnumerable<int> ReadFile()
        {
            var file = new System.IO.StreamReader(@"C:\Repos\temp\AdventCode\AdventCode10\data.txt");
            string line;
            while ((line = file.ReadLine()) != null)
                yield return Convert.ToInt32((line));
            file.Close();
        }

        private struct Node
        {
            public int Val;
            public long In;
        }
    }
}
