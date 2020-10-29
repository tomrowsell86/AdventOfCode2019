using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Parsing input file...");

            string[] vs = File.ReadAllLines("input.txt");

            var orbits = vs.Select(o => o.Split(')'))
            .Select(o => new Orbit { Center = o[0], Orbitter = o[1] }).ToList();

            var scannedOrbits = new List<Orbit>();
            var terminalChainOrbits = orbits.Except(orbits.Join(orbits, o => o.Orbitter, o => o.Center, (a, b) => a)).ToList();

            var count = terminalChainOrbits.SelectMany(o => GetOrbitsFrom(o, orbits,scannedOrbits)).Count();
            Console.WriteLine($"Orbit count for input data is {count}");
        }

        private static IEnumerable<Orbit> GetOrbitsFrom(Orbit orbit, IEnumerable<Orbit> orbitBag, IList<Orbit> scannedOrbits)
        {
            Console.WriteLine($"Scanning orbit with center {orbit.Center} and orbitter {orbit.Orbitter}...");
            Orbit orbitCenter = orbit;
          

            if (scannedOrbits.Contains(orbit))
            {
                Console.WriteLine($"Skipping orbit scan as already completed...");

                return Enumerable.Empty<Orbit>();
            }
            var orbits = new List<Orbit>();

            orbits.AddRange(GetOrbitsToCOM(orbitBag, orbitCenter));
            scannedOrbits.Add(orbit);
            Console.WriteLine($"{orbits.Count} orbits back to COM found");
            
            var parentOrbit = orbitBag.SingleOrDefault(o => o.Orbitter == orbit.Center);

            if (parentOrbit.Equals(default(Orbit)))
            {
                return orbits;
            }
            else
            {
                return GetOrbitsFrom(parentOrbit, orbitBag, scannedOrbits).Concat(orbits);

            }
        }

        private static IEnumerable<Orbit> GetOrbitsToCOM(IEnumerable<Orbit> orbitBag, Orbit orbitCenter)
        {
            do
            {
                yield return orbitCenter;
                orbitCenter = orbitBag.SingleOrDefault(o => o.Orbitter == orbitCenter.Center);
            }
            while (!orbitCenter.Equals(default(Orbit)));
        }
    }

    internal struct Orbit
    {
        public string Center { get; set; }
        public string Orbitter { get; set; }
    }
}
