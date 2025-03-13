using OzdamarDepo.Domain.Abstractions;

namespace OzdamarDepo.Domain.MediaItems
{
    public class MediaItem : Entity
    {
        public string Title { get; set; } = default!;
        public string ArtistOrActor { get; set; } = default!;
        public MediaType MediaType { get; set; } = default!;
        public decimal Price { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public MediaCondition MediaCondition { get; set; } = default!;
        public bool IsBoxSet { get; set; }
        public int DiscCount { get; set; } = 1;

        public MediaDurumEnum MediaDurum { get; set; }= default!;



    }

}
