using KnapsackSdk.Strategies;
using NUnit.Framework;

namespace KnapsackUnitTests.Strategies
{
    [TestFixture]
    public class RatioStrategyUnitTest
    {
        protected RatioStrategy Strategy { get; set; }

        [SetUp]
        public void Setup()
        {
            Strategy = new RatioStrategy();
        }

        [TestFixture]
        public sealed class RatioStrategyUnitTestShould : RatioStrategyUnitTest
        {
            [Test]
            public void ReturnCorrectResult()
            {
            }
        }
    }
}