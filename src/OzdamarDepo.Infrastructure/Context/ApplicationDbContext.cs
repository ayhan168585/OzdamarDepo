using GenericRepository;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OzdamarDepo.Domain.MediaItems;
using OzdamarDepo.Domain.Users;
using Microsoft.AspNetCore.Http;
using OzdamarDepo.Domain.Abstractions;
using OzdamarDepo.Domain.Baskets;

namespace OzdamarDepo.Infrastructure.Context;

internal sealed class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>, IUnitOfWork
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApplicationDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<MediaItem> MediaItems { get; set; }
    public DbSet<Basket> Baskets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.Ignore<IdentityUserLogin<Guid>>();
        modelBuilder.Ignore<IdentityRoleClaim<Guid>>();
        modelBuilder.Ignore<IdentityUserToken<Guid>>();
        modelBuilder.Ignore<IdentityUserRole<Guid>>();
        modelBuilder.Ignore<IdentityUserClaim<Guid>>();

    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Entity>();

        Guid? userId = null;
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null && httpContext.User.Identity is { IsAuthenticated: true })
        {
            var userIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == "user-id");
            if (userIdClaim != null)
            {
                userId = Guid.Parse(userIdClaim.Value);
            }
        }

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(p => p.CreatedAt).CurrentValue = DateTimeOffset.Now;
                entry.Property(p => p.CreateUserId).CurrentValue = userId ?? Guid.Empty;
            }
            else if (entry.State == EntityState.Modified)
            {
                if ((bool)entry.Property(p => p.IsDeleted).CurrentValue!)
                {
                    entry.Property(p => p.DeletedAt).CurrentValue = DateTimeOffset.Now;
                    entry.Property(p => p.DeleteUserId).CurrentValue = userId ?? Guid.Empty;
                }
                else
                {
                    entry.Property(p => p.UpdatedAt).CurrentValue = DateTimeOffset.Now;
                    entry.Property(p => p.UpdateUserId).CurrentValue = userId ?? Guid.Empty;
                }
            }

            if (entry.State == EntityState.Deleted)
            {
                throw new ArgumentException("Db'den direkt silme işlemi yapamazsınız!");
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

}