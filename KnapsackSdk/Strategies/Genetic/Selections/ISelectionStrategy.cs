using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies.Genetic.Selections
{
    public interface ISelectionStrategy
    {
        IEnumerable<BitArray> Select(DefinitionDto definition, Random random, List<BitArray> generation);
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

        public abstract IEnumerable<BitArray> Select(DefinitionDto definition, Random random, List<BitArray> generation);
    }

    public class ReplaceAllByPriceSelectionStrategy : AbstractSelectionStrategy
    {
        protected virtual int StartCount { get; set; }

        public override IEnumerable<BitArray> Select(DefinitionDto definition, Random random, List<BitArray> generation)
        {
            var score = GetScore(definition, generation).ToList();
            var sumScore = score.Sum();
            foreach (var _ in generation)
            {
                var randomValue = random.Next(0, (int) sumScore);
                var sumValue = 0L;
                var counter = -1;
                do
                {
                    counter++;
                    sumValue += score[counter];
                } while (sumValue <= randomValue);

                yield return generation[counter];
            }
        }

        private IEnumerable<long> GetScore(DefinitionDto definition, List<BitArray> generation)
        {
            foreach (var item in generation.Select(fenotyp => GetScoreItem(fenotyp, definition)))
            {
                yield return item.Weight > definition.Capacity
                    ? 0
                    : item.Price;
            }
        }
    }

    public class ReplaceAllByOrderSelectionStrategy : AbstractSelectionStrategy
    {
        public override IEnumerable<BitArray> Select(DefinitionDto definition, Random random, List<BitArray> generation)
        {
            var ordered = generation
                .Select((bits, index) => new {Bits = bits, OriginalIndex = index})
                .OrderBy(item => GetScoreItem(item.Bits, definition).Price)
                .Select((tuple,newIndex) => new {tuple.Bits, tuple.OriginalIndex, NewIndex = generation.Count - newIndex})
                .ToList();

            var sumOrder = ordered.Sum(item => item.NewIndex);

            for (int newGenerationIndex = 0; newGenerationIndex < generation.Count; newGenerationIndex++)
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