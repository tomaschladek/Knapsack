using System;
using KnapsackSdk.Dtos;

namespace KnapsackProblem.Experiments
{
    public interface IGenerator
    {
        DefinitionDto Generate(int instanceSize, Func<long> getCapacity, Func<int, long> getWeight, Func<int, long> getPrice);
    }
}