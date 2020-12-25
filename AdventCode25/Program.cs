using System;

namespace AdventCode25
{
    class Program
    {
        static void Main(string[] args)
        {
            var keyA = 9033205L;
            var keyB = 9281649L;

            var subjectNum = 7L;

            var loopA = TransformCrack(subjectNum, keyA);
            var loopB = TransformCrack(subjectNum, keyB);

            var key = Transform(keyB, loopA);

            Console.WriteLine(key);
        }


        public static long Transform(long num, long loops)
        {
            var x = 1L;

            for (var i = 0L; i < loops; i++)
            {
                x *= num;
                x %= 20201227;
            }

            return x;
        }


        public static long TransformCrack(long num, long target)
        {
            var x = 1L;
            var c = 0;

            while(x != target)
            {
                x *= num;
                x %= 20201227;
                c++;
            }

            return c;
        }
    }
}
