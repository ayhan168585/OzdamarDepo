using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzdamarDepo.Domain.Genel_Ayarlar;

namespace OzdamarDepo.Infrastructure.Configurations
{
    public class AppSettingConfiguration : IEntityTypeConfiguration<AppSetting>
    {
        public void Configure(EntityTypeBuilder<AppSetting> builder)
        {
            builder.Property(x => x.Key)
                .HasMaxLength(450)
                .IsRequired();

            builder.Property(x => x.Value)
                .IsRequired();

            builder.Property(x => x.ValueType)
                .HasConversion<string>() // Enum'u string olarak saklar
                .IsRequired();
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
