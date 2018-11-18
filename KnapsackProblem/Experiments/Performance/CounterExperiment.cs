namespace KnapsackProblem.Experiments.Performance
{
    public class CounterExperiment : AbstractCounterExperiment
    {
        public override string Id { get; }
        public override string SourceFolder { get; }

        public CounterExperiment(string id)
        {
            Id = SourceFolder = id;
        }
    }
}