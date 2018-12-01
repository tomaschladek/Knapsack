using System;
using System.Collections;
using System.Collections.Generic;
using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies.Genetic.Selections
{
    public interface ISelectionStrategy
    {
        IEnumerable<BitArray> Select(DefinitionDto definition, Random random, List<BitArray> generation);
        string Id { get; }
    }
}