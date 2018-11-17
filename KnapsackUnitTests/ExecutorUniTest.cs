using System.Collections.Generic;
using System.Linq;
using KnapsackProblem;
using KnapsackSdk.Dtos;
using KnapsackSdk.Strategies;
using NUnit.Framework;

namespace KnapsackUnitTests
{
    [TestFixture]
    public class ExecutorUniTest
    {

        private IDictionary<DefinitionDto, ResultDto> _samples = new Dictionary<DefinitionDto, ResultDto>();
        protected Executor Executor { get; set; }

        protected readonly IList<IStrategy> Strategies = new List<IStrategy>
        {
            new IterativeStrategy(),
            new RecursiveStrategy(),
            new BranchesAndBoundariesStrategy(),
            new DecompositionByPriceStrategy(),
            new DecompositionByWeightStrategy(),
            new DecompositionByWeightRecursiveStrategy(),
            new FtpasStrategy(2),
            new FtpasStrategy(4),
            new FtpasStrategy(8),
            new FtpasStrategy(16),
            new RatioStrategy(),
        };
        [SetUp]
        public void Setup()
        {
            Executor = new Executor();
            _samples.Add(GetDefinition(1,100, 18, 114, 42, 136, 88, 192, 3, 223), GetResult(1, 473, true, true, false, true));
            _samples.Add(GetDefinition(2,100, 55, 29 , 81, 64 , 14, 104, 52, 222), GetResult(2, 326, false, false, true, true));
            _samples.Add(GetDefinition(3,100, 89, 196, 18, 62 , 57, 34 , 69, 112), GetResult(3, 196, true, false, false, false));
            _samples.Add(GetDefinition(4,100, 34, 169, 23, 152, 62, 44 , 2 , 224), GetResult(4, 545, true, true, false, true));

            _samples.Add(GetDefinition(5,100, 67, 145, 74, 111, 87, 139, 65, 243), GetResult(5, 243, false, false, false, true));
            _samples.Add(GetDefinition(6,100, 12, 66 , 52, 167, 16, 150, 3 ,180), GetResult(6, 563, true, true, true, true));
            _samples.Add(GetDefinition(7,100, 85, 190, 13, 165, 4 ,47  ,83 ,198), GetResult(7, 410, false, true, true, true));
            _samples.Add(GetDefinition(8,100, 31, 96 , 38, 77 , 50, 1  ,22 ,202), GetResult(8, 375, true, true, false, true));
            _samples.Add(GetDefinition(8,100, 88, 235, 26, 224, 76, 220, 80, 194), GetResult(8, 235, true, false, false, false));
        }

        private static ResultDto GetResult(int id, int price, bool is1, bool is2, bool is3, bool is4)
        {
            return new ResultDto(id, price, new List<bool>
                {
                    is1,is2,is3,is4
                });
        }

        private static DefinitionDto GetDefinition(int id, int capacity, int weight1, int price1, int weight2, int price2, int weight3, int price3, int weight4, int price4)
        {
            return new DefinitionDto(id, capacity, new List<ItemDto>
                {
                    new ItemDto(weight1, price1),
                    new ItemDto(weight2, price2),
                    new ItemDto(weight3, price3),
                    new ItemDto(weight4, price4)
                });
        }

        [TestFixture]
        public sealed class ExecutorUniTestShould : ExecutorUniTest
        {
            [Test]
            [TestCase(EStrategy.Iteration)]
            [TestCase(EStrategy.Recursive)]
            [TestCase(EStrategy.BandB)]
            [TestCase(EStrategy.DecompositionByPrice)]
            [TestCase(EStrategy.DecompositionByWeight)]
            [TestCase(EStrategy.DecompositionByWeightRecursive)]
            public void ReturnExactResultForIteration(EStrategy strategy)
            {
                var result = Executor.ExecuteStrategy(_samples.Keys.ToList(), _samples.Keys.Select(key => _samples[key]).ToList(), GetStrategy(strategy), 1);
                Assert.AreEqual(0,result.MaxError);
                Assert.AreEqual(0,result.RelativeError);
                Assert.AreEqual(GetStrategy(strategy).Id, result.StrategyId);
            }

            [Test]
            public void ReturnCorrectErrorsForRatioStrategy()
            {
                var strategy = new RatioStrategy();
                var result = Executor.ExecuteStrategy(_samples.Keys.ToList(), _samples.Keys.Select(key => _samples[key]).ToList(), strategy, 2);
                Assert.AreEqual(0.1122,result.MaxError,0.0001);
                Assert.AreEqual(0.1590,result.RelativeError, 0.0001);
            }

            [Test]
            [TestCase(EStrategy.Iteration,16)]
            [TestCase(EStrategy.Recursive,16)]
            [TestCase(EStrategy.BandB,2)]
            [TestCase(EStrategy.DecompositionByPrice,2664)]
            [TestCase(EStrategy.DecompositionByWeight,404)]
            [TestCase(EStrategy.DecompositionByWeightRecursive,10)]
            [TestCase(EStrategy.Ftpas2,1336)]
            [TestCase(EStrategy.Ftpas4,672)]
            [TestCase(EStrategy.Ftpas8,340)]
            [TestCase(EStrategy.Ftpas16,172)]
            [TestCase(EStrategy.Ratio,1)]
            public void ReturnCountersCorrectly(EStrategy strategyIndex, long expectedResult)
            {
                var strategy = GetStrategy(strategyIndex);
                var result = strategy.Compute(_samples.Keys.First());
                Assert.AreEqual(expectedResult, result.Item2);
            }

            private IStrategy GetStrategy(EStrategy strategyIndex)
            {
                return Strategies[(int)strategyIndex];
            }
        }
    }
}