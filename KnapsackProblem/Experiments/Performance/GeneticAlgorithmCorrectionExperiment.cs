using System.Collections.Generic;
using KnapsackSdk.Strategies;
using KnapsackSdk.Strategies.Genetic.Corrections;
using KnapsackSdk.Strategies.Genetic.Crossing;
using KnapsackSdk.Strategies.Genetic.Selections;

namespace KnapsackProblem.Experiments.Performance
{
    public class GeneticAlgorithmCorrectionExperiment : GeneticAlgorithmExperiment
    {
        public override string Id => "GA_Corrections_corrections";

        protected override IEnumerable<IStrategy> GetStrategies()
        {
            var generations = new[] { 50, 100, 150, 200, 300, 500 };
            var populations = new[] { 10, 20, 30, 50, 100 };
            var mutationProbabilities = new[] { 1 };
            var crossStrategies = new ICrossStrategy[] { new DoubleCrossStrategy()};
            var selectionStrategies = new ISelectionStrategy[]
            {
                new FitnessSelectionStrategy(5,5,new NoCorrectionStrategy()),
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
                                yield return new GeneticStrategy(generation, population, mutationProbability, 50, crossStrategy, selectionStrategy);
                            }
                        }
                    }
                }
            }
        }
    }
}