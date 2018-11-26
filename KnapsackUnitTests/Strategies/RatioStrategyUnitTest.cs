using System.Collections.Generic;
using KnapsackSdk.Dtos;
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
            public void ReturnMostValuableItem()
            {
                var items = new List<ItemDto>
                {
                    new ItemDto(51,52),
                    new ItemDto(90,60),
                };
                var definition = new DefinitionDto(1,100,items);
                var result = Strategy.Compute(definition);
                Assert.AreEqual(items[1].Price, result.Item1.Price);
            }

            [Test]
            public void ReturnSumOfSmallItems()
            {
                var items = new List<ItemDto>
                {
                    new ItemDto(51,52),
                    new ItemDto(50,50),
                    new ItemDto(30,30),
                    new ItemDto(20,19),
                };
                var definition = new DefinitionDto(1,100,items);
                var result = Strategy.Compute(definition);
                Assert.AreEqual(items[0].Price+items[2].Price,result.Item1.Price);
            }
        }
    }
}