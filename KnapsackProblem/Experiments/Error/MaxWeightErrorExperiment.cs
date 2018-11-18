using System.Collections.Generic;
using KnapsackSdk.Dtos;

namespace KnapsackProblem.Experiments.Error
{
    public class MaxWeightErrorExperiment : AbstractErrorExperiment
    {
        public MaxWeightErrorExperiment(int? price)
        {
            Price = price;
        }

        private int? Price { get; set; }

        public override string Id => $"ErrorMaxWeightFixedPrice";

        protected override IEnumerable<int> GetParameters()
        {
            return new[] { 5, 10, 15, 20, 25, 30,35,  40, 45,50, 55,60, 65, 70, 75,80, 85, 90, 95, 100 ,105};
        }

        protected override DefinitionDto GenerateDefinition(int parameter)
        {
            return Generator.Generate(10,() => 100, index => Random.Next(1,parameter), index => Price ?? Random.Next(1,300));
        }

        protected override string Suffix => Price.ToString();
    }

    public class MaxWeightRatioErrorExperiment : AbstractErrorExperiment
    {
        public MaxWeightRatioErrorExperiment(int? price)
        {
            Price = price;
        }

        private int? Price { get; set; }

        public override string Id => $"ErrorMaxWeightRatio";

        protected override IEnumerable<int> GetParameters()
        {
            return new[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
        }

        protected override DefinitionDto GenerateDefinition(int parameter)
        {
            return Generator.Generate(10, () => 100, index => Random.Next(1, 100) > parameter ? Random.Next(50, 100) : Random.Next(1, 50), index => Price ?? Random.Next(0,100));
        }

        protected override string Suffix => Price.ToString();
    }
}