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

                var parsedOpCode = opCodeText.Length > 1 ? int.Parse(opCodeText.Substring(opCodeText.Length - 2, 2)) : span[position];

                var parameterModes = opCodeText.Length > 1 ? opCodeText
                .Substring(0, opCodeText.Length - 2)
                .Reverse()
                .Select(v => int.Parse(v.ToString()))
                .ToArray() : new int[0];

                switch (parsedOpCode)
                {
                    case 1:

                        Operation(span, position, (a, b) => a + b, parameterModes);
                        position += 4;
                        break;
                    case 2:
                        Operation(span, position, (a, b) => a * b, parameterModes);
                        position += 4;
                        break;
                    case 3:

                        span[span[position + 1]] = input;
                        position += 2;

                        break;
                    case 4:

                        var outputValue = IsByRefParameterModeAt(parameterModes, 0) ? 
                        span[span[position + 1]] : 
                        span[position + 1];
                        Console.WriteLine(outputValue);
                        position += 2;
                        break;
                    case 99:
                        return span.ToArray();
                    default: position++; break;

                }

            }
        }

        private static void Operation(Span<int> span, int position, Func<int, int, int> operation, int[] parameterModes)
        {
            var arg1 = IsByRefParameterModeAt(parameterModes, 0) ? span[span[position + 1]] : span[position + 1];
            var arg2 = IsByRefParameterModeAt(parameterModes, 1) ? span[span[position + 2]] : span[position + 2];
            var resultPosition = span[position + 3];
            span[resultPosition] = operation(arg1, arg2);

        }

        private static bool IsByRefParameterModeAt(int[] parameterModes, int position)
        {
            return parameterModes.Count() < position + 1 || parameterModes[position] == 0;
        }
    }
}