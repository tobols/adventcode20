using System;
using System.Linq;

namespace AdventCode2
{
    class Program
    {
        static void Main(string[] args)
        {
            int counter = 0;
            int counter2 = 0;
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(@"C:\Repos\temp\AdventCode\AdventCode2\data1.txt");

            while ((line = file.ReadLine()) != null)
            {
                var components = line.Split(" ");
                var nums = components[0].Split("-");
                var ch = components[1][0];
                var start = int.Parse(nums[0]);
                var end = int.Parse(nums[1]);
                var sum = components[2].Count(c => c == ch);

                if (sum >= start && sum <= end)
                    counter++;

                if (components[2][start-1] == ch ^ components[2][end-1] == ch)
                    counter2++;
            }

            file.Close();
            System.Console.WriteLine("There were {0} correct passwords.", counter);
            System.Console.WriteLine("There were {0} correct weird passwords.", counter2);
            System.Console.ReadLine();
        }


        private static int TriangleRowSum(int row, int dist, int start)
            => (int)((Math.Pow(row, 3) + row) * dist / 2 + row * (start - dist));

        //     1
        //    3 5
        //   7 9 11
        // 13 15 17 19
        //
        //      2
        //     4 6
        //   8 10 12
        // 14 16 18 20
    }
}
