using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode11
{
    class Program
    {
        static char[][] seats;
        static char[][] last;

        static void Main(string[] args)
        {
            seats = ReadFile().ToArray();
            last = (char[][])seats.Clone();

            for (int i = 0; i < seats.Length; i++)
            {
                for (int j = 0; j < seats[0].Length; j++)
                {
                    if (seats[i][j] == 'L' || seats[i][j] == '#')
                        EvaluateSeat(i, j);
                }
            }
        }


        private static void EvaluateSeat(int x, int y)
        {
            var counter = 0;

            for (int i = (x - 1) >= 0 ? (x-1) : 0; i <= x + 1 && i < seats.Length; i++)
            {
                for (int j = (y - 1) >= 0 ? (y-1): 0; j <= y + 1 && j < seats[0].Length; j++)
                {
                    if (i == x && j == y)
                        continue;

                    if (seats[x][y] == 'L' && last[i][j] == '#')
                        counter++;
                    else if (last[i][j] == '#')
                        counter++;
                }
            }

            if (seats[x][y] == 'L' && counter == 0)
                seats[x][y] = '#';

            else if (seats[x][y] == '#' && counter >= 4)
                seats[x][y] = 'L';
        }


        private static IEnumerable<char[]> ReadFile()
        {
            var file = new System.IO.StreamReader(@"C:\Repos\temp\AdventCode\AdventCode11\data.txt");
            string line;
            while ((line = file.ReadLine()) != null)
                yield return line.ToCharArray();
            file.Close();
        }
    }
}
