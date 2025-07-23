using GenericRepository;
using OzdamarDepo.Domain.Abstractions;
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
        Task<List<Order>> GetAllWithBasketsAsync();
        Task<List<OrderWithBasketsDto>> GetOrdersWithBasketsAndMediaAsync();
        Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    }
}
