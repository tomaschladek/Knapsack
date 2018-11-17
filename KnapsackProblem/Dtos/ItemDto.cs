namespace KnapsackProblem.Dtos
{
    public class ItemDto
    {
        public long Weight { get; set; }
        public long Price { get; set; }

        public ItemDto(long weight, long price)
        {
            Weight = weight;
            Price = price;
        }

        public override string ToString()
        {
            return $"W:{Weight} P:{Price}";
        }
    }
}