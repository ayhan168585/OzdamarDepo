using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OzdamarDepo.Domain.Users;
using System;

namespace OzdamarDepo.Domain.Abstractions
{
    public abstract class Entity
    {
        public Entity()
        {
            Id = Guid.CreateVersion7();
           
        }

        public Guid Id { get; set; }

        #region Audit Log

        public bool IsActive { get; set; } = true;
        public DateTimeOffset CreatedAt { get; set; }
        public Guid CreateUserId { get; set; } = default!;
        public string CreateUserName => GetCreateUserName();  
        public DateTimeOffset? UpdatedAt { get; set; }
        public Guid? UpdateUserId { get; set; }
        public string UpdateUserName => GetUpdateUserName();     
        public bool IsDeleted { get; set; }
        public Guid? DeleteUserId { get; set; }
        public DateTimeOffset DeletedAt { get; set; }

        private string GetCreateUserName()
        {
            HttpContextAccessor httpContextAccessor = new();
            var userManager = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<UserManager<AppUser>>();
            AppUser appUser = userManager.Users.FirstOrDefault(p => p.Id == CreateUserId);

            if (appUser == null)
                return "Sistem";

            return appUser.FirstName + " " + appUser.LastName + " (" + appUser.Email + ")";
        }


        private string GetUpdateUserName()
        {
            if (UpdateUserId is null) return string.Empty;

            HttpContextAccessor httpContextAccessor = new();
            var userManager = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<UserManager<AppUser>>();
            AppUser appUser = userManager.Users.First(p => p.Id == UpdateUserId);
            return appUser.FirstName + " " + appUser.LastName + "(" + appUser.Email + ")";
        }

        #endregion


    }
}
 