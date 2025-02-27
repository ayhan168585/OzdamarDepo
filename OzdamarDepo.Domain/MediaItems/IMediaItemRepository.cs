using GenericRepository;
using OzdamarDepo.Domain.MediaItems;

namespace OzdamarDepo.Domain.MediaItems
{
    public interface IMediaItemRepository : IRepository<MediaItem>
    {
        // Özel sorgu örneği
        Task<IEnumerable<MediaItem>> GetByMediaTypeAsync(string mediaType);
        Task<IEnumerable<MediaItem>> GetItemsByConditionScoreAsync(int minScore);
    }
} 