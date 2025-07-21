using GenericRepository;
using Microsoft.EntityFrameworkCore;
using OzdamarDepo.Domain.MediaItems;
using OzdamarDepo.Infrastructure.Context;

namespace OzdamarDepo.Infrastructure.Repositories;

public sealed class MediaItemRepository : Repository<MediaItem, ApplicationDbContext>, IMediaItemRepository
{
    private readonly ApplicationDbContext _context;

    public MediaItemRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<MediaItem?> GetByIdAsync(Guid id)
    {
        return await _context.MediaItems
            .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
    }
}
