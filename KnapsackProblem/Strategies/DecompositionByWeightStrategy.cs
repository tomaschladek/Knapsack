using System;
using KnapsackProblem.Dtos;

namespace KnapsackProblem.Strategies
{
    public class DecompositionByWeightStrategy : AbstractDecompositionStrategy
    {
        protected override int GetSearchSpaceSize(DefinitionDto definition)
        {
            return (int) (definition.Capacity + 1);
        }

        protected override long GetTableValue(long without, long with)
        {
            return Math.Max(without, with);
        }

        protected override bool IsNewMaximum(DefinitionDto definition, long[,] stateSpace, PositionDto maxPrize, int indexWeight, int indexItem)
        {
            return stateSpace[indexWeight, indexItem] > maxPrize.Value;
        }

        protected override long GetCompositionValue(ItemDto item)
        {
            return item.Weight;
        }

        protected override long GetTableValue(ItemDto item)
        {
            return item.Price;
        }

        public override string Id => "WeightDecomposition";
    }
}