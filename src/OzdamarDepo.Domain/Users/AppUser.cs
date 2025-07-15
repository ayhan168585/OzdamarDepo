using Microsoft.AspNetCore.Identity;

namespace OzdamarDepo.Domain.Users
{
    public sealed class AppUser : IdentityUser<Guid>
    {
        public AppUser()
        {
            Id = Guid.CreateVersion7();
        }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string FullName => $"{FirstName} {LastName}";


        public DateTimeOffset CreatedAt { get; set; }
        public Guid CreateUserId { get; set; } = default!;
        public DateTimeOffset? UpdatedAt { get; set; }
        public Guid? UpdateUserId { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? DeleteUserId { get; set; }
        public DateTimeOffset DeletedAt { get; set; }
    }
}
