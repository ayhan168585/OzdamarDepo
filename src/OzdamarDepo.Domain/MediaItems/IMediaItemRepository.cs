using GenericRepository;
using System.Threading.Tasks;

namespace OzdamarDepo.Domain.MediaItems;

public interface IMediaItemRepository : IRepository<MediaItem>
{
    Task<MediaItem?> GetByIdAsync(Guid id);

}
