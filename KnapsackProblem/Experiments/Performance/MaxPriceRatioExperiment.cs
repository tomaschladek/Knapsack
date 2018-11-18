using System.Collections.Generic;
using KnapsackSdk.Dtos;
using KnapsackSdk.Strategies;

namespace KnapsackProblem.Experiments.Performance
{
    public class MaxPriceRatioExperiment : AbstractGeneratorExperiment
    {
        public override string Id => "MaxPriceRatio12";

        protected override DefinitionDto GenerateDefinitions(int parameter)
        {
            return Generator.Generate(10, () => 100, index => 12, index => Random.Next(1,100) > parameter ? Random.Next(100, 200) : Random.Next(1,100));
        }

        protected override IEnumerable<int> GetParameters()
        {
            return new[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
        }

        protected override IEnumerable<IStrategy> GetStrategies()
        {
            return new List<IStrategy>
            {
                new BranchesAndBoundariesStrategy(""),
                new BranchesAndBoundariesStrategy("2"),
                new BranchesAndBoundariesStrategy("3"),
                new BranchesAndBoundariesStrategy("4"),
                new BranchesAndBoundariesStrategy("5")
            };
        }

        protected override bool IsRegenerated()
        {
            return true;
        }
    }
}