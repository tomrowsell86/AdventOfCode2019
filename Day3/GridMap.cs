using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
namespace Day3
{
    public class GridMap
    {
        private const int UpperBoundY = 19999;
        private const int UpperBoundX = 30000;
        private readonly char[,] _map = new char[30000, 20000];
        private int _currentX = 15000;
        private int _currentY = 10000;

        private char _currentWireId;
        public GridMap()
        {
            _map[_currentX, _currentY] = 'O';
        }
        public void NewWire(char id)
        {
            _currentWireId = id;
            _currentX = 15000;
            _currentY = 10000;
        }
        public void PlotNext(string instruction)
        {
            string direction = instruction.Substring(0, 1);
            int magnitude = int.Parse(instruction.Substring(1));
            foreach (var inc in Enumerable.Range(1, magnitude))
            {
                switch (direction)
                {
                    case "R":
                        _currentX++;
                        break;
                    case "U":
                        _currentY++;
                        break;
                    case "D":
                        _currentY--;
                        break;
                    case "L":
                        _currentX--;
                        break;
                }
                char currentVal = _map[_currentX, _currentY];
                _map[_currentX, _currentY] = currentVal != '\0' && currentVal != _currentWireId ? 'X' : _currentWireId;

            }
        }
        public int GenerateManhattanValue()
        {
            List<(int, int)> intersectionCoOrdinates = ScanForIntersections();

            var lowestManhattonValue = intersectionCoOrdinates.Select(i => Math.Abs(i.Item2 - 10000) +
                 Math.Abs(i.Item1 - 15000)).OrderBy(v => v).First();
            return lowestManhattonValue;
        }

        private List<(int, int)> ScanForIntersections()
        {
            var intersections = new List<(int, int)>();

            for (int y = UpperBoundY; y >= 0; y--)
            {
                for (int x = 0; x < UpperBoundX; x++)
                {
                    if (_map[x, y] == 'X')
                    {
                        Console.WriteLine($"Intersection found : x:{x}, y: {y}");
                        intersections.Add((x, y));
                    }
                }
            }

            return intersections;
        }
    }


}