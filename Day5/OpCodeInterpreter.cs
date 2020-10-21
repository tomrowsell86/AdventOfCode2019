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
        public int[] Run(int input)
        {
            var span = program.AsSpan();
            var position = 0;
            while (true)
            {
                var opCodeText = span[position].ToString();
                var paramModeRepository = ParameterModeRepository.ParseFromOpCode(opCodeText);

                var parsedOpCode = opCodeText.Length > 1 ? int.Parse(opCodeText.Substring(opCodeText.Length - 2, 2)) : span[position];

                switch (parsedOpCode)
                {
                    case 1:

                        Operation(span, position, (a, b) => a + b, paramModeRepository);
                        position += 4;
                        break;
                    case 2:
                        Operation(span, position, (a, b) => a * b, paramModeRepository);
                        position += 4;
                        break;
                    case 3:

                        span[span[position + 1]] = input;
                        position += 2;

                        break;
                    case 4:

                        var outputValue = GetParameterValue(span, 0, paramModeRepository, position);
                        Console.WriteLine(outputValue);
                        position += 2;
                        break;

                    case 5:
                        //GetParameterValue(span, position, paramModeRepository);

                        break;
                    case 99:
                        return span.ToArray();
                    default: position++; break;

                }

            }
        }

        private static int GetParameterValue(Span<int> span, int position, ParameterModeRepository paramModeRepository, int instructionPointer) =>
            paramModeRepository.IsByRefParameterModeAt(position) ? span[span[instructionPointer + position + 1 ]] : span[instructionPointer + position + 1];


        private static void Operation(Span<int> span, int position, Func<int, int, int> operation, ParameterModeRepository parameterModeRepository)
        {
            var arg1 = GetParameterValue(span, 0, parameterModeRepository, position);
            var arg2 = GetParameterValue(span, 1, parameterModeRepository, position);
         
            var resultPosition = span[position + 3];
            span[resultPosition] = operation(arg1, arg2);

        }
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