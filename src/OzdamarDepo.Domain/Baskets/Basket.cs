using OzdamarDepo.Domain.Abstractions;

namespace OzdamarDepo.Domain.Baskets
{
    public sealed class Basket:Entity
    {
        public Guid UserId { get; set; }
        public Guid MediaItemId { get; set; }
        public string MediaItemTitle { get; set; } = default!;
        public decimal MediaItemPrice { get; set; }
        public int Quantity { get; set; } = 1;
        public string MediaItemImageUrl { get; set; } = default!;

    }

  
}
