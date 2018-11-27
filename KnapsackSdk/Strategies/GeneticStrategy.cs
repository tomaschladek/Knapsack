using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies
{
    public class GeneticStrategy : AbstractStrategy
    {
        public GeneticStrategy(int generationCount, int populationSize, int mutationProbability, int crossoverProbability, ECrossStrategy crossStrategy)
        {
            Generations = generationCount;
            PopulationSize = populationSize;
            MutationProbability = mutationProbability;
            CrossoverProbability = crossoverProbability;
            CrossStrategy = crossStrategy;
        }

        public ECrossStrategy CrossStrategy { get; set; }

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
                var score = GetScore(definition, generation);

                var generationSelection = GetSelection(random, generation, score);
                var generationNew = GetNewGeneration(definition, random, generationSelection);

                Mutation(random, generationNew);

                generation = generationNew;
                var currentResult = GetResult(definition, generation);
                Debug.WriteLine($"{generationIndex}\t{currentResult.Item1}\t{currentResult.Item2}");
            }

            var result = GetResult(definition, generation);
            return (new ResultDto(definition.Id, result.Item1, result.Item3), 0L);
        }

        private static (long, long, IList<bool>) GetResult(DefinitionDto definition, List<IList<bool>> generation)
        {
            var candidates = generation
                .Select(generationItem => (generationItem
                        .Zip(definition.Items, (isPresent, item) => isPresent ? item : null), generationItem))
                .Select(item => (item.Item1.Sum(itemL => itemL?.Price ?? 0), item.Item1.Sum(itemL => itemL?.Weight ?? 0), item.Item2))
                .Where(item => item.Item2 <= definition.Capacity)
                .ToList();
            if (!candidates.Any())
            {
                return (0, 0, Enumerable.Repeat(false, definition.Items.Count).ToList());
            }
            var maxPrice = candidates.Max(item => item.Item1);
            var result = candidates.First(item => item.Item1 == maxPrice);
            return result;
        }

        private void Mutation(Random random, List<IList<bool>> generationNew)
        {
            foreach (var fenotyp in generationNew)
            {
                for (int fenotypIndex = 0; fenotypIndex < fenotyp.Count; fenotypIndex++)
                {
                    if (random.Next(0, 100) < MutationProbability)
                    {
                        fenotyp[fenotypIndex] = !fenotyp[fenotypIndex];
                    }
                }
            }
        }

        private List<IList<bool>> GetNewGeneration(DefinitionDto definition, Random random, List<IList<bool>> generationSelection)
        {
            switch (CrossStrategy)
            {
                case ECrossStrategy.Random:
                    return CrossRandom(definition, random, generationSelection).ToList();
                case ECrossStrategy.Single:
                    return CrossSingle(definition, random, generationSelection).ToList();
                case ECrossStrategy.Double:
                    return CrossDouble(definition, random, generationSelection).ToList();
            }
            throw new NotImplementedException();
        }

        private IEnumerable<IList<bool>> CrossSingle(DefinitionDto definition, Random random, List<IList<bool>> generationSelection)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<IList<bool>> CrossDouble(DefinitionDto definition, Random random, List<IList<bool>> generationSelection)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<IList<bool>> CrossRandom(DefinitionDto definition, Random random, List<IList<bool>> generationSelection)
        {
            for (int populationIndex = 0; populationIndex < PopulationSize; populationIndex += 2)
            {
                var randomVector = new List<bool>();
                for (int itemIndex = 0; itemIndex < definition.Items.Count; itemIndex++)
                {
                    randomVector.Add(random.Next(0, 100) > CrossoverProbability);
                }

                var first = new List<bool>();
                var second = new List<bool>();

                for (var crossoverIndex = 0; crossoverIndex < randomVector.Count; crossoverIndex++)
                {
                    if (randomVector[crossoverIndex])
                    {
                        first.Add(generationSelection[populationIndex][crossoverIndex]);
                        second.Add(generationSelection[populationIndex + 1][crossoverIndex]);
                    }
                    else
                    {
                        first.Add(generationSelection[populationIndex + 1][crossoverIndex]);
                        second.Add(generationSelection[populationIndex][crossoverIndex]);
                    }
                }
                yield return first;
                yield return second;
            }
        }

        private List<IList<bool>> GetSelection(Random random, List<IList<bool>> generation, List<long> score)
        {
            var sumScore = score.Sum();
            var generationSelection = new List<IList<bool>>();
            for (int newGenerationIndex = 0; newGenerationIndex < PopulationSize; newGenerationIndex++)
            {
                var randomValue = random.Next(0, (int)sumScore);
                var sumValue = 0L;
                var counter = -1;
                do
                {
                    counter++;
                    sumValue += score[counter];
                } while (sumValue <= randomValue);
                generationSelection.Add(generation[counter]);
            }

            return generationSelection;
        }

        private List<long> GetScore(DefinitionDto definition, List<IList<bool>> generation)
        {
            var summary = new List<ItemDto>();
            foreach (var fenotyp in generation)
            {
                var items = fenotyp.Zip(definition.Items, (isPresent, item) => isPresent ? item : null).Where(item => item != null).ToList();
                summary.Add(new ItemDto(items.Sum(item => item.Price), items.Sum(item => item.Weight)));
            }

            var score = new List<long>();
            for (int fenotypIndex = 0; fenotypIndex < PopulationSize; fenotypIndex++)
            {
                if (summary[fenotypIndex].Weight > definition.Capacity)
                {
                    score.Add(summary[fenotypIndex].Price -
                              (long)((double)definition.Capacity / summary[fenotypIndex].Weight));
                }
                else
                {
                    score.Add(summary[fenotypIndex].Price);
                }
            }

            return score;
        }

        private List<IList<bool>> InitializeGeneration(DefinitionDto definition, Random random)
        {
            var generation = new List<IList<bool>>();
            for (int generationIndex = 0; generationIndex < PopulationSize; generationIndex++)
            {
                generation.Add(new List<bool>());
                for (int itemIndex = 0; itemIndex < definition.Items.Count; itemIndex++)
                {
                    generation.Last().Add(random.Next(0, 100) > 50);
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