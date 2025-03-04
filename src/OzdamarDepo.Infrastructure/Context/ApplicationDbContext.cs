using GenericRepository;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OzdamarDepo.Domain.MediaItems;
using OzdamarDepo.Domain.Users;
using Microsoft.AspNetCore.Http;

namespace OzdamarDepo.Infrastructure.Context;

internal sealed class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<MediaItem> MediaItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.Ignore<IdentityUserLogin<Guid>>();
        modelBuilder.Ignore<IdentityUserRole<Guid>>();
        modelBuilder.Ignore<IdentityUserClaim<Guid>>();
        modelBuilder.Ignore<IdentityUserToken<Guid>>();
        modelBuilder.Ignore<IdentityRoleClaim<Guid>>();

    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<OzdamarDepo.Domain.Abstractions.Entity>();
        HttpContextAccessor httpContextAccessor = new();
        string userIdString = httpContextAccessor.HttpContext!.User.Claims.First(p => p.Type == "user-id").Value;
        Guid userId = Guid.Parse(userIdString);
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(p => p.CreatedAt)
                    .CurrentValue = DateTimeOffset.Now;
                entry.Property(p => p.CreateUserId)
                    .CurrentValue = userId;
            }
            if (entry.State == EntityState.Modified)
            {
                if (entry.Property(p => p.IsDeleted).CurrentValue == true)
                {
                    entry.Property(p => p.DeletedAt)
                   .CurrentValue = DateTimeOffset.Now;
                    entry.Property(p => p.DeleteUserId)
                        .CurrentValue = userId;
                }
                else
                {
                    entry.Property(p => p.UpdatedAt)
                   .CurrentValue = DateTimeOffset.Now;
                    entry.Property(p => p.UpdateUserId)
                        .CurrentValue = userId;
                }

            }

            if (entry.State == EntityState.Deleted)
            {
                throw new ArgumentException("Db'den direkt silme iþlemi yapamazsýnýz!");
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

}