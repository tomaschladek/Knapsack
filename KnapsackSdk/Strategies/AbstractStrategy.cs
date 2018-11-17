using System.Collections.Generic;
using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies
{
    public abstract class AbstractStrategy : IStrategy
    {
        public abstract ResultDto Compute(DefinitionDto definition);
        public abstract string Id { get; }


        protected ItemDto GetSum(IList<bool> presenceVector, IList<ItemDto> definitionItems)
        {
            ItemDto item = new ItemDto(0, 0);
            for (int index = 0; index < presenceVector.Count; index++)
            {
                if (presenceVector[index])
                {
                    var definitionItem = definitionItems[index];
                    item.Price += definitionItem.Price;
                    item.Weight += definitionItem.Weight;
                }
            }

            return item;
        }


        protected long[] GetSum(long seedValue, IList<ItemDto> definitionItems)
        {
            var bitValue = 1;
            var price = 0L;
            var weight = 0L;
            foreach (var item in definitionItems)
            {
                var isOn = (bitValue & seedValue) > 0;
                if (isOn)
                {
                    var definitionItem = item;
                    price += definitionItem.Price;
                    weight += definitionItem.Weight;
                }
                bitValue = bitValue << 1;
            }
            return new[] {weight, price};
        }
    }
}