﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OzdamarDepo.Application.Services;
using OzdamarDepo.Domain.Users;
using TS.Result;

namespace OzdamarDepo.Application.Auth
{
    public sealed class LoginCommandHandler(UserManager<AppUser> usermanager, SignInManager<AppUser> signInManager, IJwtProvider jwtProvider) : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
    {
        public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            AppUser? user = await usermanager.Users.FirstOrDefaultAsync(p => p.Email == request.UserNameOrEmail || p.UserName == request.UserNameOrEmail, cancellationToken);

            if (user is null)
            {
                return Result<LoginCommandResponse>.Failure("Kullanıcı bulunamadı!");
            }

            SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, true);

            if (signInResult.IsLockedOut)
            {
                TimeSpan? timeSpan = user.LockoutEnd - DateTime.UtcNow;
                if (timeSpan is not null)
                    return (500, $"Şifrenizi 5 defa yanlış girdiğiniz için kullanıcı {Math.Ceiling(timeSpan.Value.TotalMinutes)} dakika süreyle bloke edilmiştir");
                else
                    return (500, "Kullanıcınız 5 kez yanlış şifre girdiği için 5 dakika süreyle bloke edilmiştir");
            }

            if (signInResult.IsNotAllowed)
            {
                return (500, "Mail adresiniz onaylı değil");
            }

            if (!signInResult.Succeeded)
            {
                return (500, "Şifreniz yanlış");
            }

            //token üret

            var token = await jwtProvider.CreateTokenAsync(user, cancellationToken);

            var response = new LoginCommandResponse()
            {
                AccessToken = token,
                User = new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                }
            };

            return response;
        }
    }
}
