using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzdamarDepo.Application.Orders
{
    public interface IOrderBasketCleaner
    {
        Task ClearUserBasketsAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
