using System.Collections.Generic;

namespace KnapsackSdk.Dtos
{
    public class ResultDto
    {
        public long Id { get; set; }
        public long Price { get; set; }
        public IList<bool> Items { get; set; }

        public ResultDto(long id, long price, IList<bool> items)
        {
            Id = id;
            Price = price;
            Items = items;
        }

        public override string ToString()
        {
            return $"{Id} {Price}";
        }
    }
}