using OzdamarDepo.Domain.Abstractions;
using OzdamarDepo.Domain.MediaItems;
using OzdamarDepo.Domain.Orders;
using OzdamarDepo.Domain.Users;

namespace OzdamarDepo.Domain.Baskets
{
    public class Basket:Entity
    {
        public Guid UserId { get; set; }
        public Guid MediaItemId { get; set; }

        public string MediaItemTitle { get; set; } = default!;
        public decimal MediaItemPrice { get; set; }
        public int Quantity { get; set; }
        public string MediaItemImageUrl { get; set; } = default!;

        public Guid? OrderId { get; set; }

        public virtual MediaItem MediaItem { get; set; } = default!;
        public virtual Order? Order { get; set; }
        public virtual AppUser User { get; set; } = default!;
        public bool IsInBasket { get; set; } = true;




    }


}
