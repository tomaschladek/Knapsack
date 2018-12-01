using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KnapsackSdk.Dtos;
using KnapsackSdk.Strategies.Genetic.Corrections;

namespace KnapsackSdk.Strategies.Genetic.Selections
{
    public class FitnessSelectionStrategy : AbstractSelectionStrategy
    {
        public FitnessSelectionStrategy() : this(0,0, new NoCorrectionStrategy())
        {
        }

        public FitnessSelectionStrategy(int elitesCount, int weakestsCount, ICorrectionStrategy correctionStrategy) : base(elitesCount, weakestsCount,correctionStrategy)
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
}