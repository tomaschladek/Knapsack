using System.Collections.Generic;
using System.IO;
using System.Linq;
using KnapsackSdk.Strategies;
using KnapsackSdk.Strategies.Genetic.Corrections;
using KnapsackSdk.Strategies.Genetic.Crossing;
using KnapsackSdk.Strategies.Genetic.Selections;

namespace KnapsackProblem.Experiments.Performance
{
    public class GeneticAlgorithmCrossExperiment : AbstractExperiment<ExecutionResultDto>
    {
        public override string Id => "GA_Duration_Crosss";

        protected override void Execute(string sourcePath, Dictionary<string, IList<ExecutionResultDto>> cache)
        {
            var definitions = GetDefinitions(sourcePath, 30);
            var results = FileManager.GetResults(Path.Combine(sourcePath, "Output", $"knap_30.sol.dat")).ToList();

            foreach (var taskSize in GetParameters())
            {
                foreach (var strategy in GetStrategies())
                {
                    ((GeneticStrategy) strategy).Generations = taskSize;
                    var result = Executor.ExecuteStrategy(definitions, results, strategy, 10);
                    if (!cache.ContainsKey(strategy.Id))
                    {
                        cache.Add(strategy.Id, new List<ExecutionResultDto>());
                    }
                    cache[strategy.Id].Add(result);
                }
            }
        }

        protected override IEnumerable<int> GetParameters()
        {
            return new[] { 50, 100};
        }

        protected override IEnumerable<IStrategy> GetStrategies()
        {
            var generations = new[] { 50, 100};
            var populations = new[] { 10, 20, 30, 50, 100 };
            var mutationProbabilities = new[] { 1 };
            var crossStrategies = new ICrossStrategy[] { new SingleCrossStrategy(), new DoubleCrossStrategy(), new RandomCrossStrategy() };
            var selectionStrategies = new ISelectionStrategy[]
            {
                new FitnessSelectionStrategy(5,5,new RatioCorrectionStrategy()),
            };
            foreach (var generation in generations)
            {
                foreach (var population in populations)
                {
                    foreach (var mutationProbability in mutationProbabilities)
                    {
                        foreach (var crossStrategy in crossStrategies)
                        {
                            foreach (var selectionStrategy in selectionStrategies)
                            {
                                yield return new GeneticStrategy(generation, population, mutationProbability, 50, crossStrategy,
                                    selectionStrategy);
                            }
                        }
                    }
                }
            }
        }
    }
}