using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KnapsackProblem.Experiments
{
    public class DurationExperiment : AbstractExperiment<ExecutionResultDto>
    {
        public override string Id => "DurationEvaluation";
        public override string SourceFolder => "Input";

        protected override void Execute(string sourcePath, Dictionary<string, IList<ExecutionResultDto>> cache)
        {
            foreach (var taskSize in GetParameters())
            {
                var definitions = GetDefinitions(sourcePath,taskSize);
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
            return new[] { 4, 10, 15, 20, 22, 25, 27, 30, 32, 35, 37, 40 };
        }
    }
}