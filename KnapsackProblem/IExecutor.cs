using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using KnapsackSdk.Dtos;
using KnapsackSdk.Strategies;

namespace KnapsackProblem
{
    public interface IExecutor
    {
        ExecutionResultDto ExecuteStrategy(IList<DefinitionDto> definitions, IList<ResultDto> results,
            IStrategy strategy, int repetition);
    }

    public class Executor : IExecutor
    {

        public ExecutionResultDto ExecuteStrategy(IList<DefinitionDto> definitions, IList<ResultDto> results,
            IStrategy strategy, int repetition)
        {
            var duration = ExecuteStrategyWithStopWatch(definitions, results, strategy, repetition, out var relativeErrorSum, out var maxError);

            return new ExecutionResultDto(strategy.Id, duration, relativeErrorSum, maxError);
        }

        private static double ExecuteStrategyWithStopWatch(IList<DefinitionDto> definitions, IList<ResultDto> results, IStrategy strategy, int repetition, out double relativeErrorSum, out double maxError)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            ExecuteStrategy(definitions, results, strategy, repetition, out relativeErrorSum, out maxError);

            stopwatch.Stop();

            return stopwatch.Elapsed.TotalMilliseconds / repetition;
        }

        private static void ExecuteStrategy(IList<DefinitionDto> definitions, IList<ResultDto> results, IStrategy strategy, int repetition, out double relativeErrorSum, out double maxError)
        {
            IList<double> errors = null;
            for (int repetitionIndex = 0; repetitionIndex < repetition; repetitionIndex++)
            {
                errors = definitions.Zip(results, (definition, result) =>
                {
                    var computedResult = strategy.Compute(definition);

                    return (double) (result.Price - computedResult.Item1.Price) / result.Price;
                }).ToList();

            }
            relativeErrorSum = errors?.Sum() ?? 0;
            maxError = errors?.Max() ?? 0;
        }
    }
}