using GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OzdamarDepo.Domain.Abstractions;
using OzdamarDepo.Domain.Baskets;
using OzdamarDepo.Domain.Genel_Ayarlar;
using OzdamarDepo.Domain.MediaItems;
using OzdamarDepo.Domain.Orders;
using OzdamarDepo.Domain.Users;
using OzdamarDepo.Infrastructure.Configurations;
using System.Security.Claims;

namespace OzdamarDepo.Infrastructure.Context;

public sealed class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>, IUnitOfWork
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApplicationDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<MediaItem> MediaItems { get; set; }
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<Order> Orders { get; set; }

    public DbSet<AppSetting> AppSettings { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.Ignore<IdentityUserLogin<Guid>>();
        modelBuilder.Ignore<IdentityRoleClaim<Guid>>();
        modelBuilder.Ignore<IdentityUserToken<Guid>>();
        modelBuilder.Ignore<IdentityUserRole<Guid>>();
        modelBuilder.Ignore<IdentityUserClaim<Guid>>();




        modelBuilder.Entity<Order>()
       .HasMany(o => o.Baskets)
       .WithOne(b => b.Order)
       .HasForeignKey(b => b.OrderId)
       .OnDelete(DeleteBehavior.Restrict); // Optional: Order silinince basket silinmesin

        modelBuilder.Entity<Basket>()
            .HasOne(b => b.MediaItem)
            .WithMany()
            .HasForeignKey(b => b.MediaItemId);

        modelBuilder.Entity<Basket>()
            .HasOne(b => b.User)
            .WithMany()
            .HasForeignKey(b => b.UserId);

        modelBuilder.Entity<Basket>()
    .HasOne(b => b.Order)
    .WithMany(o => o.Baskets)
    .HasForeignKey(b => b.OrderId)
    .IsRequired(false); //

    //    modelBuilder.Entity<Basket>()
    //.HasOne(b => b.MediaItem)
    //.WithMany()
    //.HasForeignKey(b => b.MediaItemId)
    //.OnDelete(DeleteBehavior.NoAction); // ya da

        modelBuilder.Entity<MediaItem>()
       .HasMany(m => m.Baskets)
       .WithOne(b => b.MediaItem)
       .HasForeignKey(b => b.MediaItemId)
       .OnDelete(DeleteBehavior.Restrict); // veya SetNull/Cascade

        modelBuilder.Entity<AppSetting>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Key).IsUnique();
            entity.Property(e => e.Value).IsRequired();
        });

        modelBuilder.ApplyConfiguration(new AppSettingConfiguration());



        //modelBuilder.Entity<Basket>()
        //    .HasOne(b => b.Order)
        //    .WithMany(o => o.Baskets)
        //    .HasForeignKey(b => b.OrderId);

        //modelBuilder.Entity<Basket>()
        //    .HasOne(b => b.User)
        //    .WithMany()
        //    .HasForeignKey(b => b.UserId);

    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Entity>();

        Guid? userId = null;
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null && httpContext.User.Identity is { IsAuthenticated: true })
        {
            var userIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "user-id");

            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var parsedUserId))
            {
                userId = parsedUserId;
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
                throw new ArgumentException($"'{entry.Entity.GetType().Name}' nesnesi veritabanından doğrudan silinemez. Soft-delete uygulanmalıdır.");
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

}