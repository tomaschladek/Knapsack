using System;
using System.Collections;
using System.Collections.Generic;

namespace KnapsackSdk.Strategies.Genetic.Crossing
{
    public interface ICrossStrategy
    {
        IEnumerable<BitArray> Cross(int vectorSize, Random random,
            List<BitArray> generation, int populationSize, int crossoverProbability);

        string Id { get; }
    }
}