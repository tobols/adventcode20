using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode17
{
    class Program
    {
        private static Dictionary<string, int[]> currentCubes;
        private static Dictionary<string, int[]> nextCubes;
        private static List<string> testedCubes;


        static void Main(string[] args)
        {
            currentCubes = GetInitialState(3);
            testedCubes = new List<string>();
            nextCubes = new Dictionary<string, int[]>();

            for (int i = 0; i < 6; i++)
            {
                UpdateCubes(3);
                Console.WriteLine(currentCubes.Count);
            }

            Console.WriteLine();

            currentCubes = GetInitialState(4);
            testedCubes = new List<string>();
            nextCubes = new Dictionary<string, int[]>();

            for (int i = 0; i < 6; i++)
            {
                UpdateCubes(4);
                Console.WriteLine(currentCubes.Count);
            }
        }


        private static void UpdateCubes(int dimensions)
        {
            foreach (var cube in currentCubes)
            {
                var c = cube.Value;
                if (dimensions == 3) TestCube2(c[0], c[1], c[2]);
                if (dimensions == 4) TestCube3(c[0], c[1], c[2], c[3]);
            }

            currentCubes = nextCubes;
            nextCubes = new Dictionary<string, int[]>();
            testedCubes = new List<string>();
        }


        private static void TestCube2(int x, int y, int z)
        {
            var hash = GetIdentifier(x, y, z);
            if (testedCubes.Contains(hash))
                return;
            testedCubes.Add(hash);
            var active = currentCubes.ContainsKey(hash);
            var sumActive = 0;

            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    for (int k = z - 1; k <= z + 1; k++)
                    {
                        if (GetIdentifier(i, j, k) != hash && currentCubes.ContainsKey(GetIdentifier(i, j, k)))
                            sumActive++;

                        if (active)
                            TestCube2(i, j, k);
                    }
                }
            }

            if (active && sumActive >= 2 && sumActive <= 3)
                nextCubes.Add(hash, new int[] { x, y, z });
            else if (!active && sumActive == 3)
                nextCubes.Add(hash, new int[] { x, y, z });
        }


        private static void TestCube3(int x, int y, int z, int w)
        {
            var hash = GetIdentifier(x, y, z, w);
            if (testedCubes.Contains(hash))
                return;
            testedCubes.Add(hash);
            var active = currentCubes.ContainsKey(hash);
            var sumActive = 0;

            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    for (int k = z - 1; k <= z + 1; k++)
                    {
                        for (int l = w - 1; l <= w + 1; l++)
                        {
                            if (GetIdentifier(i, j, k, l) != hash && currentCubes.ContainsKey(GetIdentifier(i, j, k, l)))
                                sumActive++;

                            if (active)
                                TestCube3(i, j, k, l);
                        }
                    }
                }
            }

            if (active && sumActive >= 2 && sumActive <= 3)
                nextCubes.Add(hash, new int[] { x, y, z, w });
            else if (!active && sumActive == 3)
                nextCubes.Add(hash, new int[] { x, y, z, w });
        }


        private static Dictionary<string, int[]> GetInitialState(int dimensions)
        {
            //string[] s = new string[]
            //{
            //    ".#.",
            //    "..#",
            //    "###",
            //};

            string[] s = new string[]
            {
                "####.#..",
                ".......#",
                "#..#####",
                ".....##.",
                "##...###",
                "#..#.#.#",
                ".##...#.",
                "#...##.."
            };

            Dictionary<string, int[]> cubes = new Dictionary<string, int[]>();

            for (int i = 0; i < s.Length; i++)
                for (int j = 0; j < s[i].Length; j++)
                    if (s[i][j] == '#')
                    {
                        var h = new int[dimensions];
                        h[0] = i;
                        h[1] = j;

                        cubes.Add(GetIdentifier(h), h);
                    }

            return cubes;
        }

        private static string GetIdentifier(params int[] vals)
        {
            return vals.Aggregate(":", (r, v) => $"{r}:{v}");
        }
    }
}

