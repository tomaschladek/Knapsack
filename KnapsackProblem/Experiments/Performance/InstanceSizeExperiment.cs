using KnapsackSdk.Strategies;

namespace KnapsackProblem.Experiments.Performance
{
    public class InstanceSizeExperiment : AbstractCounterExperiment
    {
        public override string SourceFolder => "Input";
        public override string Id => "InstanceSize";

        protected override bool IsSkipped(int taskSize, IStrategy strategy)
        {
            return taskSize > 20 && (strategy is IterativeStrategy || strategy is RecursiveStrategy);
        }
    }
}