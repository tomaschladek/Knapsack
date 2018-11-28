using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies.Genetic.Selections
{
    public class EliteSelectionStrategy : ReplaceAllByPriceSelectionStrategy
    {
        public EliteSelectionStrategy(int elitesCount)
        {
            ElitesCount = elitesCount;
        }

        public int ElitesCount { get; set; }

        public override IEnumerable<BitArray> Select(DefinitionDto definition, Random random, List<BitArray> generation)
        {
            var elites = generation
                .Select(fenotyp => new {Fenotyp = fenotyp, Score = GetScoreItem(fenotyp, definition)})
                .Where(tuple => tuple.Score.Weight <= definition.Capacity)
                .OrderByDescending(tuple => tuple.Score.Price)
                .Take(ElitesCount).ToArray();

            StartCount = elites.Length;

            return elites.Select(item => item.Fenotyp).Concat(base.Select(definition, random, generation.ToList()));
        }
    }
}