using System;
using System.Linq;
using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies
{
    public class DecompositionByPriceStrategy : AbstractDecompositionStrategy
    {
        protected override bool IsNewMaximum(DefinitionDto definition, long[,] stateSpace, PositionDto maxPrize, int indexPrice, int indexItem)
        {
            return indexPrice > maxPrize.X
                    && indexItem == definition.Items.Count-1
                    && stateSpace[indexPrice, indexItem] > 0
                    && stateSpace[indexPrice, indexItem] <= definition.Capacity;
        }

        protected override int GetSearchSpaceSize(DefinitionDto definition)
        {
            return (int) (definition.Items.Sum(item => item.Price) + 1);
        }

        protected override long GetTableValue(long without, long with)
        {
            if (with > 0 && without > 0)
            {
                return Math.Min(without, with);
            }
            return Math.Max(without, with);
        }

        protected override long GetCompositionValue(ItemDto item)
        {
            return item.Price;
        }


        protected override long GetTableValue(ItemDto item)
        {
            return item.Weight;
        }

        public override string Id => "PriceDecomposition";
    }
}