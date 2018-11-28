using System;
using System.Collections;
using System.Collections.Generic;
using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies.Genetic.Crossing
{
    public class SingleCrossStrategy : ICrossStrategy
    {
        public IEnumerable<BitArray> Cross(DefinitionDto definition, Random random, List<BitArray> generation,
            int populationSize,
            int crossoverProbability)
        {
            for (var fenotypIndex = 0; fenotypIndex < generation.Count; fenotypIndex +=2)
            {
                var cut = random.Next(0, definition.Items.Count);
                var first = new BitArray(generation[fenotypIndex]);
                var second = new BitArray(generation[fenotypIndex+1]);
                for (int index = cut; index < first.Count; index++)
                {
                    var temp = generation[fenotypIndex][index];
                    first[index] = second[index];
                    second[index] = temp;
                }

                yield return first;
                yield return second;
            }
        }
    }
}