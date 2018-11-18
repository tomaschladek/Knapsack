using System.Collections.Generic;
using KnapsackSdk.Dtos;

namespace KnapsackProblem.Experiments.Error
{
    public class MaxPriceErrorExperiment : AbstractErrorExperiment
    {
        public MaxPriceErrorExperiment(int? weight)
        {
            Weight = weight;
        }

        private int? Weight { get; set; }

        public override string Id => "ErrorMaxPrice";
        protected override IEnumerable<int> GetParameters()
        {
            return new[]{10,50,75,100,125,150,175,200};
        }

        protected override DefinitionDto GenerateDefinition(int parameter)
        {
            return Generator.Generate(10, () => 100, index => Weight.HasValue ? Weight.Value : Random.Next(0,50), index => Random.Next(1, parameter));
        }

        protected override string Suffix => Weight?.ToString();
    }

    public class MaxPriceRatioErrorExperiment : AbstractErrorExperiment
    {
        public MaxPriceRatioErrorExperiment(int? weight)
        {
            Weight = weight;
        }

        private int? Weight { get; set; }

        public override string Id => "ErrorMaxPriceRatio";
        protected override IEnumerable<int> GetParameters()
        {
            return new[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
        }

        protected override DefinitionDto GenerateDefinition(int parameter)
        {
            return Generator.Generate(10, () => 100, index => Weight ?? Random.Next(1,50), index => Random.Next(1, 100) > parameter ? Random.Next(100, 200) : Random.Next(1, 100));
        }

        protected override string Suffix => Weight.ToString();
    }
}