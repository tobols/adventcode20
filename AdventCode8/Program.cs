using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode8
{
    class Program
    {
        static void Main(string[] args)
        {
            DataMemory mem = new DataMemory();
            Processor processor;
            mem.Set(ReadFile().ToArray());

            Console.WriteLine("\nRUNNING PROCESSOR\n");

            processor = new Processor(mem);
            try { while (true) processor.ClockTick(); }
            catch (LoopException ex) { Console.WriteLine(ex.Message); }

            Console.WriteLine("\nDEBUGGING\n");

            for (int i = 210; i >= 0; i--)
            {
                processor = new Processor(mem, i);
                try { while (true) processor.ClockTick(); }
                catch (LoopException) { }
                catch (IndexOutOfRangeException) { return; }
            }
        }

        private static IEnumerable<string> ReadFile()
        {
            var file = new System.IO.StreamReader(@"C:\Repos\temp\AdventCode\AdventCode8\data.txt");
            string line;
            while ((line = file.ReadLine()) != null)
                yield return line;
            file.Close();
        }

        private class Processor
        {
            public int Accumulator = 0;
            public int ProgramCounter = 0;
            public Dictionary<int, string> VisitedInstructions = new Dictionary<int, string>();
            private string NextInstruction;
            private DataMemory _storage;
            private int _debugInstruction = -1;
            private int _clockTicks = 0;
            private bool _debugFlag = false;

            public Processor(DataMemory storage)
            {
                _storage = storage;
            }

            public Processor(DataMemory storage, int debugInstruction)
            {
                _storage = storage;
                _debugInstruction = debugInstruction;
                _debugFlag = true;
            }

            public void ClockTick()
            {
                ReadInstruction();
                ExecuteInstruction();
                IncreaseProgramCounter();
                RunSimpleLoopSafetyMechanism();

                if (_debugFlag) _clockTicks++;
            }

            private void IncreaseProgramCounter()
            {
                ProgramCounter++;
            }

            private void ReadInstruction()
            {
                try { NextInstruction = _storage.Get(ProgramCounter); }
                catch { Console.WriteLine($"INDEXOUTOFRANGE.\nREGISTERDUMP:\nACCUMULATOR={Accumulator}.\nPROGRAMCOUNTER={ProgramCounter}."); throw; }
            }

            private void ExecuteInstruction()
            {
                var next = NextInstruction[..3];
                if (_debugFlag && _debugInstruction == _clockTicks)
                    next = next == "jmp" ? "nop" : next == "nop" ? "jmp" : next;

                switch (next)
                {
                    case "nop": break;
                    case "jmp": ProgramCounter += int.Parse(NextInstruction.Substring(3)) - 1; break;
                    case "acc": Accumulator += int.Parse(NextInstruction.Substring(3)); break;
                }
            }

            private void RunSimpleLoopSafetyMechanism()
            {
                if (VisitedInstructions.ContainsKey(ProgramCounter))
                    throw new LoopException($"LOOPEXCEPTION.\nREGISTERDUMP:\nACCUMULATOR={Accumulator}.\nPROGRAMCOUNTER={ProgramCounter}.");
                VisitedInstructions.Add(ProgramCounter, NextInstruction);
            }
        }

        private class DataMemory
        {
            private string[] _data;

            public void Set(string[] data)
            {
                _data = data;
            }

            public string Get(int row)
            {
                return _data[row];
            }
        }

        private class LoopException : Exception
        {
            public LoopException(string message)
                : base(message)
            { }
        }
    }
}
