using GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzdamarDepo.Domain.Baskets
{
    public interface IBasketRepository:IRepository<Basket>
    {
        
        public async Task SoftDeleteAsync(Basket basket, Guid userId)
        {
          
        }
    }
}
