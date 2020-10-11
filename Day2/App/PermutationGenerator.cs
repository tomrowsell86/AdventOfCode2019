using System.Linq;

namespace AdventOfCode.Day.Two
{
    internal class PermutationGenerator
    {
        private readonly int[] initialProgram;
        private int currentVerb;
        private int currentNoun;

        public PermutationGenerator(int[] initialProgram)
        {
            this.initialProgram = initialProgram;
            this.currentVerb = initialProgram[2];
            this.currentNoun = initialProgram[1];
        }

        public int[] Current { get; private set;}

        public void Next(){

            if(currentVerb + 1 >= initialProgram.Length)
            {
                this.currentVerb = 2;
                ++this.currentNoun;
            }
            else{
                ++currentVerb;
            }
            System.Console.WriteLine($"Current state of generator : currentNoun: {currentNoun}, current verb:{currentVerb}");

            Current = initialProgram.Take(1)
            .Append(currentNoun)
            .Append(currentVerb)
            .Concat(initialProgram.Skip(3))
            .ToArray();

        }

    }
}