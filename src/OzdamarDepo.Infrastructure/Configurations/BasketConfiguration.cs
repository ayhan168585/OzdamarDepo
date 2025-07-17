using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OzdamarDepo.Domain.MediaItems;
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
