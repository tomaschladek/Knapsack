using System.Collections.Generic;
using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies
{
    public class RecursiveStrategy : AbstractStrategy
    {
        public override ResultDto Compute(DefinitionDto definition)
        {
            return ComputeResultRecursively(definition, new List<bool>(), 0);
        }

        private ResultDto ComputeResultRecursively(DefinitionDto definition, List<bool> result, long weight)
        {
            if (result.Count == definition.Items.Count)
            {
                if (weight > definition.Capacity)
                {
                    return null;
                }
                return new ResultDto(definition.Id, GetSum(result, definition.Items).Price, result);
            }
            var item = definition.Items[result.Count];
            var sumWeight = weight + item.Weight;
            var withPresence = new List<bool>(result) { true };
            var resultWith = ComputeResultRecursively(definition, withPresence, sumWeight);

            var withoutPresence = new List<bool>(result) { false };
            var resultWithout = ComputeResultRecursively(definition, withoutPresence, weight);

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