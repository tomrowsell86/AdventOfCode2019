using System;
using System.IO;
using System.Linq;
namespace Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = File.ReadAllText("input.txt").Split(',').Select(c=> int.Parse(c.ToString())).ToArray();
            var interpreter = new OpCodeInterpreter(program);
            var map = interpreter.Run(1);
            Console.WriteLine("Hello World!");
        }
    }
}
