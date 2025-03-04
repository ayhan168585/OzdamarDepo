using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzdamarDepo.Domain.Users;

namespace OzdamarDepo.Infrastructure.Configurations
{
    internal sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasIndex(p => p.UserName).IsUnique();
            builder.Property(p => p.FirstName).HasColumnType("varchar(50)");
            builder.Property(p => p.LastName).HasColumnType("varchar(50)");
            builder.Property(p => p.UserName).HasColumnType("varchar(15)");
            builder.Property(p => p.Email).HasColumnType("varchar(MAX)");
        }
    }
}
