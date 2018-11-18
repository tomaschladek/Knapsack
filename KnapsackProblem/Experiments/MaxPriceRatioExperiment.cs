﻿using System.Collections.Generic;
using KnapsackSdk.Dtos;
using KnapsackSdk.Strategies;

namespace KnapsackProblem.Experiments
{
    public class MaxPriceRatioExperiment : AbstractGeneratorExperiment
    {
        public override string Id => "MaxPriceRatio20";

        protected override DefinitionDto GenerateDefinitions(int parameter)
        {
            return Generator.Generate(10, () => 100, index => 20, index => Random.Next(1,100) > parameter ? Random.Next(100, 200) : Random.Next(1,100));
        }

        protected override IEnumerable<int> GetParameters()
        {
            return new[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
        }

        protected override IEnumerable<IStrategy> GetStrategies()
        {
            return new List<IStrategy>
            {
                new BranchesAndBoundariesStrategy(),
                new FtpasStrategy(16),
            };
        }
    }
}