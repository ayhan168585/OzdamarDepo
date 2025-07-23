using GenericRepository;
using Microsoft.EntityFrameworkCore;
using OzdamarDepo.Application.Orders;
using OzdamarDepo.Domain.Abstractions;
using OzdamarDepo.Domain.Orders;
using OzdamarDepo.Infrastructure.Context;

namespace OzdamarDepo.Infrastructure.Repositories
{
    public sealed class OrderRepository : Repository<Order, ApplicationDbContext>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

     

        public async Task<List<Order>> GetAllWithBasketsAsync()
        {
            return await _context.Orders
                .Include(o => o.Baskets)
                    .ThenInclude(b => b.MediaItem)
                .Where(o => !o.IsDeleted)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .Include(o => o.Baskets)
                    .ThenInclude(b => b.MediaItem)
                .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted, cancellationToken);
        }

        public async Task<List<OrderWithBasketsDto>> GetOrdersWithBasketsAndMediaAsync()
        {
            return await _context.Orders
                .Include(o => o.Baskets)
                    .ThenInclude(b => b.MediaItem)
                    .IgnoreQueryFilters() // 🔥 bu şart!
                .Where(o => !o.IsDeleted)
                .Select(o => new OrderWithBasketsDto
                {
                    Order = o,
                    Baskets = o.Baskets // 🔥 Artık IsDeleted kontrolü yok!
                        .Select(b => new BasketWithMediaItemDto
                        {
                            Id = b.Id,
                            Quantity = b.Quantity,
                            MediaItemPrice = b.MediaItem != null ? b.MediaItem.Price : 0,
                            MediaItemTitle = b.MediaItem != null ? b.MediaItem.Title : "",
                            MediaItemImageUrl = b.MediaItem != null ? b.MediaItem.ImageUrl : "",
                            MediaItem = b.MediaItem != null
                                ? new MediaItemDto
                                {
                                    Title = b.MediaItem.Title,
                                    Price = b.MediaItem.Price,
                                    ImageUrl = b.MediaItem.ImageUrl
                                }
                                : null
                        }).ToList()
                })
                .ToListAsync();
        }



    }
}
