using System.Collections;
using System.Collections.Generic;
using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies.Genetic.Corrections
{
    public class NoCorrectionStrategy : ICorrectionStrategy
    {
        public string Id => "No correction";

        public IEnumerable<BitArray> CorrectGeneration(DefinitionDto definition, List<BitArray> generation)
        {
            return generation;
        }
    }
}