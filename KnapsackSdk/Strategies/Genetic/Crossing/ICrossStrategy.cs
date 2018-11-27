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

    public class DoubleCrossStrategy : ICrossStrategy
    {
        public IEnumerable<BitArray> Cross(DefinitionDto definition, Random random, List<BitArray> generation,
            int populationSize,
            int crossoverProbability)
        {
            for (var fenotypIndex = 0; fenotypIndex < generation.Count; fenotypIndex += 2)
            {
                var cutFrom = random.Next(0, definition.Items.Count);
                var cutTo = random.Next(0, definition.Items.Count);
                var first = new BitArray(generation[fenotypIndex]);
                var second = new BitArray(generation[fenotypIndex + 1]);

                var counter = cutFrom;
                while (counter != cutTo)
                {
                    var temp = generation[fenotypIndex][counter];
                    first[counter] = second[counter];
                    second[counter] = temp;
                    counter = (counter + 1) % first.Count;
                }

                yield return first;
                yield return second;
            }
        }
    }

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