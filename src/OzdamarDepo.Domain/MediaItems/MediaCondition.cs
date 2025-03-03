namespace OzdamarDepo.Domain.MediaItems
{
    public sealed record MediaCondition
    {
        public int ConditionScore { get; set; } = 5; // 1-10 arası
        public string Description { get; set; } = "İyi durumda";
    }
} 