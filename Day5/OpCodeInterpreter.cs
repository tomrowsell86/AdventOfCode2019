using System;
using System.Linq;
namespace Day5
{
    public class OpCodeInterpreter
    {
        private readonly int[] program;

        public OpCodeInterpreter(int[] program)
        {
            this.program = program;
        }
        public Span<int> Run(int input)
        {
            var programSpan = program.AsSpan();
            var instructionPointer = 0;

            while (true)
            {
                var opCodeText = programSpan[instructionPointer].ToString();
                var paramModeRepository = ParameterModeRepository.ParseFromOpCode(opCodeText);
                var extractor = new ParameterExtractor(paramModeRepository);

                var parsedOpCode = opCodeText.Length > 1 ? int.Parse(opCodeText.Substring(opCodeText.Length - 2, 2)) : programSpan[instructionPointer];

                switch (parsedOpCode)
                {
                    case 1:
                        Operation(programSpan, instructionPointer, (a, b) => a + b, extractor);
                        instructionPointer += 4;
                        break;
                    case 2:
                        Operation(programSpan, instructionPointer, (a, b) => a * b, extractor);
                        instructionPointer += 4;
                        break;
                    case 3:

                        programSpan[programSpan[instructionPointer + 1]] = input;
                        instructionPointer += 2;
                        break;
                    case 4:
                        var outputValue = extractor.GetParameterValue(programSpan, 0, instructionPointer);
                        Console.WriteLine(outputValue);
                        instructionPointer += 2;
                        break;

                    case 5:
                        JumpToWhenTrue(programSpan, ref instructionPointer, extractor, v => v != 0);
                        break;

                    case 6:
                        JumpToWhenTrue(programSpan, ref instructionPointer, extractor, v => v == 0);
                        break;

                    case 7:
                        WriteTrueIfTrueAt(programSpan, ref instructionPointer, extractor, (a, b) => a < b);

                        break;
                    case 8:
                        WriteTrueIfTrueAt(programSpan, ref instructionPointer, extractor, (a, b) => a == b);
                        break;
                    case 99:
                        return programSpan;
                }
            }
        }

        private static void WriteTrueIfTrueAt(Span<int> programSpan, ref int instructionPointer, ParameterExtractor extractor, Func<int, int, bool> predicate)
        {
            var arg1 = extractor.GetParameterValue(programSpan, 0, instructionPointer);
            var arg2 = extractor.GetParameterValue(programSpan, 1, instructionPointer);
            var writeToPointer = instructionPointer + 3;

            programSpan[programSpan[writeToPointer]] = predicate(arg1, arg2) ? 1 : 0;
            instructionPointer += 4;
        }

        private static void JumpToWhenTrue(Span<int> programSpan, ref int instructionPointer, ParameterExtractor extractor, Func<int, bool> predicate)
        {
            var testValue = extractor.GetParameterValue(programSpan, 0, instructionPointer);
            if (predicate(testValue))
            {
                instructionPointer = extractor.GetParameterValue(programSpan, 1, instructionPointer);
            }
            else
            {
                instructionPointer += 3;
            }
        }

        private static void Operation(Span<int> program, int instructionPointer, Func<int, int, int> operation, ParameterExtractor extractor)
        {
            var arg1 = extractor.GetParameterValue(program, 0, instructionPointer);
            var arg2 = extractor.GetParameterValue(program, 1, instructionPointer);

            var resultPosition = program[instructionPointer + 3];
            program[resultPosition] = operation(arg1, arg2);

        }
    }

    internal class ParameterExtractor
    {
        private ParameterModeRepository paramModeRepository;

        public ParameterExtractor(ParameterModeRepository paramModeRepository)
        {
            this.paramModeRepository = paramModeRepository;
        }

        public int GetParameterValue(Span<int> program, int parameterPosition, int instructionPointer) 
            => paramModeRepository.IsByRefParameterModeAt(parameterPosition) ? 
                program[program[instructionPointer + parameterPosition + 1]]
                : 
                program[instructionPointer + parameterPosition + 1];
    }

    internal class ParameterModeRepository
    {
        private readonly int[] parameterModes;
        private ParameterModeRepository(int[] parameterModes)
        {
            this.parameterModes = parameterModes;
        }

        public bool IsByRefParameterModeAt(int paramPosition) => parameterModes.Count() < paramPosition + 1 || parameterModes[paramPosition] == 0;

        public static ParameterModeRepository ParseFromOpCode(string opCode)
        {
            var modes = opCode.Length > 1 ? opCode
               .Substring(0, opCode.Length - 2)
               .Reverse()
               .Select(v => int.Parse(v.ToString()))
               .ToArray() : new int[0];
            return new ParameterModeRepository(modes);
        }
    }


}