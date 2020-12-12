using System;
using System.Collections.Generic;
using System.IO;

namespace AdventCode12
{
    class Program
    {
        static void Main(string[] args)
        {
            var instructions = ReadFile();
            var ship = new Ship();
            var ship2 = new Ship2();

            foreach (var instr in instructions)
            {
                ship.RunInstruction(instr[0], int.Parse(instr.Substring(1)));
                ship2.RunInstruction(instr[0], int.Parse(instr.Substring(1)));
            }

            Console.WriteLine($"Distance: {ship.GetDistance()}");
            Console.WriteLine($"Distance: {ship2.GetDistance()}");
        }

        private static IEnumerable<string> ReadFile()
        {
            var file = new StreamReader(Path.Combine(Environment.CurrentDirectory, "data.txt"));
            string line;
            while ((line = file.ReadLine()) != null)
                yield return line;
            file.Close();
        }

        private class Ship2
        {
            int[] position = new int[] { 0, 0 };
            int[] waypoint = new int[] { 10, 1 };

            public void RunInstruction(char i, int v)
            {
                switch (i)
                {
                    case 'E': MoveWayPoint(v, 0); break;
                    case 'N': MoveWayPoint(v, 90); break;
                    case 'W': MoveWayPoint(v, 180); break;
                    case 'S': MoveWayPoint(v, 270); break;
                    case 'F': MoveShip(v); break;
                    case 'L': Rotate(1, v); break;
                    case 'R': Rotate(-1, v); break;
                }
            }

            public int GetDistance()
            {
                return Math.Abs(position[0]) + Math.Abs(position[1]);
            }

            private void Rotate(int direction, int degrees)
            {
                var a = Math.PI / 180 * direction * degrees + Math.Atan2(waypoint[1], waypoint[0]);
                var hyp = Math.Sqrt(Math.Pow(waypoint[0], 2) + Math.Pow(waypoint[1], 2));
                waypoint[0] = (int)Math.Round(Math.Cos(a) * hyp);
                waypoint[1] = (int)Math.Round(Math.Sin(a) * hyp);
            }

            private void MoveWayPoint(int steps, int degrees)
            {
                waypoint[0] += (int)(Math.Cos(Math.PI / 180 * degrees)) * steps;
                waypoint[1] += (int)(Math.Sin(Math.PI / 180 * degrees)) * steps;
            }

            private void MoveShip(int steps)
            {
                position[0] += waypoint[0] * steps;
                position[1] += waypoint[1] * steps;
            }
        }

        private class Ship
        {
            int orientation = 0;
            int[] position = new int[] { 0, 0 };

            public void RunInstruction(char i, int v)
            {
                switch (i)
                {
                    case 'E': Move(v, 0); break;
                    case 'N': Move(v, 90); break;
                    case 'W': Move(v, 180); break;
                    case 'S': Move(v, 270); break;
                    case 'F': Move(v, orientation); break;
                    case 'L': Turn(1, v); break;
                    case 'R': Turn(-1, v); break;
                }
            }

            public int GetDistance()
            {
                return Math.Abs(position[0]) + Math.Abs(position[1]);
            }

            private void Turn(int direction, int degrees)
            {
                orientation = Mod(orientation + direction * degrees, 360);
            }

            private void Move(int steps, int degrees)
            {
                position[0] += (int)(Math.Cos(Math.PI / 180 * degrees)) * steps;
                position[1] += (int)(Math.Sin(Math.PI / 180 * degrees)) * steps;
            }

            private int Mod(int a, int b)
            {
                int x = a % b;
                return x * b < 0 ? x + b : x;
            }
        }
    }
}
