using System.Collections.Generic;
using KnapsackSdk.Dtos;

namespace KnapsackProblem.Experiments.Error
{
    public class InstanceSizeErrorExperiment : AbstractErrorExperiment
    {
        public override string Id => "InstanceSize";
        protected override IEnumerable<int> GetParameters()
        {
            return new[] {4, 8, 10, 12, 15, 17, 20, 22, 25, 27, 30, 32, 35, 37, 40};
        }

        protected override DefinitionDto GenerateDefinition(int parameter)
        {
            return Generator.Generate(parameter, () => 300, index => Random.Next(1, 300), index => Random.Next(1, 600));
        }

        protected override string Suffix => "";
    }
}