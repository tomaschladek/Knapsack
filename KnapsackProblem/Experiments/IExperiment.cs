namespace KnapsackProblem.Experiments
{
    interface IExperiment
    {
        string Id { get; }
        void Execute(string sourcePath);
    }
}