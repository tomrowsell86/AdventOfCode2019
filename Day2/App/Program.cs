using System;
using System.IO;
using System.Linq;

namespace AdventOfCode.Day.Two
{
    class Program
    {
        static void Main(string[] args){
            Console.WriteLine("Starting run...");

            using(var sr = new StreamReader(File.OpenRead("input.txt"))){
                var program = sr.ReadToEnd().Split(',');
                var permutationGenerator = new PermutationGenerator(program.Select(i => int.Parse(i)).ToArray());
                int result;
                do{
                    permutationGenerator.Next();
                    
                }
                while ((result = new OpCodeInterpreter(permutationGenerator.Current).Run()[0])!= 19690720);
                
                Console.WriteLine($"Results are in : {string.Join(',',result)}");
                int noun = permutationGenerator.Current[1];
                int verb = permutationGenerator.Current[2];
                Console.WriteLine($"Inputs were noun: {noun}, verb:{verb}");
                Console.WriteLine($"Final Answer: {100 * noun + verb}");
                Console.ReadLine();        
                 }
        }
    }
}