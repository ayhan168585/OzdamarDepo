using GenericRepository;
using OzdamarDepo.Domain.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzdamarDepo.Domain.Orders
{
    public interface IOrderRepository : IRepository<Order>
    {
    }
}
