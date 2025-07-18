using GenericRepository;
using OzdamarDepo.Domain.Baskets;
using OzdamarDepo.Domain.Orders;
using OzdamarDepo.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzdamarDepo.Infrastructure.Repositories
{
    public sealed class OrderRepository : Repository<Order,
  ApplicationDbContext>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
