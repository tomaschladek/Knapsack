using System;
using System.Collections.Generic;
using KnapsackSdk.Dtos;

namespace KnapsackProblem.Experiments
{
    public class Generator : IGenerator
    {
        public DefinitionDto Generate(int instanceSize,Func<long> getCapacity, Func<int, long> getWeight, Func<int, long> getPrice)
        {
            var items = new List<ItemDto>();
            for (int index = 0; index < instanceSize; index++)
            {
                items.Add(new ItemDto(getWeight(index),getPrice(index)));
            }
            return new DefinitionDto(1,getCapacity(),items);
        }
    }
}