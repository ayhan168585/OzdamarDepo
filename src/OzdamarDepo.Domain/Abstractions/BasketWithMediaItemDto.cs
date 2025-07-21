namespace OzdamarDepo.Domain.Abstractions
{
    public class BasketWithMediaItemDto
    {
        public Guid Id { get; init; }
        public int Quantity { get; init; }
        public decimal MediaItemPrice { get; init; }
        public string MediaItemTitle { get; init; }
        public string MediaItemImageUrl { get; init; }

        // MediaItem nesnesi varsa:
        public MediaItemDto MediaItem { get; init; }

        public decimal Total { get; set; }

    }
}
