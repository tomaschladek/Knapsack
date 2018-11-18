using System.Collections.Generic;
using System.IO;
using System.Linq;
using KnapsackSdk.Strategies;

namespace KnapsackProblem.Experiments
{
    public abstract class AbstractExperiment<TCacheType> : IExperiment
    {
        public abstract string Id { get; }
        public abstract string SourceFolder { get; }

        public void Execute(string sourcePath)
        {
            var results = InitializeResultsCache<TCacheType>();
            Execute(sourcePath,results);
            PrintOutCache(results, sourcePath);
        }

        protected abstract void Execute(string sourcePath, Dictionary<string, IList<TCacheType>> results);

        protected readonly IFileManager FileManager = new FileManager();
        protected readonly IExecutor Executor = new Executor();
        protected readonly IGenerator Generator = new Generator();

        protected IList<IStrategy> Strategies = new List<IStrategy>
        {
            new IterativeStrategy(),
            new RecursiveStrategy(),
            new RatioStrategy(),
            new BranchesAndBoundariesStrategy(),
            new DecompositionByWeightStrategy(),
            new DecompositionByPriceStrategy(),
            new DecompositionByWeightRecursiveStrategy(),
            new FtpasStrategy(2),
            new FtpasStrategy(4),
            new FtpasStrategy(8),
            new FtpasStrategy(16),
        };

        protected List<KnapsackSdk.Dtos.DefinitionDto> GetDefinitions(string sourcePath, int taskSize)
        {
            return FileManager.GetDefinitions(Path.Combine(sourcePath, SourceFolder, $"knap_{taskSize}.inst.dat")).ToList();
        }


        protected abstract IEnumerable<int> GetParameters();




        private Dictionary<string, IList<TType>> InitializeResultsCache<TType>()
        {
            var results = new Dictionary<string, IList<TType>>();
            foreach (var strategy in GetStrategies())
            {
                results.Add(strategy.Id, new List<TType>());
            }

            return results;
        }

        protected virtual IEnumerable<IStrategy> GetStrategies()
        {
            return Strategies;
        }


        private void PrintOutCache<TType>(Dictionary<string, IList<TType>> results, string sourcePath)
        {
            var header = new List<string>{"Strategy"};
            header.AddRange(GetParameters().Select(item => item.ToString()));
            FileManager.AppendResult(Path.Combine(sourcePath, $"{Id}.csv"), header.ToArray());

            foreach (var result in results.Keys)
            {
                var parameters = new List<string>
                {
                    result
                };
                parameters.AddRange(results[result].Select(item => item.ToString()));
                FileManager.AppendResult(Path.Combine(sourcePath, $"{Id}.csv"), parameters.ToArray());
            }
        }

    }
}