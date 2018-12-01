using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KnapsackSdk.Dtos;
using KnapsackSdk.Strategies.Genetic.Corrections;

namespace KnapsackSdk.Strategies.Genetic.Selections
{
    public abstract class AbstractSelectionStrategy : ISelectionStrategy
    {
        public AbstractSelectionStrategy(int elitesCount, int weakestsCount, ICorrectionStrategy correctionStrategy)
        {
            ElitesCount = elitesCount;
            WeakestsCount = weakestsCount;
            CorrectionStrategy = correctionStrategy;
        }

        protected ICorrectionStrategy CorrectionStrategy { get; set; }

        protected int ElitesCount { get; set; }
        protected int WeakestsCount { get; set; }
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

        private IEnumerable<BitArray> SelectElites(DefinitionDto definition, List<BitArray> generation)
        {
            var elites = generation
                .Select(fenotyp => new { Fenotyp = fenotyp, Score = GetScoreItem(fenotyp, definition) })
                .Where(tuple => tuple.Score.Weight <= definition.Capacity)
                .OrderByDescending(tuple => tuple.Score.Price)
                .Take(ElitesCount).ToArray();

            StartCount = elites.Length - WeakestsCount;

            return elites.Select(item => item.Fenotyp);
        }

        private IEnumerable<BitArray> RemoveWeakests(DefinitionDto definition, List<BitArray> generation)
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
            var correctedGeneration = CorrectionStrategy.CorrectGeneration(definition, generation).ToList();
            var elites = SelectElites(definition, correctedGeneration).ToList();
            var childrenByScore = SelectByCriteria(definition, random, generation.ToList());
            var result = RemoveWeakests(definition,elites.Concat(childrenByScore).ToList());
            return result;
        }

        public abstract string Id { get; }

        protected abstract IEnumerable<BitArray> SelectByCriteria(DefinitionDto definition, Random random, List<BitArray> generation);
    }
}