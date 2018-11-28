using System;
using System.Collections;
using System.Collections.Generic;
using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies.Genetic.Crossing
{
    public interface ICrossStrategy
    {
        IEnumerable<BitArray> Cross(DefinitionDto definition, Random random,
            List<BitArray> generation, int populationSize, int crossoverProbability);
    }
}