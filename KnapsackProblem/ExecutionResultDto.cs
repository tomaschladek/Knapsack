namespace KnapsackProblem
{
    public class ExecutionResultDto
    {
        public string StrategyId { get; set; }
        public double Duration { get; set; }
        public double RelativeError { get; set; }
        public double MaxError { get; set; }

        public ExecutionResultDto(string strategyId, double duration, double relativeError, double maxError)
        {
            StrategyId = strategyId;
            Duration = duration;
            RelativeError = relativeError;
            MaxError = maxError;
        }
    }
}