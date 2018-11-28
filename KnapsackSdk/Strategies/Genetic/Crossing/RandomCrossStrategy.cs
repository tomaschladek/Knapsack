using System;
using System.Collections;
using System.Collections.Generic;
using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies.Genetic.Crossing
{
    public class RandomCrossStrategy : ICrossStrategy
    {
        public IEnumerable<BitArray> Cross(DefinitionDto definition, Random random, List<BitArray> generation,
            int populationSize, int crossoverProbability)
        {
            for (int populationIndex = 0; populationIndex < populationSize; populationIndex += 2)
            {
                var randomVector = new BitArray(definition.Items.Count);
                for (int itemIndex = 0; itemIndex < definition.Items.Count; itemIndex++)
                {
                    randomVector.Set(itemIndex,random.Next(0, 100) > crossoverProbability);
                }

                var first = new BitArray(definition.Items.Count);
                var second = new BitArray(definition.Items.Count);

                for (var crossoverIndex = 0; crossoverIndex < randomVector.Count; crossoverIndex++)
                {
                    if (randomVector[crossoverIndex])
                    {
                        first.Set(crossoverIndex,generation[populationIndex][crossoverIndex]);
                        second.Set(crossoverIndex,generation[populationIndex + 1][crossoverIndex]);
                    }
                    else
                    {
                        first.Set(crossoverIndex, generation[populationIndex+1][crossoverIndex]);
                        second.Set(crossoverIndex, generation[populationIndex][crossoverIndex]);
                    }
                }
                yield return first;
                yield return second;
            }
        }
    }
}