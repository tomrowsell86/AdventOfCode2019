using System;

namespace AdventOfCode.Day.Two{
    public class OpCodeInterpreter{
        private readonly int[] program;

        public OpCodeInterpreter(int[] program )
        {
            this.program = program;
        }
        public int[] Run(){
            var span = program.AsSpan();
            var position = 0;
            while(true)
            {
                switch(span[position]){
                    case 1:

                        Operation(span, position, (a,b) => a+b);
                        break;
                    case 2:
                        Operation(span, position, (a,b) => a*b);
                        break;
                    case 99:
                        return span.ToArray();

                }
                position = position + 4;
            }
        }

        private static void Operation(Span<int> span, int position, Func<int,int,int> operation)
        {
            var arg1 = span[span[position + 1]];
            var arg2 = span[span[position + 2]];
            var resultPosition = span[position + 3];
            span[resultPosition] = operation(arg1,arg2);

        }
    }
    }