using System;

namespace AdventOfCode.Day.One
{
    public class ModuleFuelCalculator{
        public static double Calculate(double moduleMass){
            
            var fuelForModule = Math.Floor((double)moduleMass/3) - (double)2;
            if(fuelForModule <= 0) return 0;
            return fuelForModule + Calculate(fuelForModule);
        }
    }
}