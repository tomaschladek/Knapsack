using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using KnapsackSdk.Dtos;
using KnapsackSdk.Strategies.Genetic;
using KnapsackSdk.Strategies.Genetic.Crossing;
using KnapsackSdk.Strategies.Genetic.Selections;

namespace KnapsackSdk.Strategies
{
    public class GeneticStrategy : AbstractStrategy
    {
        public GeneticStrategy(int generationCount, int populationSize, int mutationProbability,
            int crossoverProbability, ICrossStrategy crossStrategy, ISelectionStrategy selectionStrategy)
        {
            Generations = generationCount;
            PopulationSize = populationSize;
            MutationProbability = mutationProbability;
            CrossoverProbability = crossoverProbability;
            CrossStrategy = crossStrategy;
            SelectionStrategy = selectionStrategy;
        }

        public ICrossStrategy CrossStrategy { get; set; }
        public ISelectionStrategy SelectionStrategy { get; set; }

        private int PopulationSize { get; }
        private int Generations { get;}
        private int MutationProbability { get; }
        private int CrossoverProbability { get; }

        public override (ResultDto, long) Compute(DefinitionDto definition)
        {
            var random = new Random();
            var generation = InitializeGeneration(definition, random);

            for (var generationIndex = 0; generationIndex < Generations; generationIndex++)
            {
                var generationSelection = SelectionStrategy.Select(definition, random, generation, PopulationSize).ToList();
                var generationNew = CrossStrategy.Cross(definition, random, generationSelection, PopulationSize, CrossoverProbability).ToList();

                Mutation(random, generationNew);

                generation = generationNew;
                var currentResult = GetResult(definition, generation);
                Debug.WriteLine($"{generationIndex}\t{currentResult.Item1.Price}\t{currentResult.Item1.Weight}");
            }

            var result = GetResult(definition, generation);
            var resultBools = new List<bool>();
            for (int index = 0; index < result.Item2.Count; index++)
            {
                resultBools.Add(result.Item2[index]);
            }
            return (new ResultDto(definition.Id, result.Item1.Price, resultBools), 0L);
        }

        private (ItemDto, BitArray) GetResult(DefinitionDto definition, List<BitArray> generation)
        {
            var candidates = generation
                .Select(generationItem => (GetScoreItem(generationItem,definition),generationItem))
                .Where(item => item.Item1.Weight <= definition.Capacity)
                .ToList();
            if (!candidates.Any())
            {
                return (new ItemDto(0,0), new BitArray(definition.Items.Count));
            }
            var maxPrice = candidates.Max(item => item.Item1.Price);
            var result = candidates.First(item => item.Item1.Price == maxPrice);
            return result;
        }

        private void Mutation(Random random, List<BitArray> generationNew)
        {
            foreach (var fenotyp in generationNew)
            {
                for (int fenotypIndex = 0; fenotypIndex < fenotyp.Count; fenotypIndex++)
                {
                    if (random.Next(0, 100) < MutationProbability)
                    {
                        fenotyp.Set(fenotypIndex,!fenotyp[fenotypIndex]);
                    }
                }
            }
        }

        private ItemDto GetScoreItem(BitArray fenotyp, DefinitionDto definition)
        {
            var price = 0L;
            var weight = 0L;
            for (int index = 0; index < fenotyp.Count; index++)
            {
                if (fenotyp[index])
                {
                    price += definition.Items[index].Price;
                    weight += definition.Items[index].Weight;
                }
            }
            return new ItemDto(weight,price);
        }

        private List<BitArray> InitializeGeneration(DefinitionDto definition, Random random)
        {
            var generation = new List<BitArray>();
            for (int generationIndex = 0; generationIndex < PopulationSize; generationIndex++)
            {
                generation.Add(new BitArray(definition.Items.Count));
                for (int itemIndex = 0; itemIndex < definition.Items.Count; itemIndex++)
                {
                    generation.Last().Set(itemIndex,random.Next(0, 100) > 50);
                }
            }

            return generation;
        }

        public override string Id => "Genetic";
    }

    public enum ECrossStrategy
    {
        Single,Double,Random
    }
}