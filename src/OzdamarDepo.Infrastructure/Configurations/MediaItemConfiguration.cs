using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzdamarDepo.Domain.MediaItems;

namespace OzdamarDepo.Infrastructure.Configurations;

internal sealed class MediaItemConfiguration : IEntityTypeConfiguration<MediaItem>
{
   
        public void Configure(EntityTypeBuilder<MediaItem> builder)
        {
            builder.OwnsOne(m => m.MediaType, mt =>
            {
                mt.Property(t => t.Format).HasColumnName("Format");
                mt.Property(t => t.Category).HasColumnName("Category");
            });

            builder.OwnsOne(m => m.MediaCondition, c =>
            {
                c.Property(i => i.ConditionScore).HasColumnName("ConditionScore");
                c.Property(i => i.Description).HasColumnName("Description");
            });

            builder.Property(i => i.Price).HasColumnType("money");

        builder.HasMany(m => m.Baskets)
          .WithOne(b => b.MediaItem)
          .HasForeignKey(b => b.MediaItemId)
          .OnDelete(DeleteBehavior.Restrict); //

        builder.HasQueryFilter(x => !x.IsDeleted);


    }
    
}