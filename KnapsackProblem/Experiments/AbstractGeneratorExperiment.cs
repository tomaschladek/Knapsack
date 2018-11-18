using System.Collections.Generic;
using KnapsackSdk.Dtos;

namespace KnapsackProblem.Experiments
{
    public abstract class AbstractGeneratorExperiment : AbstractExperiment<long>
    {
        protected override void Execute(string sourcePath, Dictionary<string, IList<long>> results)
        {
            foreach (var parameter in GetParameters())
            {
                var definition = GenerateDefinitions(parameter);

                foreach (var strategy in GetStrategies())
                {
                    definition = IsRegenerated() ? GenerateDefinitions(parameter) : definition;
                    var result = strategy.Compute(definition);
                    results[strategy.Id].Add(result.Item2);
                }
            }
        }

        protected virtual bool IsRegenerated()
        {
            return false;
        }

        protected abstract DefinitionDto GenerateDefinitions(int parameter);
    }
}