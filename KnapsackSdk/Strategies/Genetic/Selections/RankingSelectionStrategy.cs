using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies.Genetic.Selections
{
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