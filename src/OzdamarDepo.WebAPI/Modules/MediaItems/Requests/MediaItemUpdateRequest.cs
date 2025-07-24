namespace OzdamarDepo.WebAPI.Modules.MediaItems.Requests
{
    public class MediaItemUpdateRequest
    {
        public string Title { get; set; }= string.Empty;
        public string ImageUrl { get; set; }= string.Empty;
        public string ArtistOrActor { get; set; } = string.Empty;
        public string MediaFormat { get; set; } = string.Empty;
        public string MediaCategory { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ConditionScore { get; set; }
        public string ConditionDescription { get; set; } = string.Empty;
        public bool IsBoxSet { get; set; }
        public int DiscCount { get; set; }
    }

}
