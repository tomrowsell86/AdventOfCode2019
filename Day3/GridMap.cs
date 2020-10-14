using System;
using System.Linq;
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

        private int _totalSteps;
        private char _currentWireId;
        public event EventHandler<WireIntersectionEventArgs> WireIntersected;

        public void NewWire(char id)
        {
            _currentWireId = id;
            _totalSteps = 0;
            _currentX = 15000;
            _currentY = 10000;
        }
        public void PlotNext(string instruction)
        {
            string direction = instruction.Substring(0, 1);
            int magnitude = int.Parse(instruction.Substring(1));
            foreach (var inc in Enumerable.Range(1, magnitude))
            {

                _totalSteps++;
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
                if (currentVal != '\0' && currentVal != _currentWireId)
                {
                    if (WireIntersected != null)
                    {
                        WireIntersected(this, new WireIntersectionEventArgs(CurrentPosition, _totalSteps, _currentWireId));
                    }
                    _map[_currentX, _currentY] = 'X';
                }
                 else
                 {  _map[_currentX, _currentY] = _currentWireId;
                 }

            }
        }
        public int GenerateManhattanValue()
        {
            var intersectionCoOrdinates = ScanForIntersections();

            var lowestManhattonValue = intersectionCoOrdinates.Select(i => Math.Abs(i.Y - 10000) +
                 Math.Abs(i.X - 15000)).OrderBy(v => v).First();
            return lowestManhattonValue;
        }

        public List<Position> ScanForIntersections()
        {
            var intersections = new List<Position>();

            for (int y = UpperBoundY; y >= 0; y--)
            {
                for (int x = 0; x < UpperBoundX; x++)
                {
                    if (_map[x, y] == 'X')
                    {
                        Console.WriteLine($"Intersection found : x:{x}, y: {y}");
                        intersections.Add(new Position { X = x, Y = y });
                    }
                }
            }

            return intersections;
        }

        public Position CurrentPosition
        {
            get { return new Position { X = _currentX, Y = _currentY }; }
        }
    }
    public class WireIntersectionEventArgs : EventArgs
    {
        public char WireId {get;}
        public WireIntersectionEventArgs(Position intersectionPosition, int currentStepIncrement, char wireId) : base()
        {
            IntersectionPosition = intersectionPosition;
            CurrentStepIncrement = currentStepIncrement;
            WireId = wireId;
        }
        public Position IntersectionPosition { get; }
        public int CurrentStepIncrement { get; }
    }

    public struct Position
    {
        public static bool operator ==(Position a, Position b) => a.Equals(b);
        public static bool operator !=(Position a, Position b) => !a.Equals(b);

        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Position position &&
                   X == position.X &&
                   Y == position.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
