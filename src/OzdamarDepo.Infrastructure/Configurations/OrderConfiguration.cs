using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzdamarDepo.Domain.Orders;

namespace OzdamarDepo.Infrastructure.Configurations
{
    internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasMany(o => o.Baskets)
                   .WithOne(b => b.Order)
                   .HasForeignKey(b => b.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Enum alanını int olarak tanıt (zorunlu olmasa da tavsiye edilir)
            builder.Property(o => o.CargoStatus)
                   .HasConversion<int>()
                   .IsRequired();

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
