using System.Collections;
using System.Collections.Generic;
using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies.Genetic.Corrections
{
    public interface ICorrectionStrategy
    {
        IEnumerable<BitArray> CorrectGeneration(DefinitionDto definition, List<BitArray> generation);
    }
}