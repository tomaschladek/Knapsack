using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies.Genetic.Corrections
{
    public class RatioCorrectionStrategy : ICorrectionStrategy
    {
        public string Id => "Ratio correction";

        public IEnumerable<BitArray> CorrectGeneration(DefinitionDto definition, List<BitArray> generation)
        {
            foreach (var fenotyp in generation)
            {
                while (definition.Capacity < definition.Items.Select((item, itemIndex) => fenotyp[itemIndex] ? item.Weight : 0).Sum())
                {
                    var newMin = new { Index = -1, Value = double.MaxValue };
                    for (var indexGen = 0; indexGen < definition.Items.Count; indexGen++)
                    {
                        var item = definition.Items[indexGen];
                        var ratio = (double)item.Price / item.Weight;
                        if (fenotyp[indexGen] && ratio < newMin.Value)
                        {
                            newMin = new { Index = indexGen, Value = ratio };
                        }
                    }

                    if (newMin.Index != -1)
                    {
                        fenotyp[newMin.Index] = false;
                    }
                }
                yield return fenotyp;
            }
        }
    }
}