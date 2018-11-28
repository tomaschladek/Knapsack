﻿using System;
using System.Collections;
using System.Collections.Generic;
using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies.Genetic.Crossing
{
    public class DoubleCrossStrategy : ICrossStrategy
    {
        public IEnumerable<BitArray> Cross(DefinitionDto definition, Random random, List<BitArray> generation,
            int populationSize,
            int crossoverProbability)
        {
            for (var fenotypIndex = 0; fenotypIndex < generation.Count-1; fenotypIndex += 2)
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
}