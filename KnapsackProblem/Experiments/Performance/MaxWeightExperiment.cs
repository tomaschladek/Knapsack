using System.Collections.Generic;
using KnapsackSdk.Dtos;
using KnapsackSdk.Strategies;

namespace KnapsackProblem.Experiments.Performance
{
    public class MaxWeightExperiment : AbstractGeneratorExperiment
    {
        public override string Id => "MaxWeight";

        protected override DefinitionDto GenerateDefinitions(int parameter)
        {
            return Generator.Generate(10, () => 100, index => Random.Next(1, parameter), index => 50);
        }

        protected override IEnumerable<int> GetParameters()
        {
            return new[] {10, 20, 30, 40, 50, 60, 70, 80, 90, 100};
            //return new[] {30, 40, 50};
        }

        protected override IEnumerable<IStrategy> GetStrategies()
        {
            return new List<IStrategy>
            {
                new BranchesAndBoundariesStrategy(),
                new DecompositionByWeightRecursiveStrategy()
            };
        }
    }

    public class MaxWeightInverseExperiment : AbstractGeneratorExperiment
    {
        public override string Id => "MaxWeightInverse";

        protected override DefinitionDto GenerateDefinitions(int parameter)
        {
            var capacity = 100;
            return Generator.Generate(10, () => capacity, index => capacity - Random.Next(1, parameter), index => 50);
        }

        protected override IEnumerable<int> GetParameters()
        {
            return new[] {10, 20, 30, 40, 50, 60, 70, 80, 90, 100};
            //return new[] {30, 40, 50};
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
                new DecompositionByWeightRecursiveStrategy("10")
            };
        }
    }
}