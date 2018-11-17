namespace KnapsackProblem.Dtos
{
    public class RatioItemDto : ItemDto
    {
        public double Ratio { get; private set; }
        public RatioItemDto(long weight, long price) : base(weight, price)
        {
            Ratio = (double)price / weight;
        }
    }
}