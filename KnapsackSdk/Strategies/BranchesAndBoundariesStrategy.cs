using System.Collections.Generic;
using System.Linq;
using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies
{
    public class BranchesAndBoundariesStrategy : AbstractStrategy
    {
        public override ResultDto Compute(DefinitionDto definition)
        {
            var sum = 0L;
            var maxPrices = Enumerable.Reverse(definition.Items).Select(item =>
            {
                sum += item.Price;
                return sum;
            }).Reverse().ToList();
            long max = -1;
            return ComputeResultRecursively(definition, new List<bool>(), maxPrices, ref max, 0, 0);
        }

        private ResultDto ComputeResultRecursively(DefinitionDto definition, List<bool> result, List<long> maxPrices, ref long currentMax, long currentWeight, long currentPrice)
        {

            if (currentWeight > definition.Capacity
                || (result.Count < definition.Items.Count
                    && currentPrice + maxPrices[result.Count] < currentMax))
            {
                return null;
            }
            if (result.Count == definition.Items.Count)
            {
                return new ResultDto(definition.Id, currentPrice, result);
            }

            var resultWith = ComputeResultRecursively(definition, new List<bool>(result) { true }, maxPrices, ref currentMax, currentWeight + definition.Items[result.Count].Weight, currentPrice + definition.Items[result.Count].Price);
            var resultWithout = ComputeResultRecursively(definition, new List<bool>(result) { false }, maxPrices, ref currentMax, currentWeight, currentPrice);

            if (resultWith != null
                && (resultWithout == null
                    || resultWith.Price > resultWithout.Price))
            {
                if (currentMax < resultWith.Price)
                {
                    currentMax = resultWith.Price;
                }
                return resultWith;
            }

            if (currentMax < resultWithout?.Price)
            {
                currentMax = resultWithout.Price;
            }
            return resultWithout;
        }

        public override string Id => "B&B";
    }
}