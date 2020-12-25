using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventCode24
{
    class Program
    {
        static Dictionary<string, Vector> directions = new Dictionary<string, Vector>()
        {
            { "e", new Vector(1, -1, 0) },
            { "w", new Vector(-1, 1, 0) },
            { "nw", new Vector(0, 1, -1) },
            { "se", new Vector(0, -1, 1) },
            { "ne", new Vector(1, 0, -1) },
            { "sw", new Vector(-1, 0, 1) }
        };

        static Dictionary<int, int[]> blackTiles = new Dictionary<int, int[]>();
        static Dictionary<int, int[]> next = new Dictionary<int, int[]>();


        static void Main(string[] args)
        {
            var instructions = ReadFile("data").ToList();
            
            
            foreach (var instr in instructions)
            {
                var tile = new Vector();
                var d = "";
                for (int i = 0; i < instr.Length;i++)
                {
                    d += instr[i];
                    if (directions.ContainsKey(d))
                    {
                        tile += directions[d];
                        d = "";
                    }
                }

                if (blackTiles.ContainsKey(Id(tile)))
                    blackTiles.Remove(Id(tile));
                else
                    blackTiles.Add(Id(tile), tile.Coords);
            }

            Console.WriteLine(blackTiles.Count());


            for (int i= 1; i <= 100; i++)
            {
                FlipTiles();
                Console.WriteLine($"{i}: {blackTiles.Count()}");
            }
        }



        private static void FlipTiles()
        {
            next = new Dictionary<int, int[]>();

            foreach (var tile in blackTiles)
            {
                TestTile(tile.Value);

                var t = new Vector(tile.Value);
                foreach (var d in directions.Values)
                {
                    var x = t + d;
                    TestTile(x.Coords);
                }
            }

            blackTiles = next;
        }


        private static void TestTile(int[] v)
        {
            int sum = 0;
            Vector tile = new Vector(v);

            foreach (var d in directions.Values)
            {
                var x = tile + d;
                if (blackTiles.ContainsKey(Id(x)))
                    sum++;
            }

            if (blackTiles.ContainsKey(Id(v)))
            {
                if (0 < sum && sum <= 2 && !next.ContainsKey(Id(v)))
                    next.Add(Id(v), v);
            }
            else
            {
                if (sum == 2 && !next.ContainsKey(Id(v)))
                    next.Add(Id(v), v);
            }
        }


        private static IEnumerable<string> ReadFile(string name)
        {
            var file = new StreamReader(Path.Combine(Environment.CurrentDirectory, $"{name}.txt"));
            string line;
            while ((line = file.ReadLine()) != null)
                yield return line;
            file.Close();
        }

        private class Vector
        {
            public int X = 0;
            public int Y = 0;
            public int Z = 0;

            public Vector() { }

            public Vector(params int[] c)
            {
                X = c[0];
                Y = c[1];
                Z = c[2];
            }

            public int[] Coords => new int[] { X, Y, Z };

            public static Vector operator +(Vector a, Vector b)
            {
                return new Vector()
                {
                    X = a.X + b.X,
                    Y = a.Y + b.Y,
                    Z = a.Z + b.Z
                };
            }
        }

        private static Vector GetVector(int id)
        {
            return null;
        }

        private static int Id(Vector v)
        {
            return Id(v.X, v.Y, v.Z);
        }

        private static int Id(params int[] c)
        {
            return c[0] + c[1] * 1000 + c[2] * 1000000;
        }
    }
}
