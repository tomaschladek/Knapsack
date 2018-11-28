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
        public AbstractSelectionStrategy(int elitesCount, int weakestsCount)
        {
            ElitesCount = elitesCount;
            WeakestsCount = weakestsCount;
        }

        private int ElitesCount { get; set; }
        private int WeakestsCount { get; set; }
        protected int StartCount { get; set; }

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

        protected IEnumerable<BitArray> SelectElites(DefinitionDto definition, List<BitArray> generation)
        {
            var elites = generation
                .Select(fenotyp => new { Fenotyp = fenotyp, Score = GetScoreItem(fenotyp, definition) })
                .Where(tuple => tuple.Score.Weight <= definition.Capacity)
                .OrderByDescending(tuple => tuple.Score.Price)
                .Take(ElitesCount).ToArray();

            StartCount = elites.Length - WeakestsCount;

            return elites.Select(item => item.Fenotyp);
        }

        protected IEnumerable<BitArray> RemoveWeakests(DefinitionDto definition, List<BitArray> generation)
        {
            return generation
                .Select(fenotyp => new { Fenotyp = fenotyp, Score = GetScoreItem(fenotyp, definition) })
                .OrderBy(tuple => tuple.Score.Price)
                .Skip(WeakestsCount)
                .Select(item => item.Fenotyp)
                .ToArray();
        }


        public IEnumerable<BitArray> Select(DefinitionDto definition, Random random, List<BitArray> generation)
        {
            var elites = SelectElites(definition, generation).ToList();
            var childrenByScore = SelectByCriteria(definition, random, generation.ToList());
            var result = RemoveWeakests(definition,elites.Concat(childrenByScore).ToList());
            return result;
        }

        protected abstract IEnumerable<BitArray> SelectByCriteria(DefinitionDto definition, Random random, List<BitArray> toList);
    }

    public class FitnessSelectionStrategy : AbstractSelectionStrategy
    {
        public FitnessSelectionStrategy() : this(0,0)
        {
        }

        public FitnessSelectionStrategy(int elitesCount, int weakestsCount) : base(elitesCount, weakestsCount)
        {
        }

        protected override IEnumerable<BitArray> SelectByCriteria(DefinitionDto definition, Random random, List<BitArray> generation)
        {
            var score = GetScore(definition, generation).ToList();
            var sumScore = score.Sum();
            for (var newGenerationIndex = StartCount; newGenerationIndex < generation.Count; newGenerationIndex++)
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

    public class RankingSelectionStrategy : AbstractSelectionStrategy
    {

        public RankingSelectionStrategy() : this(0,0)
        {
        }

        public RankingSelectionStrategy(int elitesCount, int weakestsCount) : base(elitesCount,weakestsCount)
        {
        }
        
        protected override IEnumerable<BitArray> SelectByCriteria(DefinitionDto definition, Random random, List<BitArray> generation)
        {
            var ordered = generation
                .Select((bits, index) => new {Bits = bits, OriginalIndex = index})
                .OrderBy(item => GetScoreItem(item.Bits, definition).Price)
                .Select((tuple,newIndex) => new {tuple.Bits, tuple.OriginalIndex, NewIndex = generation.Count - newIndex})
                .ToList();

            var sumOrder = ordered.Sum(item => item.NewIndex);

            for (int newGenerationIndex = StartCount; newGenerationIndex < generation.Count; newGenerationIndex++)
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