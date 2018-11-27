using System.Collections.Generic;
using KnapsackSdk.Dtos;
using KnapsackSdk.Strategies;
using NUnit.Framework;

namespace KnapsackUnitTests.Strategies
{
    [TestFixture]
    public class GeneticStrategyUnitTest
    {
        protected GeneticStrategy Strategy { get; set; }

        [SetUp]
        public void Setup()
        {
            Definition = new DefinitionDto(9350, 400, new List<ItemDto>
            {
                new ItemDto(22, 61),
                new ItemDto(28, 24),
                new ItemDto(1, 31),
                new ItemDto(6, 73),
                new ItemDto(38, 92),
                new ItemDto(5, 168),
                new ItemDto(11, 65),
                new ItemDto(20, 4),
                new ItemDto(46, 54),
                new ItemDto(3, 165),
                new ItemDto(32, 17),
                new ItemDto(14, 251),
                new ItemDto(42, 146),
                new ItemDto(35, 45),
                new ItemDto(33, 147),
                new ItemDto(21, 108),
                new ItemDto(4, 211),
                new ItemDto(15, 78),
                new ItemDto(8, 216),
                new ItemDto(40, 59),
                new ItemDto(39, 235),
                new ItemDto(2, 152),
                new ItemDto(17, 187),
                new ItemDto(9, 9),
                new ItemDto(44, 3),
                new ItemDto(16, 40),
                new ItemDto(12, 72),
                new ItemDto(43, 67),
                new ItemDto(7, 175),
                new ItemDto(25, 126)
            });
            Result = new ResultDto(9350,2875, new List<bool>
            {
                true,false,true,true,true,true,true,false,false,true,false,true,true,false,true,true,true,true,true,false,true,true,true,true,false,true,true,true,true,true
            });
        }

        public ResultDto Result { get; set; }

        public DefinitionDto Definition { get; set; }

        [TestFixture]
        public sealed class GeneticStrategyUnitTestShould : GeneticStrategyUnitTest
        {
            [Test]
            public void ConvergateToSomeResults()
            {
                Strategy = new GeneticStrategy(100,10,5,80, ECrossStrategy.Random);
                var result = Strategy.Compute(Definition);
                Assert.IsTrue(Result.Price >= result.Item1.Price);
                Assert.IsTrue(result.Item1.Price >= 0);
            }
        }
    }
}