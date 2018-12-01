using System.Collections.Generic;
using System.IO;
using System.Linq;
using KnapsackSdk.Strategies;
using KnapsackSdk.Strategies.Genetic.Corrections;
using KnapsackSdk.Strategies.Genetic.Crossing;
using KnapsackSdk.Strategies.Genetic.Selections;

namespace KnapsackProblem.Experiments.Performance
{
    public class GeneticAlgorithmExperiment : AbstractExperiment<ExecutionResultDto>
    {
        public override string Id => "GA_Duration";

        protected override void Execute(string sourcePath, Dictionary<string, IList<ExecutionResultDto>> cache)
        {
            foreach (var taskSize in GetParameters())
            {
                var definitions = GetDefinitions(sourcePath, taskSize);
                var results = FileManager.GetResults(Path.Combine(sourcePath, "Output", $"knap_{taskSize}.sol.dat")).ToList();

                foreach (var strategy in GetStrategies())
                {
                    var result = Executor.ExecuteStrategy(definitions, results, strategy, 10);
                    cache[strategy.Id].Add(result);
                }
            }
        }

        protected override IEnumerable<int> GetParameters()
        {
            return new[] { 30, 32, 35, 37, 40 };
        }

        protected override IEnumerable<IStrategy> GetStrategies()
        {
            var generations = new []{50,100,150,200,300,500};
            var populations = new []{10,20,30,50,100};
            var mutationProbabilities = new []{1,2,3,5,10};
            var crossStrategies = new ICrossStrategy[]{new SingleCrossStrategy(),new DoubleCrossStrategy(), new RandomCrossStrategy()};
            var selectionStrategies = new ISelectionStrategy[]
            {
                new TournamentSelectionStrategy(5,5,5,new NoCorrectionStrategy()),
                new TournamentSelectionStrategy(10,5,5,new NoCorrectionStrategy()),
                new TournamentSelectionStrategy(5,5,5,new RatioCorrectionStrategy()),
                new TournamentSelectionStrategy(10,5,5,new RatioCorrectionStrategy()),
                new FitnessSelectionStrategy(5,5,new NoCorrectionStrategy()),
                new FitnessSelectionStrategy(5,5,new RatioCorrectionStrategy()),
                new RankingSelectionStrategy(5,5,new NoCorrectionStrategy()), 
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
                                yield return new GeneticStrategy(generation, population, mutationProbability, 50, crossStrategy, selectionStrategy);
                            }
                        }
                    }
                }
            }
        }
    }
}