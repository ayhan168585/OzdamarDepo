using OzdamarDepo.Domain.Orders;

namespace OzdamarDepo.Domain.Abstractions
{
    public class OrderWithBasketsDto
    {
        public Order Order { get; set; } = default!;
        public List<BasketWithMediaItemDto> Baskets { get; set; } = new();
    }
}
