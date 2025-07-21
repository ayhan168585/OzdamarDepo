using GenericRepository;
using Microsoft.EntityFrameworkCore;
using OzdamarDepo.Domain.Baskets;
using OzdamarDepo.Infrastructure.Context;

namespace OzdamarDepo.Infrastructure.Repositories
{
    public sealed class BasketRepository : Repository<Basket, ApplicationDbContext>, IBasketRepository
    {
        private readonly ApplicationDbContext _context;

        public BasketRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Basket>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken)
        {
            return await _context.Baskets
         .Include(b => b.MediaItem) // MediaItem navigation'ı alınsın
         .Where(b => ids.Contains(b.Id) && !b.IsDeleted)
         .ToListAsync(cancellationToken);
        }

        public Task UpdateRangeAsync(IEnumerable<Basket> baskets)
        {
            _context.Baskets.UpdateRange(baskets);
            return Task.CompletedTask;
        }
    }

}
