using Microsoft.AspNetCore.Identity;
using OzdamarDepo.Domain.Users;

namespace OzdamarDepo.WebAPI
{
    public static class ExtensionsMiddleware
    {
        public static void CreateFirstUser(WebApplication app)
        {
            using (var scoped = app.Services.CreateScope())
            {
                var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                if (!userManager.Users.Any(p => p.UserName == "admin"))
                {
                    AppUser user = new()
                    {
                        Id = Guid.Parse("019582c7-2237-787e-b808-908e68e1cc1a"),
                        UserName = "admin",
                        Email = "admin@admin.com",
                        FirstName = "Ayhan",
                        LastName = "Özer",
                        EmailConfirmed = true,
                        CreatedAt = DateTimeOffset.Now,
                    };

                    user.CreateUserId = user.Id;

                    userManager.CreateAsync(user, "1").Wait();


                }
            }
        }
    }
}
