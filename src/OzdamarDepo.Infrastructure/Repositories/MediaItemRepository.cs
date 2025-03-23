using GenericRepository;
using OzdamarDepo.Domain.MediaItems;
using OzdamarDepo.Infrastructure.Context;

namespace OzdamarDepo.Infrastructure.Repositories;

internal sealed class MediaItemRepository : Repository<MediaItem,
ApplicationDbContext>, IMediaItemRepository
{
    public MediaItemRepository(ApplicationDbContext context) : base(context)
    {
    }
}
