using System;
using System.Collections.Generic;
using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies
{
    public class IterativeStrategy : AbstractStrategy
    {
        public override (ResultDto, long) Compute(DefinitionDto definition)
        {
            var maxValue = Math.Pow(2, definition.Items.Count);
            var maxPrice = 0L;
            var bestSeed = 0L;

            for (long index = 0; index < maxValue; index++)
            {
                var sumItem = GetSum(index, definition.Items);

                if (sumItem[0] <= definition.Capacity && sumItem[1] > maxPrice)
                {
                    maxPrice = sumItem[1];
                    bestSeed = index;
                }
            }

            return (new ResultDto(definition.Id, maxPrice, GetItems(bestSeed, definition.Items.Count)),(long)maxValue);
        }

        public override string Id => "Iterative";

        private static IList<bool> GetItems(long seedValue, int itemsCount)
        {
            var bitValue = 1;
            var items = new List<bool>();
            for (int index = 0; index < itemsCount; index++)
            {
                items.Add((bitValue & seedValue) > 0);
                bitValue = bitValue << 1;
            }

            return items;
        }

    }
}