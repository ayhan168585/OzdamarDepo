﻿using Microsoft.EntityFrameworkCore;
using OzdamarDepo.Application.Orders;
using OzdamarDepo.Infrastructure.Context;

namespace OzdamarDepo.Infrastructure.Repositories
{
    // OrderBasketCleaner.cs
    public sealed class OrderBasketCleaner(ApplicationDbContext context) : IOrderBasketCleaner
    {
        public async Task ClearUserBasketsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var baskets = await context.Baskets
                .Where(b => b.UserId == userId && b.IsInBasket && !b.IsDeleted)
                .ToListAsync(cancellationToken);

            foreach (var basket in baskets)
            {
                basket.IsDeleted = true;
                basket.IsInBasket = false; // 👈 Sepetten çıkar
            }

            await context.SaveChangesAsync(cancellationToken);
        }
    }


}
