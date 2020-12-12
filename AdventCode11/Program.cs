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
            last = DeepClone(seats);
            var iterations = 0;

            while (UpdateSeats()) { Console.WriteLine($"Iterations: {++iterations}"); }
            Console.WriteLine($"Occupied seats: {CountOccupied()}");
        }

        private static bool UpdateSeats()
        {
            var hasChanges = false;

            for (int i = 0; i < seats.Length; i++)
                for (int j = 0; j < seats[0].Length; j++)
                    if (seats[i][j] == 'L' || seats[i][j] == '#')
                        hasChanges = EvaluateSeat(i, j) || hasChanges;

            if (hasChanges)
                last = DeepClone(seats);

            return hasChanges;
        }

        private static bool EvaluateSeat(int x, int y)
        {
            var counter = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;

                    if (seats[x][y] == 'L' && Look(x, y, i, j, seats.Length))
                        counter++;
                    else if (seats[x][y] == '#' && Look(x, y, i, j, seats.Length))
                        counter++;
                }
            }

            if (seats[x][y] == 'L' && counter == 0)
                seats[x][y] = '#';
            else if (seats[x][y] == '#' && counter >= 5)
                seats[x][y] = 'L';
            else
                return false;

            return true;
        }

        private static bool Look(int x, int y, int dx, int dy, int range = 1)
        {
            var c = 1;

            while(x + dx * c >= 0 && y + dy * c >= 0 && x + dx * c < seats.Length && y + dy * c < seats[0].Length && c <= range)
            {
                if (last[x + dx * c][y + dy * c] == '#') return true;
                if (last[x + dx * c][y + dy * c] == 'L') return false;
                c++;
            }

            return false;
        }

        private static char[][] DeepClone(char[][] arr)
        {
            char[][] copyArr = new char[arr.Length][];

            for (int i = 0; i < arr.Length; i++)
            {
                copyArr[i] = new char[arr[i].Length];
                for (int j = 0; j < arr[i].Length; j++)
                {
                    copyArr[i][j] = arr[i][j];
                }
            }

            return copyArr;
        }

        private static int CountOccupied()
        {
            return seats.SelectMany(s => s).Where(c => c == '#').Count();
        }


        private static IEnumerable<char[]> ReadFile()
        {
            var file = new System.IO.StreamReader(@"D:\Repos\temp\adventcode20\AdventCode11\data.txt");
            string line;
            while ((line = file.ReadLine()) != null)
                yield return line.ToCharArray();
            file.Close();
        }
    }
}
