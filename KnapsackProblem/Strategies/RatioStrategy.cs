using System.Collections.Generic;
using System.Linq;
using KnapsackProblem.Dtos;

namespace KnapsackProblem.Strategies
{
    public class RatioStrategy : AbstractStrategy
    {
        public override ResultDto Compute(DefinitionDto definition)
        {
            var ratioItems = definition.Items.Select(item => new RatioItemDto(item.Weight, item.Price)).OrderByDescending(item => item.Ratio).ToList();
            var list = new List<bool>();
            var weightSum = 0L;
            var priceSum = 0L;
            foreach (var ratioItem in ratioItems)
            {
                if (weightSum + ratioItem.Weight <= definition.Capacity)
                {
                    weightSum += ratioItem.Weight;
                    priceSum += ratioItem.Price;
                    list.Add(true);
                }
                else
                {
                    list.Add(false);
                }
            }
            return new ResultDto(definition.Id,priceSum,list);
        }

        public override string Id => "Ratio";
    }
}