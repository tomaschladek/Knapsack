using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using KnapsackProblem.Dtos;
using KnapsackProblem.Strategies;

namespace KnapsackProblem
{
    class Program
    {
        public const string SourcePath =
            "C:\\Users\\tomas.chladek\\Documents\\Personal\\Uni\\Master\\3rd\\PAA\\Knapsack\\";

        private const bool DebugFlag = true;

        static void Main()
        {
            var taskSizes = new[] { 4, 10, 15, 20, 22, 25, 27, 30, 32, 35, 37, 40 };
            //var taskSizes = new[] { 4, 10, 15, 22, 25 };
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

            ExecuteTasksOnStrategies(taskSizes, strategies,10);

        }

        private static void ExecuteTasksOnStrategies(int[] taskSizes, List<IStrategy> strategies, int repetition)
        {
            foreach (var taskSize in taskSizes)
            {
                var definitions = GetDefinitions(taskSize);
                var results = GetResults(taskSize);

                foreach (var strategy in strategies)
                {
                    ExecuteStrategy(taskSize, definitions, results, strategy, repetition);
                }
            }
        }

        private static void ExecuteStrategy(int taskSize, IList<DefinitionDto> definitions, IList<ResultDto> results,
            IStrategy strategy, int repetition)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            var relativeErrorSum = 0d;
            var maxError = 0d;
            for (int repetitionIndex = 0; repetitionIndex < repetition; repetitionIndex++)
            {
                for (int index = 0; index < definitions.Count; index++)
                {
                    var definition = definitions[index];
                    var result = results[index];
                    var computedResult = strategy.Compute(definition);
                    var error = (double)(result.Price - computedResult.Price) / result.Price;
                    if (error > maxError)
                    {
                        maxError = error;
                    }
                    relativeErrorSum += error;
                }
            }

            stopwatch.Stop();
            var duration = stopwatch.Elapsed.TotalMilliseconds/repetition;
            var relativeError = relativeErrorSum / (definitions.Count*repetition);

            if (DebugFlag)
            {
                File.AppendAllText(Path.Combine(SourcePath, "result2018.csv"),
                    $"{taskSize}\t{strategy.Id}\t{duration}\t{relativeError}\t{maxError}{Environment.NewLine}");
            }
        }

        private static IList<ResultDto> GetResults(int taskSize)
        {
            string line;
            var results = new List<ResultDto>();

            var file = new StreamReader(Path.Combine(SourcePath, "Output", $"knap_{taskSize}.sol.dat"));
            while ((line = file.ReadLine()) != null)
            {
                var result = GetResult(line);
                results.Add(result);
            }

            file.Close();
            return results;
        }

        private static ResultDto GetResult(string text)
        {
            var parsedValues = text.Split(' ').Where(value => !string.IsNullOrEmpty(value)).Select(Int32.Parse).ToList();
            var items = new List<bool>();
            for (int index = 0; index < parsedValues[1]; index++)
            {
                items.Add(parsedValues[3 + index]==1);
            }
            return new ResultDto(parsedValues[0], parsedValues[2], items);
        }

        private static IList<DefinitionDto> GetDefinitions(int taskSize)
        {
            string line;
            var definitions = new List<DefinitionDto>();

            var file = new StreamReader(Path.Combine(SourcePath, "Input", $"knap_{taskSize}.inst.dat"));
            while ((line = file.ReadLine()) != null)
            {
                var definition = GetDefinition(line);
                definitions.Add(definition);   
            }

            file.Close();
            return definitions;
        }

        private static DefinitionDto GetDefinition(string text)
        {
            var parsedValues = text.Split(' ').Select(Int32.Parse).ToList();
            var items = new List<ItemDto>();
            for (int index = 0; index < parsedValues[1]; index++)
            {
                var item = new ItemDto(parsedValues[3+index*2], parsedValues[4 + index * 2]);
                items.Add(item);
            }
            return new DefinitionDto(parsedValues[0],parsedValues[2],items);
        }
    }
}
