using System.Collections.Generic;
using System.IO;
using System.Linq;
using KnapsackSdk.Strategies;
using KnapsackSdk.Strategies.Genetic.Corrections;
using KnapsackSdk.Strategies.Genetic.Crossing;
using KnapsackSdk.Strategies.Genetic.Selections;

namespace KnapsackProblem.Experiments.Performance
{
    public class GeneticAlgorithmSelectionExperiment : AbstractExperiment<ExecutionResultDto>
    {
        public override string Id => "GA_Duration_Selection";

        protected override void Execute(string sourcePath, Dictionary<string, IList<ExecutionResultDto>> cache)
        {
            var definitions = GetDefinitions(sourcePath, 30);
            var results = FileManager.GetResults(Path.Combine(sourcePath, "Output", $"knap_30.sol.dat")).ToList();

            foreach (var unused in GetParameters())
            {
                foreach (var strategy in GetStrategies())
                {
                    var result = Executor.ExecuteStrategy(definitions, results, strategy, 10);
                    cache[strategy.Id].Add(result);
                }
            }
        }

        protected override IEnumerable<int> GetParameters()
        {
            return new[] { 1 };
        }

        protected override IEnumerable<IStrategy> GetStrategies()
        {
            var generations = new[] { 50, 100, 150, 200 };
            var populations = new[] { 10, 20, 30, 50 };
            var mutationProbabilities = new[] { 1 };
            var crossStrategies = new ICrossStrategy[] {new DoubleCrossStrategy() };
            var selectionStrategies = new ISelectionStrategy[]
            {
                new TournamentSelectionStrategy(5,5,5,new RatioCorrectionStrategy()),
                new TournamentSelectionStrategy(10,5,5,new RatioCorrectionStrategy()),
                new FitnessSelectionStrategy(5,5,new RatioCorrectionStrategy()),
                new RankingSelectionStrategy(5,5,new RatioCorrectionStrategy()),
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