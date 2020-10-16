using System;
using System.Collections.Generic;
using System.Linq;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {

            var expectedRankSequence = Enumerable.Range(0, 6).ToArray();


            var passwords = (args[0] == "--part" && args[1] == "b") ?
             GeneratePasswords(expectedRankSequence,"264793-803935",result=>result.Count == 2 ): 
             GeneratePasswords(expectedRankSequence,"264793-803935",result=>result == result);

            using (System.IO.StreamWriter streamWriter = System.IO.File.CreateText("Results.txt"))
            {
                passwords.ForEach(p =>
                {
                    streamWriter.WriteLine(p);
                });
            }
            Console.WriteLine($"PasswordCount{passwords.Count}");
        }

        private static List<string> GeneratePasswords(int[] expectedRankSequence, string rangeString,Func<AdjacentDigitOccurence, bool> scanResultPredicate)
        {

            return (from item in CreatePasswordRange(rangeString)
                   let sequence = item.Zip(expectedRankSequence, (c, n) =>
                   new { Value = int.Parse(c.ToString()), Index = n }).OrderBy(n => n.Value)
                   where sequence.Select(a => a.Index).SequenceEqual(expectedRankSequence)
                   where new AdjacentDigitScanner().Execute(item).Any(scanResultPredicate)
                   select item).ToList();
        }

        private static IEnumerable<string> CreatePasswordRange(string rangeString)
        {
            var lowerBound = int.Parse(rangeString.Substring(0,6));
            var upperBound = int.Parse(rangeString.Substring(7,6));
            return Enumerable.Range(lowerBound, (upperBound - lowerBound)+1).Select(n => n.ToString());

        }
    }

    internal class AdjacentDigitScanner
    {
        internal IEnumerable<AdjacentDigitOccurence> Execute(string permutation)
        {
            return permutation
            .Select(l => int.Parse(l.ToString()))
            .Zip(Enumerable.Range(0, 6)
            , (a, b) => new { Index = b, Digit = a })
            .GroupBy(n => n.Digit, n => n)
            .Where(n => n.Count() > 1)
            .Select(group=>new AdjacentDigitOccurence
            {

                Digit = group.Key,
                StartOffset = group.Min(g => g.Index),
                Count = group.Count()
            });
        }
    }

    internal class AdjacentDigitOccurence
    {
        public int Digit { get; set; }
        public int Count { get; set; }
        public int StartOffset { get; set; }
    }
}
