using System.Collections.Generic;
using System.IO;
using System.Linq;
using KnapsackSdk.Strategies;

namespace KnapsackProblem
{
    class Program
    {
        public const string SourcePath =
            @"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\PAA\Knapsack\";

        static void Main()
        {
            var taskSizes = new[] { 4, 10, 15, 20, 22, 25, 27, 30, 32, 35, 37, 40 };
            var strategies = new List<IStrategy>
            {
                //new IterativeStrategy(),
                //new RecursiveStrategy(),
                //new RatioStrategy(),
                new BranchesAndBoundariesStrategy(),
                //new DecompositionByWeightStrategy(),
                //new DecompositionByPriceStrategy(),
                //new DecompositionByWeightRecursiveStrategy(),
                //new FtpasStrategy(2),
                //new FtpasStrategy(4),
                //new FtpasStrategy(8),
                //new FtpasStrategy(16),
            };

            ExecuteTasksOnStrategies(taskSizes, strategies,10, true);

        }

        private static void ExecuteTasksOnStrategies(int[] taskSizes, List<IStrategy> strategies, int repetition,
            bool logResults)
        {
            IFileManager fileManager = new FileManager();
            IExecutor executor = new Executor();

            foreach (var taskSize in taskSizes)
            {
                var definitions = fileManager.GetDefinitions(Path.Combine(SourcePath, "Input", $"knap_{taskSize}.inst.dat")).ToList();
                var results = fileManager.GetResults(Path.Combine(SourcePath, "Output", $"knap_{taskSize}.sol.dat")).ToList();

                foreach (var strategy in strategies)
                {
                    var result = executor.ExecuteStrategy(definitions, results, strategy, repetition);
                    if (logResults)
                    {
                        fileManager.AppendResult(Path.Combine(SourcePath, "result2018.csv"), taskSize, result);
                    }
                }
            }
        }
    }
}
