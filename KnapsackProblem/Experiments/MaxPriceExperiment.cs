using System.Collections.Generic;
using KnapsackSdk.Dtos;
using KnapsackSdk.Strategies;

namespace KnapsackProblem.Experiments
{
    public class MaxPriceExperiment : AbstractGeneratorExperiment
    {
        public override string Id => "MaxPrice";

        protected override DefinitionDto GenerateDefinitions(int parameter)
        {
            var weights = new[] {25, 36, 5, 18, 43, 31, 16, 25, 11, 45};
            return Generator.Generate(10, () => 100, index => weights[index], index => Random.Next(1, parameter));
        }

        protected override IEnumerable<int> GetParameters()
        {
            return new[] {20, 40, 60, 80, 100, 120, 140, 160, 180, 200};
        }

        protected override IEnumerable<IStrategy> GetStrategies()
        {
            return new List<IStrategy>
            {
                new BranchesAndBoundariesStrategy(),
                new BranchesAndBoundariesStrategy("2"),
                new BranchesAndBoundariesStrategy("3"),
                new BranchesAndBoundariesStrategy("4"),
                new FtpasStrategy(16),
                new FtpasStrategy(16,"2"),
                new FtpasStrategy(16,"3"),
                new FtpasStrategy(16,"4"),
            };
        }
    }
}