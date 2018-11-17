using System.Collections.Generic;
using System.Linq;
using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies
{
    public abstract class AbstractDecompositionStrategy : AbstractStrategy
    {
        protected abstract int GetSearchSpaceSize(DefinitionDto definition);
        protected abstract long GetTableValue(long without, long with);
        protected abstract bool IsNewMaximum(DefinitionDto definition, long[,] stateSpace, PositionDto maxPrize, int indexPrice, int indexItem);
        protected abstract long GetCompositionValue(ItemDto item);
        protected abstract long GetTableValue(ItemDto item);

        public override (ResultDto, long) Compute(DefinitionDto definition)
        {
            var searchSpaceSize = GetSearchSpaceSize(definition);
            var searchSpace = new long[searchSpaceSize, definition.Items.Count];

            var referencePoint = GetMaxPricePoint(definition, searchSpaceSize, searchSpace);
            var result = ReconstructItems(searchSpace, referencePoint.X, referencePoint.Y, definition);

            return (new ResultDto(definition.Id, GetResultValue(result,definition), result), definition.Items.Count*searchSpaceSize);
        }

        private long GetResultValue(IList<bool> result, DefinitionDto definition)
        {
            return result.Zip(definition.Items, (presence, item) => presence ? item.Price : 0).Sum();
        }

        private long GetMax(DefinitionDto definition, long[,] stateSpace, int indexValue, int indexItem)
        {
            var without = GetWithoutSum(stateSpace, indexValue, indexItem);
            var with = GetWithSum(stateSpace, indexValue, indexItem, definition.Items[indexItem]);

            return GetTableValue(without, with);
        }

        protected PositionDto GetMaxPricePoint(DefinitionDto definition, long searchSpaceSiz, long[,] stateSpace)
        {
            var referencePoint = new PositionDto(0, 0, 0);
            for (var indexValue = 0; indexValue < searchSpaceSiz; indexValue++)
            {
                for (var indexItem = 0; indexItem < definition.Items.Count; indexItem++)
                {
                    stateSpace[indexValue, indexItem] = GetMax(definition, stateSpace, indexValue, indexItem);
                    if (IsNewMaximum(definition, stateSpace, referencePoint, indexValue, indexItem))
                    {
                        referencePoint = new PositionDto(indexValue, indexItem, stateSpace[indexValue, indexItem]);
                    }
                }
            }

            return referencePoint;
        }

        protected IList<bool> ReconstructItems(long[,] stateSpace, int x, int y, DefinitionDto definition)
        {
            var result = GetPath(stateSpace, x, y, definition);
            return result.Reverse().ToList();
        }

        private IEnumerable<bool> GetPath(long[,] stateSpace, int x, int y, DefinitionDto definition)
        {
            for (int index = 0; index <= y; index++)
            {
                var without = GetWithoutSum(stateSpace, x, y - index);
                var isWithSum = without != stateSpace[x, y - index];
                if (isWithSum)
                {
                    x = x - (int)GetCompositionValue(definition.Items[y - index]);
                }

                yield return isWithSum;
            }
        }

        private static long GetWithoutSum(long[,] stateSpace, int x, int y)
        {
            return y - 1 < 0
                ? 0
                : stateSpace[x, y - 1];
        }

        private long GetWithSum(long[,] stateSpace, int indexValue, int indexItem, ItemDto item)
        {
            if (indexValue - GetCompositionValue(item) < 0)
            {
                // not enough capacity
                return 0;
            }

            if (indexValue - GetCompositionValue(item) == 0)
            {
                // item itself
                return GetTableValue(item);
            }

            if (indexItem > 0
                && stateSpace[indexValue - GetCompositionValue(item), indexItem - 1] > 0)
            {
                // there is previous result and I can still add this item
                return stateSpace[indexValue - GetCompositionValue(item), indexItem - 1] + GetTableValue(item);
            }
            return 0;
        }

        protected class PositionDto
        {
            public int X { get; set; }
            public int Y { get; set; }
            public long Value { get; set; }

            public PositionDto(int x, int y, long value)
            {
                X = x;
                Y = y;
                Value = value;
            }
        }
    }
}