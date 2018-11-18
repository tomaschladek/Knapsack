using System.Collections.Generic;
using KnapsackSdk.Dtos;
using KnapsackSdk.Strategies;

namespace KnapsackProblem.Experiments.Performance
{
    public class CapacityOnlyExperiment : AbstractGeneratorExperiment
    {
        public override string Id => "CapacityOnly";

        protected override DefinitionDto GenerateDefinitions(int parameter)
        {
            return Generator.Generate(10, () => parameter, index => Random.Next(1, 100), index => 50);
        }

        protected override IEnumerable<int> GetParameters()
        {
            return new[] {25, 50, 75, 100, 125, 150, 175, 200};
        }

        protected override bool IsRegenerated()
        {
            return true;
        }

        protected override IEnumerable<IStrategy> GetStrategies()
        {
            return new List<IStrategy>
            {
                new BranchesAndBoundariesStrategy("2"),
                new BranchesAndBoundariesStrategy("3"),
                new BranchesAndBoundariesStrategy("4"),
                new BranchesAndBoundariesStrategy("5"),
                new BranchesAndBoundariesStrategy("6"),
                new BranchesAndBoundariesStrategy("7"),
                new BranchesAndBoundariesStrategy("8"),
                new BranchesAndBoundariesStrategy("9"),
                new BranchesAndBoundariesStrategy("10"),
                new DecompositionByWeightRecursiveStrategy("2"),
                new DecompositionByWeightRecursiveStrategy("3"),
                new DecompositionByWeightRecursiveStrategy("4"),
                new DecompositionByWeightRecursiveStrategy("5"),
                new DecompositionByWeightRecursiveStrategy("6"),
                new DecompositionByWeightRecursiveStrategy("7"),
                new DecompositionByWeightRecursiveStrategy("8"),
                new DecompositionByWeightRecursiveStrategy("9"),
                new DecompositionByWeightRecursiveStrategy("10"),
            };
        }
    }
}