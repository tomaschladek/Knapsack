using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies.Genetic.Selections
{
    public interface ISelectionStrategy
    {
        IEnumerable<BitArray> Select(DefinitionDto definition, Random random, List<BitArray> generation, 
            int populationSize);
    }

    public abstract class AbstractSelectionStrategy : ISelectionStrategy
    {

        protected ItemDto GetScoreItem(BitArray fenotyp, DefinitionDto definition)
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
            return new ItemDto(weight, price);
        }

        public abstract IEnumerable<BitArray> Select(DefinitionDto definition, Random random, List<BitArray> generation, int populationSize);
    }

    public class ReplaceAllByPriceySelectionStrategy : AbstractSelectionStrategy
    {
        public override IEnumerable<BitArray> Select(DefinitionDto definition, Random random, List<BitArray> generation, int populationSize)
        {
            var score = GetScore(definition, generation, populationSize);
            var sumScore = score.Sum();
            var generationSelection = new List<BitArray>();
            for (int newGenerationIndex = 0; newGenerationIndex < populationSize; newGenerationIndex++)
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


        private List<long> GetScore(DefinitionDto definition, List<BitArray> generation, int populationSize)
        {
            var summary = new List<ItemDto>();
            foreach (var fenotyp in generation)
            {
                summary.Add(GetScoreItem(fenotyp, definition));
            }

            var score = new List<long>();
            for (int fenotypIndex = 0; fenotypIndex < populationSize; fenotypIndex++)
            {
                score.Add(
                    summary[fenotypIndex].Weight > definition.Capacity 
                    ? 0 
                    : summary[fenotypIndex].Price);
            }

            return score;
        }
    }

    public class ReplaceAllByOrderSelectionStrategy : AbstractSelectionStrategy
    {
        public override IEnumerable<BitArray> Select(DefinitionDto definition, Random random, List<BitArray> generation, int populationSize)
        {
            var ordered = generation
                .Select((bits, index) => new {Bits = bits, OriginalIndex = index})
                .OrderBy(item => GetScoreItem(item.Bits, definition).Price)
                .Select((tuple,newIndex) => new {tuple.Bits, tuple.OriginalIndex, NewIndex = populationSize-newIndex}).ToList();
            var sumOrder = ordered.Sum(item => item.NewIndex);
            for (int newGenerationIndex = 0; newGenerationIndex < populationSize; newGenerationIndex++)
            {
                var randomValue = random.Next(0, sumOrder);
                var sumValue = 0L;
                var counter = -1;
                do
                {
                    counter++;
                    sumValue += ordered[counter].NewIndex;
                } while (sumValue <= randomValue);

                yield return generation[ordered[counter].OriginalIndex];
            }
        }
    }
}