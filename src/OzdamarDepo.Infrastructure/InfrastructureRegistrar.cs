using OzdamarDepo.Domain.Users;
using OzdamarDepo.Infrastructure.Context;
using GenericRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using OzdamarDepo.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace OzdamarDepo.Infrastructure
{
    public static class InfrastructureRegistrar
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                string connectionString = configuration.GetConnectionString("SqlServer")!;
                opt.UseSqlServer(connectionString);
            });

            services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<ApplicationDbContext>());

            services
     .AddIdentityCore<AppUser>(options =>
     {
         options.Password.RequiredLength = 1;
         options.Password.RequireNonAlphanumeric = false;
         options.Password.RequireUppercase = false;
         options.Password.RequireLowercase = false;
         options.Password.RequireDigit = false;
         options.SignIn.RequireConfirmedEmail = true;
         options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
         options.Lockout.MaxFailedAccessAttempts = 3;
         options.Lockout.AllowedForNewUsers = true;
     })
     .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.ConfigureOptions<JwtOptionsSetup>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
            services.AddAuthorization();


            // SignInManager'i elle ekleyelim
            services.AddScoped<SignInManager<AppUser>>();
            services.AddHttpContextAccessor();




            services.Scan(opt => opt.FromAssemblies(typeof(InfrastructureRegistrar).Assembly)
            .AddClasses(publicOnly: false)
             .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

            return services;




        }
    }
}