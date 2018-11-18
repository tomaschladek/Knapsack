using System.Collections.Generic;
using System.Linq;
using KnapsackSdk.Dtos;
using KnapsackSdk.Strategies;

namespace KnapsackProblem.Experiments.Error
{
    public abstract class AbstractErrorExperiment : AbstractExperiment<double>
    {
        protected override void Execute(string sourcePath, Dictionary<string, IList<double>> cache)
        {
            foreach (var parameter in GetParameters())
            {
                var definitions = GenerateDefinitions(parameter).ToList();
                var results = definitions
                    .Select(definition => new IterativeStrategy().Compute(definition).Item1).ToList();

                foreach (var strategy in GetStrategies())
                {
                    if (IsRegenerated())
                    {
                        definitions = GenerateDefinitions(parameter).ToList();
                        results = definitions
                            .Select(definition => new DecompositionByWeightStrategy().Compute(definition).Item1).ToList();
                    }
                    var result = Executor.ExecuteStrategy(definitions, results,strategy,1);
                    cache[strategy.Id].Add(result.MaxError*100);
                }
            }
        }

        protected override IEnumerable<IStrategy> GetStrategies()
        {
            return new List<IStrategy>
            {
                new RatioStrategy($"-{Suffix}"),
                new FtpasStrategy(2,$"-{Suffix}"),
                new FtpasStrategy(4,$"-{Suffix}"),
                new FtpasStrategy(8,$"-{Suffix}"),
                new FtpasStrategy(16,$"-{Suffix}"),
            };
        }

        protected virtual bool IsRegenerated()
        {
            return false;
        }

        protected IEnumerable<DefinitionDto> GenerateDefinitions(int parameter)
        {
            for (int index = 0; index < 100; index++)
            {
                yield return GenerateDefinition(parameter);
            }
        }

        protected abstract DefinitionDto GenerateDefinition(int parameter);
        protected abstract string Suffix { get; }
    }
}