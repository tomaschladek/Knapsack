using System;
using System.Collections.Generic;
using KnapsackSdk.Dtos;

namespace KnapsackProblem.Experiments
{
    public abstract class AbstractGeneratorExperiment : AbstractExperiment<long>
    {
        protected Random Random = new Random();
        public override string SourceFolder => throw new NotSupportedException();

        protected override void Execute(string sourcePath, Dictionary<string, IList<long>> results)
        {
            foreach (var parameter in GetParameters())
            {
                var definition = GenerateDefinitions(parameter);

                foreach (var strategy in GetStrategies())
                {
                    var result = strategy.Compute(definition);
                    results[strategy.Id].Add(result.Item2);
                }
            }
        }

        protected abstract DefinitionDto GenerateDefinitions(int parameter);
    }
}