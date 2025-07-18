using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzdamarDepo.Domain.Baskets;

namespace OzdamarDepo.Infrastructure.Configurations
{
    internal sealed class BasketConfiguration : IEntityTypeConfiguration<Basket>
    {
        public void Configure(EntityTypeBuilder<Basket> builder)
        {
            builder.Property(i => i.MediaItemPrice).HasColumnType("money");

            builder.HasQueryFilter(x => !x.IsDeleted);
        }

    }
}
