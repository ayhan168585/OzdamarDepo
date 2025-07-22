namespace OzdamarDepo.Domain.Abstractions
{
    public class MediaItemCreateRequest
    {
        public string Title { get; set; }
        public string ArtistOrActor { get; set; }
        public string MediaFormat { get; set; }
        public string MediaCategory { get; set; }
        public bool IsBoxSet { get; set; }
        public int DiscCount { get; set; }
        public int ConditionScore { get; set; }
        public string ConditionDescription { get; set; }
        public decimal Price { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
