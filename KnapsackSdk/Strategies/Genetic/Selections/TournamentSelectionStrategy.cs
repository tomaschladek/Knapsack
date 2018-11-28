using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies.Genetic.Selections
{
    public class TournamentSelectionStrategy : AbstractSelectionStrategy
    {
        public TournamentSelectionStrategy(int tournamentSize) : this(tournamentSize, 0, 0)
        {

        }

        public TournamentSelectionStrategy(int tournamentSize, int elitesCount, int weakestsCount) : base(elitesCount, weakestsCount)
        {
            TournamentSize = tournamentSize;
        }

        public int TournamentSize { get; set; }

        protected override IEnumerable<BitArray> SelectByCriteria(DefinitionDto definition, Random random, List<BitArray> generation)
        {
            for (int index = StartCount; index < generation.Count; index++)
            {
                var tournament = GenerateTournament(random, generation)
                    .Select((fenotyp, tournamentInde) => new
                    {
                        Index = tournamentInde,
                        Fenotyp = fenotyp,
                        Score = GetScoreItem(fenotyp, definition)
                    })
                    .Where(tuple => tuple.Score.Weight <= definition.Capacity)
                    .ToList();
                var max = tournament.Max(item => item.Score.Price);
                if (max != 0)
                {
                    yield return tournament.First(item => item.Score.Price == max).Fenotyp;
                }
            }
        }

        private IEnumerable<BitArray> GenerateTournament(Random random, List<BitArray> generation)
        {
            for (int tournamentRound = 0; tournamentRound < TournamentSize; tournamentRound++)
            {
                var randomIndex = random.Next(0, generation.Count);
                yield return generation[randomIndex];
            }
        }
    }
}