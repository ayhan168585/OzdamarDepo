using GenericRepository;
using OzdamarDepo.Domain.Baskets;
using OzdamarDepo.Infrastructure.Context;

namespace OzdamarDepo.Infrastructure.Repositories
{
    internal sealed class BasketRepository : Repository<Basket,
 ApplicationDbContext>, IBasketRepository
    {
        public BasketRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
