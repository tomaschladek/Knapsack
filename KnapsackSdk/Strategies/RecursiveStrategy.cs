using System.Collections.Generic;
using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies
{
    public class RecursiveStrategy : AbstractStrategy
    {
        public override (ResultDto, long) Compute(DefinitionDto definition)
        {
            var counter = 0L;
            var result = ComputeResultRecursively(definition, new List<bool>(), 0, ref counter);
            return (result, counter);
        }

        private ResultDto ComputeResultRecursively(DefinitionDto definition, List<bool> result, long weight,
            ref long counter)
        {
            if (result.Count == definition.Items.Count)
            {
                counter++;
                if (weight > definition.Capacity)
                {
                    return null;
                }
                return new ResultDto(definition.Id, GetSum(result, definition.Items).Price, result);
            }
            var item = definition.Items[result.Count];
            var sumWeight = weight + item.Weight;
            var withPresence = new List<bool>(result) { true };
            var resultWith = ComputeResultRecursively(definition, withPresence, sumWeight, ref counter);

            var withoutPresence = new List<bool>(result) { false };
            var resultWithout = ComputeResultRecursively(definition, withoutPresence, weight, ref counter);

            if (sumWeight <= definition.Capacity
                && resultWith != null
                && (resultWithout == null
                    || resultWith.Price > resultWithout.Price))
            {
                return resultWith;
            }

            if (resultWithout != null
                && weight <= definition.Capacity)
            {
                return resultWithout;
            }

            return null;
        }

        public override string Id => "Recursive";
    }
}