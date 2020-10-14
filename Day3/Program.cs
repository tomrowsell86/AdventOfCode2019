using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            var wirePaths = File.ReadAllLines("input.txt");

            if (args[0] == "--part" && args[1].ToLower() == "a")
            {
                GridMap gridMap = new GridMap();
                ProcessGridMapInstructions(gridMap, 0, wirePaths.Length, wirePaths);
                Console.WriteLine("Calculating Manhattan value");

                var value = gridMap.GenerateManhattanValue();
                Console.WriteLine($"Manhattan value of nearest wire intersection is {value}");
            }
            else
            {
                var map = new GridMap();
                var intersections = new List<WireIntersectionEventArgs>();
                map.WireIntersected += (s, e) => intersections.Add(e);

                ProcessGridMapInstructions(map, 0, wirePaths.Length, wirePaths);
                //re-plot first wire to get the step counts.
                ProcessGridMapInstructions(map, 0, 1, wirePaths);

                var result = intersections
                .GroupBy(a => a.IntersectionPosition)
                .Select(g => g.GroupBy(ev => ev.WireId).Select(s => s.Min(s => s.CurrentStepIncrement)).Sum())
                .Min();

                Console.WriteLine($"result : {result}");
            }
            Console.Read();
        }

        private static void ProcessGridMapInstructions(GridMap gridMap, int instructionOffset, int length, string[] wirePaths)
        {
            for (int i = instructionOffset; i < length; i++)
            {
                string path = wirePaths[i];
                gridMap.NewWire(i.ToString()[0]);

                foreach (var instruction in path.Split(','))
                {
                    gridMap.PlotNext(instruction);
                }
            }
            Console.WriteLine("Finished creating map");
        }
    }
}
