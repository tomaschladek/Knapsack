using KnapsackProblem.Dtos;

namespace KnapsackProblem.Strategies
{
    public interface IStrategy
    {
        ResultDto Compute(DefinitionDto definition);
        string Id { get; }
    }
}