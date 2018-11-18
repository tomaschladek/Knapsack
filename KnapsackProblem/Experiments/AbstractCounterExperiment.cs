using System.Collections.Generic;
using System.Linq;
using KnapsackSdk.Strategies;

namespace KnapsackProblem.Experiments
{
    public abstract class AbstractCounterExperiment : AbstractExperiment<long>
    {
        protected override void Execute(string sourcePath, Dictionary<string, IList<long>> results)
        {
            foreach (var taskSize in GetParameters())
            {
                var definition = GetDefinitions(sourcePath, taskSize).First();

                foreach (var strategy in GetStrategies())
                {
                    if (IsSkipped(taskSize, strategy))
                    {
                        continue;
                    }
                    var result = strategy.Compute(definition);
                    results[strategy.Id].Add(result.Item2);
                }
            }
        }

        protected virtual bool IsSkipped(int taskSize, IStrategy strategy)
        {
            return false;
        }

        protected override IEnumerable<int> GetParameters()
        {
            return new[] { 4, 10, 15, 20, 22, 25, 27, 30, 32, 35, 37, 40 };
        }
    }
}