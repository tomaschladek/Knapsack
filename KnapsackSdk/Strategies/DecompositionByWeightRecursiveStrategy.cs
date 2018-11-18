using System.Collections.Generic;
using System.Linq;
using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies
{
    public class DecompositionByWeightRecursiveStrategy : AbstractStrategy
    {

        public DecompositionByWeightRecursiveStrategy() : this("")
        {
        }

        public DecompositionByWeightRecursiveStrategy(string id)
        {
            Id = $"Decomposition by Weight Recursive{id}";
        }

        public override (ResultDto, long) Compute(DefinitionDto definition)
        {
            var searchSpace = new RemainderDto[definition.Capacity + 1, definition.Items.Count+1];
            var counter = 0L;
            return (ComputeResultRecursively(definition, new List<bool>(), 0, 0, searchSpace, ref counter),counter);
        }

        private ResultDto ComputeResultRecursively(DefinitionDto definition, List<bool> result, long weight, long price,
            RemainderDto[,] searchSpace, ref long counter)
        {
            if (weight > definition.Capacity)
            {
                return null;
            }
            if (result.Count == definition.Items.Count)
            {
                counter++;
                return new ResultDto(definition.Id, GetSum(result, definition.Items).Price, result);
            }

            var cachedResult = searchSpace[weight, result.Count];
            if (cachedResult != null)
            {
                return new ResultDto(definition.Id,
                    GetSum(result, definition.Items).Price + cachedResult.Price,
                    result.Concat(cachedResult.Result).ToList());
            }

            var newResult = GetNewResult(definition, result, weight, price, searchSpace, ref counter);

            if (newResult != null)
            {
                CacheRemainder(result, weight, price, searchSpace, newResult);
            }

            return newResult;
        }

        private ResultDto GetNewResult(DefinitionDto definition, List<bool> result, long weight, long price, RemainderDto[,] searchSpace, ref long counter)
        {
            var resultWith = GetWithResult(definition, result, weight, price, searchSpace,ref counter);
            var resultWithout = ComputeResultRecursively(definition, new List<bool>(result) { false }, weight, price, searchSpace, ref counter);

            var returnValue = resultWithout;
            if (IsWithBetter(resultWith, resultWithout))
            {
                returnValue = resultWith;
            }
            return returnValue;
        }

        private static bool IsWithBetter(ResultDto resultWith, ResultDto resultWithout)
        {
            return resultWith != null
                            && (resultWithout == null
                                || resultWith.Price > resultWithout.Price);
        }

        private static void CacheRemainder(List<bool> result, long weight, long price, RemainderDto[,] searchSpace, ResultDto resultWith)
        {
            var newRemainder = new RemainderDto(resultWith.Items.Skip(result.Count).ToList(), resultWith.Price - price);
            searchSpace[weight, result.Count] = (searchSpace[weight, result.Count]?.Price ?? -1) < newRemainder.Price
                ? newRemainder
                : searchSpace[weight, result.Count];
        }

        private ResultDto GetWithResult(DefinitionDto definition, List<bool> result, long weight, long price, RemainderDto[,] searchSpace, ref long counter)
        {
            var item = definition.Items[result.Count];
            var sumWeight = weight + item.Weight;
            ResultDto resultWith = null;
            if (sumWeight <= definition.Capacity)
            {
                resultWith = ComputeResultRecursively(definition, new List<bool>(result) { true }, sumWeight, price + item.Price, searchSpace, ref counter);
            }

            return resultWith;
        }

        public override string Id { get; }

        private class RemainderDto
        {
            public List<bool> Result { get; set; }
            public long Price { get; set; }

            public RemainderDto(List<bool> result, long price)
            {
                Result = result;
                Price = price;
            }

            public override string ToString()
            {
                return Price.ToString();
            }
        }
    }
}