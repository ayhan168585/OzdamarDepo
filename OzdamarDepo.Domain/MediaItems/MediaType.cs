namespace OzdamarDepo.Domain.MediaItems
{
    public sealed record MediaType
    {
        public string Format { get; set; } = "CD"; // CD/DVD/Blu-ray
        public string Category { get; set; } = "Music"; // Music/Film/Game
    }
} 