namespace KnapsackProblem
{
    public class ExecutionResultDto
    {
        public int TaskSize { get; set; }
        public string StrategyId { get; set; }
        public double Duration { get; set; }
        public double RelativeError { get; set; }
        public double MaxError { get; set; }

        public ExecutionResultDto(int taskSize, string strategyId, double duration, double relativeError, double maxError)
        {
            TaskSize = taskSize;
            StrategyId = strategyId;
            Duration = duration;
            RelativeError = relativeError;
            MaxError = maxError;
        }
    }
}