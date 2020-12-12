using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode5
{
    class Program
    {
        static void Main(string[] args)
        {
            var nums = ReadFile();
            var seats = nums.Select(n => (n >> 3) * 8 + (n & 7)).OrderBy(n => n).ToArray();

            Console.WriteLine(seats.Last());
            for (int i = 0; i < seats.Length - 1; i++)
                if (seats[i]+2 == seats[i+1])
                    Console.WriteLine(seats[i]+1);
        }

        private static IEnumerable<int> ReadFile()
        {
            var file = new System.IO.StreamReader(@"C:\Repos\temp\AdventCode\AdventCode5\data.txt");
            string line;
            while ((line = file.ReadLine()) != null)
                yield return Convert.ToInt32(line.Replace('F', '0').Replace('L', '0').Replace('B', '1').Replace('R', '1'), 2);
            file.Close();
        }
    }
}
