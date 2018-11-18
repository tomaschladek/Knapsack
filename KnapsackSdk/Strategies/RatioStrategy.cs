using System.Linq;
using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies
{
    public class RatioStrategy : AbstractStrategy
    {
        public RatioStrategy(string suffix="")
        {
            Id = $"Ratio{suffix}";
        }

        public override (ResultDto, long) Compute(DefinitionDto definition)
        {
            var ratioItems = definition.Items.Select((item,index) => (index,new RatioItemDto(item.Weight, item.Price))).OrderByDescending(item => item.Item2.Ratio).ToList();
            var list = Enumerable.Repeat(false, ratioItems.Count).ToList();
            var weightSum = 0L;
            var priceSum = 0L;
            foreach (var ratioItem in ratioItems)
            {
                if (weightSum + ratioItem.Item2.Weight <= definition.Capacity)
                {
                    weightSum += ratioItem.Item2.Weight;
                    priceSum += ratioItem.Item2.Price;
                    list[ratioItem.Item1] = true;
                }
                else
                {
                    list[ratioItem.Item1] = false;
                }
            }

            var suitaibleWeight = definition.Items
                .Select((item, index) => (index, new RatioItemDto(item.Weight, item.Price)))
                .Where(item => item.Item2.Weight <= definition.Capacity).ToList();
            var newMax = suitaibleWeight.Max(item => item.Item2.Price);
            var mostPrecious = suitaibleWeight.First(item => item.Item2.Price == newMax);
            if (mostPrecious.Item2.Price > priceSum)
            {
                list = Enumerable.Repeat(false, ratioItems.Count).ToList();
                list[mostPrecious.Item1] = true;
                priceSum = mostPrecious.Item2.Price;
            }
            return (new ResultDto(definition.Id,priceSum,list),1);
        }

        public override string Id { get; }
    }
}