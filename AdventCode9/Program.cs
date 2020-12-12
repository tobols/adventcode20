using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode9
{
    class Program
    {
        static void Main(string[] args)
        {
            var nums = ReadFile().ToArray();
            var weakness = 0L;

            for (int i = 25; i < nums.Length; i++)
            {
                var found = false;
                for (int j = 1; j <= 25; j++)
                {
                    for (int k = 1; k <= 25; k++)
                    {
                        if (k == j) continue;

                        if (nums[i-j] + nums[i-k] == nums[i])
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found) break;
                }

                if (!found)
                {
                    weakness = nums[i];
                    Console.WriteLine($"Weakness: {weakness}");
                    break;
                }
            }

            for (int i = 0; i < nums.Length; i++)
            {
                var tempSum = 0L;
                var n = i;

                do { tempSum += nums[n++]; }
                while (tempSum < weakness);

                if (tempSum == weakness)
                {
                    var range = nums[i..(n - 1)].OrderBy(l => l);
                    Console.WriteLine($"Encryption weakness: {range.Min() + range.Max()}");
                    break;
                }
            }
        }

        private static IEnumerable<long> ReadFile()
        {
            var file = new System.IO.StreamReader(@"C:\Repos\temp\AdventCode\AdventCode9\data.txt");
            string line;
            while ((line = file.ReadLine()) != null)
                yield return Convert.ToInt64((line));
            file.Close();
        }
    }
}
