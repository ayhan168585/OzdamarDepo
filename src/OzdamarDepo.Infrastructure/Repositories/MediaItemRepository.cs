using GenericRepository;
using OzdamarDepo.Domain.MediaItems;
using OzdamarDepo.Infrastructure.Context;

namespace OzdamarDepo.Infrastructure.Repositories;

public sealed class MediaItemRepository : Repository<MediaItem,
ApplicationDbContext>, IMediaItemRepository
{
    public MediaItemRepository(ApplicationDbContext context) : base(context)
    {
    }
}
