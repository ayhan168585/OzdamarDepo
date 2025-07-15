using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OzdamarDepo.Domain.Abstractions;
using OzdamarDepo.Domain.MediaItems;
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
            var response = (from user in userRepository.GetAll()
                            join create_user in userManager.Users.AsQueryable() on user.CreateUserId equals create_user.Id
                            join update_user in userManager.Users.AsQueryable() on user.UpdateUserId equals update_user.Id into update_user
                            from update_users in update_user.DefaultIfEmpty()
                            select new UserGetAllQueryResponse
                            {
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                CreatedAt = user.CreatedAt,
                                UpdatedAt = user.UpdatedAt,
                                DeletedAt = user.DeletedAt,
                                Id = user.Id,
                                IsDeleted = user.IsDeleted,
                                CreateUserId = user.CreateUserId,
                                UpdateUserId = user.UpdateUserId,
                                CreateUserName = create_user.FirstName + " " + create_user.LastName + "(" + create_user.Email + ")",
                                UpdateUserName = user.UpdateUserId == null ? null : create_user.FirstName + " " + create_user.LastName + "(" + create_user.Email + ")"



                            }).AsQueryable();
            return Task.FromResult(response);
        }
    }
}
