using OzdamarDepo.Domain.Abstractions;
using OzdamarDepo.Domain.Orders;

namespace OzdamarDepo.Domain.Baskets
{
    public sealed class Basket:Entity
    {
        public Guid UserId { get; set; }
        public Guid MediaItemId { get; set; }
        public string MediaItemTitle { get; set; } = default!;
        public decimal MediaItemPrice { get; set; }
        public int Quantity { get; set; }
        public string MediaItemImageUrl { get; set; } = default!;

        public Guid? OrderId { get; set; }  // DİKKAT: Nullable yap!
        public Order? Order { get; set; }   // Navigasyon özelliği

    }

  
}
