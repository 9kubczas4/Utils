using System;
using System.Runtime.CompilerServices;
using Utils.Services;

[assembly: InternalsVisibleTo("Utils.Tests")]
namespace Utils
{
    public class FuzzyMatchingService : IFuzzyMatchingService
    {
        private readonly double WeightThreshold;
        private readonly int NumberOfCharacters;

        public FuzzyMatchingService(double weightThreshold, int numberOfCharacters)
        {
            WeightThreshold = weightThreshold;
            NumberOfCharacters = numberOfCharacters;
        }

        public double Distance(string first, string second)
        {
            return 1.0 - Proximity(first, second);
        }

        public double Proximity(string first, string second)
        {
            if (first.Length == 0)
            {
                return second.Length == 0 ? 1.0 : 0.0;
            }

            var searchRange = Math.Max(0, Math.Max(first.Length, second.Length) / 2 - 1);
            var firstMatchedCharacters = new bool[first.Length];
            var secondMatchedCharacters = new bool[second.Length];

            int matchingCharacters = GetMatchingCharacters(first, second, searchRange, firstMatchedCharacters, secondMatchedCharacters);
            if (matchingCharacters == 0)
            {
                return 0.0;
            }

            int transposed = GetTransposed(first, second, firstMatchedCharacters, secondMatchedCharacters);
            int halfTransposed = transposed / 2;

            return GetWeight(first, second, matchingCharacters, halfTransposed);
        }

        internal double GetWeight(string first, string second, int matchingCharacters, int halfTransposed)
        {
            var weight = (matchingCharacters / first.Length
                + matchingCharacters / second.Length
                + (matchingCharacters - halfTransposed) / matchingCharacters) / 3.0;

            if (weight <= WeightThreshold)
            {
                return weight;
            }

            var commonPrefixMax = Math.Min(NumberOfCharacters, Math.Min(first.Length, second.Length));
            var commonPrefixLength = 0;

            while (commonPrefixLength < commonPrefixMax && first[commonPrefixLength] == second[commonPrefixLength])
            {
                commonPrefixLength++;
            }

            if (commonPrefixLength == 0)
            {
                return weight;
            }

            return weight + 0.1 * commonPrefixLength * (1.0 - weight);
        }

        internal static int GetTransposed(string first, string second, bool[] firstMatchedCharacters, bool[] secondMatchedCharacters)
        {
            var transposed = 0;
            var k = 0;

            for (var i = 0; i < first.Length; i++)
            {
                if (!firstMatchedCharacters[i])
                {
                    continue;
                }
                while (!secondMatchedCharacters[k])
                {
                    k++;
                }
                if (first[i] != second[k])
                {
                    transposed++;
                }
                k++;
            }
            return transposed;
        }

        internal static int GetMatchingCharacters(string first, string second, int searchRange, bool[] firstMatchedCharacters, bool[] secondMatchedCharacters)
        {
            var matchingCharacters = 0;
            for (var i = 0; i < first.Length; i++)
            {
                var start = Math.Max(0, i - searchRange);
                var end = Math.Min(i + searchRange + 1, second.Length);

                for (var j = start; j < end; j++)
                {
                    if (secondMatchedCharacters[j] || first[i] != second[j])
                    {
                        continue;
                    }
                    firstMatchedCharacters[i] = true;
                    secondMatchedCharacters[j] = true;
                    matchingCharacters++;
                    break;
                }
            }
            return matchingCharacters;
        }
    }
}
