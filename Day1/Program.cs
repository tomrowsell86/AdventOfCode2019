using System;
using System.IO;
using System.Net.Http;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace AdventOfCode.Day.One
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new HttpClient();
 
            var subject = new Subject<int>();
            
            var total = subject
            .Select(i=>ModuleFuelCalculator.Calculate(i))
            .Do(i=>Console.WriteLine("OnNextCalled " + i))
            .Sum();
                
            total.Subscribe(t => Console.WriteLine("Total value : " + t));
            
            var stream = File.OpenRead("input.txt");
            using(var reader = new StreamReader(stream)){
                string line = null;
                while((line = reader.ReadLine()) != null){
                    subject.OnNext(int.Parse(line));
                }
                subject.OnCompleted();

                Console.ReadLine();
            }

        }
    }
}
