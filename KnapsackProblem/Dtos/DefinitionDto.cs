using System.Collections.Generic;

namespace KnapsackProblem.Dtos
{
    public class DefinitionDto
    {
        public long Id { get; set; }
        public long Capacity { get; set; }
        public IList<ItemDto> Items { get; set; }

        public DefinitionDto(long id, long capacity, IList<ItemDto> items)
        {
            Id = id;
            Capacity = capacity;
            Items = items;
        }

        public override string ToString()
        {
            return $"{Id} [{Capacity}] size:{Items.Count}";
        }
    }
}