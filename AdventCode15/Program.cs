using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode15
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] startNums = new int[] { 0, 13, 1, 16, 6, 17 };
            Console.WriteLine(GetSpokenNum(2020, startNums));
            Console.WriteLine(GetSpokenNum(30000000, startNums));
        }

        private static int GetSpokenNum(int n, int[] startNums)
        {
            if (startNums.Any(n => startNums.Count(x => x == n) > 1))
                throw new ArgumentException("Write better code!");

            Dictionary<int, int> nums = new Dictionary<int, int>();
            for (int i = 0; i < startNums.Length - 1; i++)
                nums.Add(startNums[i], i);

            var lastNum = startNums[startNums.Length - 1];

            for (int i = nums.Count(); i < n - 1; i++)
            {
                if (nums.ContainsKey(lastNum))
                {
                    var x = i - nums[lastNum];
                    nums[lastNum] = i;
                    lastNum = x;
                }
                else
                {
                    nums.Add(lastNum, i);
                    lastNum = 0;
                }
            }

            return lastNum;
        }
    }
}
