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

            builder.HasOne(b => b.MediaItem)
              .WithMany(m => m.Baskets)
              .HasForeignKey(b => b.MediaItemId);

            builder.HasOne(b => b.Order)
           .WithMany(o => o.Baskets)
           .HasForeignKey(b => b.OrderId)
           .OnDelete(DeleteBehavior.Cascade); // sipariş silinince sepet de silinir


            builder.HasQueryFilter(x => !x.IsDeleted);
        }

    }
}
