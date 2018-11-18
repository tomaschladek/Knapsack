using System;
using System.Collections.Generic;
using KnapsackSdk.Dtos;

namespace KnapsackProblem.Experiments.Error
{
    public class PriceWeightRatioErrorExperiment : AbstractErrorExperiment
    {
        public PriceWeightRatioErrorExperiment(int offset)
        {
            Offset = offset;
            Suffix = $"{Offset}";
        }

        private int Offset { get; set; }

        public override string Id => "ErrorPriceWeightRatio";
        protected override IEnumerable<int> GetParameters()
        {
            return new[] { 25,50,75,100,125,150,175,200 };
        }

        protected override DefinitionDto GenerateDefinition(int parameter)
        {
            var weight = 0;
            return Generator.Generate(10, () => 100, index =>
            {
                weight = Random.Next(1, 100);
                return weight;
            }, index => Math.Max(0,weight + Random.Next(-parameter+Offset, parameter+Offset)));
        }

        protected override string Suffix { get; }
    }
}