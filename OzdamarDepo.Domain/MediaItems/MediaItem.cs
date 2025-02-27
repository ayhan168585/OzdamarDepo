using OzdamarDepo.Domain.Abstractions;

namespace OzdamarDepo.Domain.MediaItems
{
    public class MediaItem : Entity
    {
        public string Title { get; set; } = default!;
        public string ArtistOrDirector { get; set; } = default!;
        public MediaType MediaType { get; set; } = new();
        public decimal Price { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public MediaCondition Condition { get; set; } = new();
        public bool IsBoxSet { get; set; }
        public int DiscCount { get; set; } = 1;
    }
} 