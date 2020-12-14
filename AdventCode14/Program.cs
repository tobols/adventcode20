using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventCode14
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = ReadFile().ToList();
            var memory = new Dictionary<long, long>();
            int memId;
            long maskO, maskA, num;
            maskO = maskA = 0;

            foreach (var line in lines)
            {
                if (line.StartsWith("mask"))
                {
                    maskO = Convert.ToInt64(line.Substring(7).Replace('X', '0'), 2);
                    maskA = Convert.ToInt64(line.Substring(7).Replace('X', '1'), 2);
                    continue;
                }

                memId = int.Parse(line[4..line.IndexOf(']')]);
                num = int.Parse(line.Substring(line.IndexOf('=') + 2));

                if (memory.ContainsKey(memId))
                    memory[memId] = (num | maskO) & maskA;
                else
                    memory.Add(memId, (num | maskO) & maskA);
            }

            Console.WriteLine(memory.Sum(m => m.Value));



            memory = new Dictionary<long, long>();
            char[] mask = new char[0];

            foreach (var line in lines)
            {
                if (line.StartsWith("mask"))
                {
                    mask = line.Substring(7).ToCharArray();
                    continue;
                }

                memId = int.Parse(line[4..line.IndexOf(']')]);
                var mId = Convert.ToString(memId, 2).ToCharArray();
                var m = "";

                for (int i = 0; i < mask.Length; i++)
                {
                    if (i >= mask.Length - mId.Length && mask[i] == '0')
                        m += mId[i + mId.Length - mask.Length];
                    else
                        m += mask[i];
                }

                WriteMem(m.ToCharArray(), memory, int.Parse(line.Substring(line.IndexOf('=') + 2)));
            }

            Console.WriteLine(memory.Sum(m => m.Value));
        }


        private static void WriteMem(char[] memId, Dictionary<long, long> memory, long num)
        {
            var i = Array.FindIndex(memId, c => c == 'X');

            if (i < 0)
            {
                var id = Convert.ToInt64(new string(memId), 2);
                if (memory.ContainsKey(id))
                    memory[id] = num;
                else
                    memory[id] = num;
            }
            else
            {
                var m = new char[memId.Length];
                Array.Copy(memId, m, memId.Length);
                m[i] = '0';
                WriteMem(m, memory, num);
                m[i] = '1';
                WriteMem(m, memory, num);
            }
        }


        private static IEnumerable<string> ReadFile()
        {
            var file = new StreamReader(Path.Combine(Environment.CurrentDirectory, "data.txt"));
            string line;
            while ((line = file.ReadLine()) != null)
                yield return line;
            file.Close();
        }
    }
}
