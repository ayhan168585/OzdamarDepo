using GenericRepository;
using OzdamarDepo.Domain.Baskets;
using OzdamarDepo.Infrastructure.Context;

namespace OzdamarDepo.Infrastructure.Repositories
{
    public sealed class BasketRepository : Repository<Basket,
 ApplicationDbContext>, IBasketRepository
    {
        public BasketRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
