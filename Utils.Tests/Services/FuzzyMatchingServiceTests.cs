using NUnit.Framework;
using System;
using Utils.Services;

namespace Utils.Tests
{
    [TestFixture]
    public class FuzzyMatchingServiceTest
    {
        [TestCase("Test", "Teather", 3, ExpectedResult = 3)]
        [TestCase("Test", "Test", 4, ExpectedResult = 4)]
        [TestCase("Test", "Test", 0, ExpectedResult = 4)]
        [TestCase("Test", "Anthem", 3, ExpectedResult = 2)]
        public int GetMatchingCharacters_WhenCalled_ShouldReturnExpectedResult(string first, string second, int searchRange)
        {
            // arrange
            bool[] firstMatching = new bool[first.Length];
            bool[] secondMatching = new bool[second.Length];

            // act and assert
            return FuzzyMatchingService.GetMatchingCharacters(first, second, searchRange, firstMatching, secondMatching);
        }

        [TestCase("Post", "Teather", 1, 1, ExpectedResult = 1)]
        [TestCase("Post", "Teather", 3, 3, ExpectedResult = 3)]
        [TestCase("Get", "Test", 2, 2, ExpectedResult = 1)]
        [TestCase("Put", "Anthem", 3, 3, ExpectedResult = 2)]
        [TestCase("Put", "Anthem", 0, 0, ExpectedResult = 0)]
        public int GetTransposed_WhenCalled_ShouldReturnExpectedResult(string first, string second, int numberOfFirstMatching, int numberOfSecondMatching)
        {
            // arrange
            bool[] firstMatching = new bool[first.Length];
            bool[] secondMatching = new bool[second.Length];

            for (var i = 0; i < numberOfFirstMatching; i++)
            {
                firstMatching[i] = true;
            }

            for (var i = 0; i < numberOfSecondMatching; i++)
            {
                secondMatching[i] = true;
            }

            // act and assert
            return FuzzyMatchingService.GetTransposed(first, second, firstMatching, secondMatching);
        }

        [TestCase("Test", "Teather", 3, 2, ExpectedResult = 0.0d)]
        [TestCase("Test", "Test", 4, 1, ExpectedResult = 0.80000000000000004d)]
        [TestCase("Test", "Test", 2, 4, ExpectedResult = -0.33333333333333331d)]
        [TestCase("Test", "Anthem", 3, 3, ExpectedResult = 0.0d)]
        public double GetWeight_WhenCalled_ShouldReturnExpectedResult(string first, string second, int matchingCharacters, int halfTransposed)
        {
            // arrange
            FuzzyMatchingService fuzzyMatchingService =  new FuzzyMatchingService(0.3, 4);
            
            // act and assert
            return fuzzyMatchingService.GetWeight(first, second, matchingCharacters, halfTransposed);
        }

        [TestCase("Al", "Al", ExpectedResult = 1.0d)]
        [TestCase("MARTHA", "MARHTA", ExpectedResult = 0.76666666666666661d)]
        [TestCase("JONES", "JOHNSON", ExpectedResult = 0.46666666666666667d)]
        [TestCase("ABCVWXYZ", "CABVWXYZ", ExpectedResult = 0.66666666666666663d)]
        public double Proximity_WhenCalled_ShouldReturnExpectedResult(string first, string second)
        {
            // arrange
            IFuzzyMatchingService fuzzyMatchingService = new FuzzyMatchingService(0.1, 3);

            // act and assert
            return fuzzyMatchingService.Proximity(first, second);
        }

        [TestCase("Al", "Al", ExpectedResult = 0.0d)]
        [TestCase("MARTHA", "MARHTA", ExpectedResult = 0.23333333333333339d)]
        [TestCase("JONES", "JOHNSON", ExpectedResult = 0.53333333333333333d)]
        [TestCase("ABCVWXYZ", "CABVWXYZ", ExpectedResult = 0.33333333333333337d)]
        public double Distance_WhenCalled_ShouldReturnExpectedResult(string first, string second)
        {
            // arrange
            IFuzzyMatchingService fuzzyMatchingService = new FuzzyMatchingService(0.1, 3);

            // act and assert
            return fuzzyMatchingService.Distance(first, second);
        }
    }
}
