using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventCode20
{
    class Program
    {
        private enum Orientation
        {
            North,
            East,
            South,
            West,
            None
        }


        static void Main(string[] args)
        {
            var tiles = LoadTiles();
            Dictionary<int, int[][]> cornerTiles = new Dictionary<int, int[][]>();
            
            foreach (var tile in tiles)
            {
                var singleSides = tile.Sides.Where(side => tiles.Count(t => t.MatchSide(side)) == 1);

                if (singleSides.Count() == 2)
                    cornerTiles.Add(tile.Id, singleSides.ToArray());
                if (cornerTiles.Count() >= 4)
                    break;
            }

            Console.WriteLine(tiles.Where(t => cornerTiles.ContainsKey(t.Id)).Aggregate(1L, (r, t) => r * t.Id));

            var image = AssembleTiles(tiles, cornerTiles);
            var imageData = ConvertToDataFormat(image);
            var assembledTile = new Tile(0, imageData, false);

            var result = FindMonsters(assembledTile);
            var maskedData = ConvertToDataFormat(result.maskedData.ToList());
            var maskedImage = new Tile(0, maskedData, false);

            PrintImage(assembledTile.Data);
            PrintImage(maskedImage.Data);

            Console.WriteLine(result.maskedData.Sum(m => m.Sum()));
        }


        private static List<int[]> AssembleTiles(List<Tile> tiles, Dictionary<int, int[][]> corners)
        {
            List<List<Tile>> assembledTiles = new List<List<Tile>>() { new List<Tile>() };
            Tile tile = null;
            Tile next;
            foreach(var corner in corners)
            {
                var t = tiles.FirstOrDefault(t => t.Id == corner.Key);
                var o = corner.Value.Select(c => t.GetOrientation(c)).ToList();
                if (o.Contains(Orientation.North) && o.Contains(Orientation.West))
                    tile = t;
            }

            while (tiles.Count > 0)
            {
                tiles.Remove(tile);
                assembledTiles.Last().Add(tile);
                next = tiles.FirstOrDefault(t => t.MatchSide(tile.GetSide(Orientation.East)));

                if (next == null)
                {
                    tile = assembledTiles.Last()[0];
                    next = tiles.FirstOrDefault(t => t.MatchSide(tile.GetSide(Orientation.South)));

                    if (next == null)
                        break;

                    while(next.GetOrientation(tile.GetSide(Orientation.South)) != Orientation.North)
                        next.RotateClockwise();

                    if (next.IsReverse(Orientation.North, tile.GetSide(Orientation.South)))
                        next.Flip(false);

                    assembledTiles.Add(new List<Tile>());
                }
                else
                {
                    while (next.GetOrientation(tile.GetSide(Orientation.East)) != Orientation.West)
                        next.RotateClockwise();

                    if (next.IsReverse(Orientation.West, tile.GetSide(Orientation.East)))
                        next.Flip(true);
                }

                tile = next;
            }


            var image = new List<int[]>();

            foreach (var row in assembledTiles)
                for (int i = 0; i < row[0].Data.Length; i++)
                    image.Add(row.SelectMany(t => t.Data[i]).ToArray());

            return image;
        }


        private static List<Tile> LoadTiles()
        {
            int id = 0;
            List<char[]> data = new List<char[]>();
            List<Tile> tiles = new List<Tile>();

            foreach (var line in ReadFile("data"))
            {
                if (line.StartsWith("Tile"))
                    id = int.Parse(line.Substring(line.IndexOf(" ") + 1, 4));
                else if (string.IsNullOrWhiteSpace(line))
                {
                    tiles.Add(new Tile(id, data.ToArray()));
                    data = new List<char[]>();
                }
                else
                    data.Add(line.ToCharArray());
            }

            tiles.Add(new Tile(id, data.ToArray()));
            return tiles;
        }


        private static IEnumerable<string> ReadFile(string name)
        {
            var file = new StreamReader(Path.Combine(Environment.CurrentDirectory, $"{name}.txt"));
            string line;
            while ((line = file.ReadLine()) != null)
                yield return line;
            file.Close();
        }


        private static void PrintImage(int[][] image)
        {
            Console.WriteLine("Image:");
            Console.WriteLine();

            foreach(var row in image)
            {
                for (int i = 0; i < row.Length; i++)
                    Console.Write(row[i] == 0 ? '.' : row[i] == 1 ? '#' : 'O');
                Console.WriteLine();
            }

            Console.WriteLine();
        }


        private static void PrintSides(Tile t)
        {
            Console.WriteLine("ID: " + t.Id);
            foreach(var e in t.Sides)
                Console.WriteLine($"{string.Join("", e)}");
            Console.WriteLine();
        }


        private static char[][] ConvertToDataFormat(List<int[]> image)
        {
            return image.Select(r => r.Select(i => i == 0 ? '.' : i == 1 ? '#' : 'O').ToArray()).ToArray();
        }


        private static MaskResult FindMonsters(Tile tile)
        {
            MaskResult result = new MaskResult();

            for (int i = 0; i < 4 && result.matches == 0; i++)
            {
                result = TestMask(tile.Data);

                if (result.matches == 0)
                {
                    tile.Flip(true);
                    result = TestMask(tile.Data);
                }

                if (result.matches == 0)
                {
                    tile.Flip(false);
                    result = TestMask(tile.Data);
                }

                if (result.matches == 0)
                {
                    tile.Flip(true);
                    result = TestMask(tile.Data);
                }

                if (result.matches == 0)
                {
                    tile.Flip(false);
                    tile.RotateClockwise();
                }
            }

            return result;
        }


        private static MaskResult TestMask(int[][] data)
        {
            var mask = GetMonsterMask();
            var maskH = mask.Length;
            var maskW = 20;
            var c = 0;
            bool found;

            var maskedData = new int[data.Length][];
            for (int i = 0; i < data.Length; i++)
                maskedData[i] = CopyArr(data[i]);

            for (int i = 0; i < data.Length - maskH + 1; i++)
            {
                for (var j = 0; j < data[0].Length - maskW + 1; j++)
                {
                    found = true;

                    for (var k = 0; k < maskH; k++)
                    {
                        var x = Convert.ToInt64(string.Join("", maskedData[i+k][j..(j+maskW)]), 2);

                        if ((x & mask[k]) != mask[k])
                        {
                            found = false;
                            break;
                        }
                    }

                    if (found)
                    {
                        for (var k = 0; k < maskH; k++)
                        {
                            var x = Convert.ToInt64(string.Join("", data[i + k][j..(j + maskW)]), 2);
                            var a = Convert.ToString(x & (~mask[k]), 2).ToCharArray();

                            var q = new char[maskW - a.Length];
                            for (int l = 0; l < q.Length; l++)
                                q[l] = '0';
                            a = q.Concat(a).ToArray();
                            
                            for (var n = 0; n < a.Length; n++)
                                maskedData[i + k][j + maskW - n - 1] = int.Parse(a[a.Length - n - 1].ToString());
                        }

                        c++;
                        j += maskW - 1;
                    }
                }
            }

            var result = new MaskResult
            {
                matches = c,
                orgData = data,
                maskedData = maskedData
            };

            return result;
        }


        private struct MaskResult
        {
            public int matches;
            public int[][] orgData;
            public int[][] maskedData;
        }


        private static long[] GetMonsterMask()
        {
            string[] monster =
            {
                "                  # ",
                "#    ##    ##    ###",//20
                " #  #  #  #  #  #   "
            };

            return monster.Select(r => Convert.ToInt64(r.Replace(" ", "0").Replace("#", "1"), 2)).ToArray();
        }

        private static int[] CopyArr(long[] a)
        {
            return a.Select(x => (int)x).ToArray();
        }


        private static int[] CopyArr(int[] a)
        {
            int[] b = new int[a.Length];
            for (int i = 0; i < a.Length; i++)
                b[i] = a[i];
            return b;
        }


        private class Tile
        {
            public int Id;
            public int[][] Sides;
            public int[][] Data;

            public Tile(int id, char[][] data, bool truncateEdges = true)
            {
                Id = id;
                Data = data.Select(d => d.Select(c => c == '#' ? 1 : c == '.' ? 0 : 5).ToArray()).ToArray();
                
                var north = Data[0];
                var east = Data.Select(d => d[d.Length - 1]).ToArray();
                var south = Data[Data.Length - 1];
                var west = Data.Select(d => d[0]).ToArray();

                Sides = new int[4][];
                Sides[0] = CopyArr(north);
                Sides[1] = CopyArr(east);
                Sides[2] = CopyArr(south);
                Sides[3] = CopyArr(west);

                if (truncateEdges)
                {
                    Data = Data[1..(Data.Length - 1)];
                    for (int i = 0; i < Data.Length; i++)
                        Data[i] = Data[i][1..(Data[i].Length - 1)];
                }
            }

            public void Flip(bool UpDown)
            {
                int[] mem;
                if (UpDown)
                {
                    mem = Sides[0];
                    Sides[0] = Sides[2];
                    Sides[2] = mem;
                    Array.Reverse(Sides[1]);
                    Array.Reverse(Sides[3]);

                    for (int i = 0; i < Data.Length / 2; i++)
                    {
                        mem = Data[i];
                        Data[i] = Data[Data.Length - 1 - i];
                        Data[Data.Length - 1 - i] = mem;
                    }
                }
                else
                {
                    mem = Sides[1];
                    Sides[1] = Sides[3];
                    Sides[3] = mem;
                    Array.Reverse(Sides[0]);
                    Array.Reverse(Sides[2]);

                    foreach (var row in Data)
                        Array.Reverse(row);
                }
            }

            public void RotateClockwise()
            {
                int[] mem = Sides[3];
                Sides[3] = Sides[2];
                Sides[2] = Sides[1];
                Sides[1] = Sides[0];
                Sides[0] = mem;
                Array.Reverse(Sides[0]);
                Array.Reverse(Sides[2]);


                int[][] rotator = new int[Data.Length][];
                for (int i = 0; i < Data.Length; i++)
                    rotator[i] = new int[Data[i].Length];

                for (int i = 0; i < Data.Length; i++)
                    for (int j = 0; j < Data[i].Length; j++)
                        rotator[i][Data.Length - 1 - j] = Data[j][i];

                Data = rotator;
            }

            public bool MatchSide(int[] side)
            {
                var x = CopyArr(side);
                Array.Reverse(x);

                for (int i = 0; i < Sides.Length; i++)
                    if (side.SequenceEqual(Sides[i]) || x.SequenceEqual(Sides[i]))
                        return true;
                return false;
            }

            public bool IsReverse(Orientation o, int[] side)
            {
                var x = CopyArr(GetSide(o));
                Array.Reverse(x);
                return x.SequenceEqual(side);
            }

            public int[] GetSide(Orientation o) =>
                o switch
                {
                    Orientation.North => Sides[0],
                    Orientation.East  => Sides[1],
                    Orientation.South => Sides[2],
                    Orientation.West  => Sides[3],
                    _                 => null,
                };

            public Orientation GetOrientation(int[] side)
            {
                var x = CopyArr(side);
                Array.Reverse(x);

                for (int i = 0; i < Sides.Length; i++)
                    if (side.SequenceEqual(Sides[i]) ||  x.SequenceEqual(Sides[i]))
                        return GetOrientation(i);
                return Orientation.None;
            }

            private Orientation GetOrientation(int v) =>
                v switch
                {
                    0 => Orientation.North,
                    1 => Orientation.East,
                    2 => Orientation.South,
                    3 => Orientation.West,
                    _ => Orientation.None
                };
        }
    }
}
