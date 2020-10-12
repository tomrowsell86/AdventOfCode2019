using System;
using System.IO;
using System.Linq;
namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            var gridMap = new GridMap();

            var wirePaths = File.ReadAllLines("input.txt");

            for (int i = 0; i < wirePaths.Length; i++)
            {
                string path = wirePaths[i];
                gridMap.NewWire(i.ToString()[0]);
            
                foreach (var instruction in path.Split(','))
                {
                    gridMap.PlotNext(instruction);
                }
            }
            Console.WriteLine("Finished creating map");
            Console.WriteLine("Calculating Manhattan value");

            var value = gridMap.GenerateManhattanValue();
            Console.WriteLine($"Manhattan value of nearest wire intersection is {value}");
            
            Console.Read();
        }
    }
}
