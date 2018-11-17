using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using KnapsackSdk.Dtos;
using KnapsackSdk.Strategies;

namespace KnapsackProblem
{
    public interface IExecutor
    {
        ExecutionResultDto ExecuteStrategy(int taskSize, IList<DefinitionDto> definitions, IList<ResultDto> results,
            IStrategy strategy, int repetition);
    }

    public class Executor : IExecutor
    {

        public ExecutionResultDto ExecuteStrategy(int taskSize, IList<DefinitionDto> definitions, IList<ResultDto> results,
            IStrategy strategy, int repetition)
        {
            var relativeErrorSum = 0d;
            var maxError = 0d;

            var duration = ExecuteStrategyWithStopWatch(definitions, results, strategy, repetition, ref relativeErrorSum, ref maxError);

            var relativeError = relativeErrorSum / (definitions.Count * repetition);

            return new ExecutionResultDto(taskSize, strategy.Id, duration, relativeError, maxError);
        }

        private static double ExecuteStrategyWithStopWatch(IList<DefinitionDto> definitions, IList<ResultDto> results, IStrategy strategy, int repetition, ref double relativeErrorSum, ref double maxError)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            ExecuteStrategy(definitions, results, strategy, repetition, ref relativeErrorSum, ref maxError);

            stopwatch.Stop();

            return stopwatch.Elapsed.TotalMilliseconds / repetition;
        }

        private static void ExecuteStrategy(IList<DefinitionDto> definitions, IList<ResultDto> results, IStrategy strategy, int repetition, ref double relativeErrorSum, ref double maxError)
        {
            for (int repetitionIndex = 0; repetitionIndex < repetition; repetitionIndex++)
            {
                var errors = definitions.Zip(results, (definition, result) =>
                {
                    var computedResult = strategy.Compute(definition);

                    return (double) (result.Price - computedResult.Price) / result.Price;
                }).ToList();

                relativeErrorSum = errors.Sum();
                maxError = errors.Max();
            }
        }
    }
}