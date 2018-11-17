using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies
{
    public interface IStrategy
    {
        (ResultDto, long) Compute(DefinitionDto definition);
        string Id { get; }
    }
}