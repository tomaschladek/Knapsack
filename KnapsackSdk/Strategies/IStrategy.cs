using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies
{
    public interface IStrategy
    {
        ResultDto Compute(DefinitionDto definition);
        string Id { get; }
    }
}