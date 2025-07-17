using MediatR;
using Microsoft.AspNetCore.Identity;
using OzdamarDepo.Domain.Abstractions;
using OzdamarDepo.Domain.Users;

namespace OzdamarDepo.Application.Users
{
    public sealed record UserGetAllQuery() : IRequest<IQueryable<UserGetAllQueryResponse>>;

    public sealed class UserGetAllQueryResponse : EntityDto
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;

        public string Password { get; set; } = default!;
      
    }

    internal sealed class UserGetAllQueryHandler(IUserRepository userRepository, UserManager<AppUser> userManager) : IRequestHandler<UserGetAllQuery, IQueryable<UserGetAllQueryResponse>>
    {
        public Task<IQueryable<UserGetAllQueryResponse>> Handle(UserGetAllQuery request, CancellationToken cancellationToken)
        {
            var users = userRepository.GetAll().ToList();
            var appUsers = userManager.Users.ToList();

            var response = users.Select(user =>
            {
                var Email=appUsers.FirstOrDefault(u => u.Id == user.Id)?.Email ?? string.Empty;
                var UserName = appUsers.FirstOrDefault(u => u.Id == user.Id)?.UserName ?? string.Empty;
                var Password = appUsers.FirstOrDefault(u => u.Id == user.Id)?.PasswordHash ?? string.Empty;
              
                var createUser = appUsers.FirstOrDefault(u => u.Id == user.CreateUserId);
                var updateUser = appUsers.FirstOrDefault(u => u.Id == user.UpdateUserId);

                return new UserGetAllQueryResponse
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = UserName,
                    Email = Email,
                    Password = Password,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt,
                    DeletedAt = user.DeletedAt,
                    Id = user.Id,
                    IsDeleted = user.IsDeleted,
                    CreateUserId = user.CreateUserId,
                    UpdateUserId = user.UpdateUserId,
                    CreateUserName = createUser is not null ? $"{createUser.FirstName} {createUser.LastName} ({createUser.Email})" : null,
                    UpdateUserName = updateUser is not null ? $"{updateUser.FirstName} {updateUser.LastName} ({updateUser.Email})" : null
                };
            }).AsQueryable();

            return Task.FromResult(response);

        }
    }
}
