using System.Collections.Generic;
using KnapsackSdk.Dtos;

namespace KnapsackProblem.Experiments.Error
{
    public class RandomErrorExperiment : AbstractErrorExperiment
    {
        public override string Id => "ErrorRandom";
        protected override IEnumerable<int> GetParameters()
        {
            return new[] {200,200,200,200,200,200,200,200,200,200,200,200,200,200,200,200,200,200,200};
        }

        protected override DefinitionDto GenerateDefinition(int parameter)
        {
            return Generator.Generate(10, () => 100, index => Random.Next(1, 100), index => Random.Next(1, parameter));
        }

        protected override string Suffix => "";
    }
}