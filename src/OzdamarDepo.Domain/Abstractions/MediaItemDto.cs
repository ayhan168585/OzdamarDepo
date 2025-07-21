namespace OzdamarDepo.Domain.Abstractions
{

    public class MediaItemDto
    {
        public string Title { get; set; } = default!;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = default!;
    }
}
