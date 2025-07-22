using MediatR;
using Microsoft.AspNetCore.Identity;
using OzdamarDepo.Application.Auth.Dtos;
using OzdamarDepo.Application.Services;
using OzdamarDepo.Domain.Users;
using TS.Result;

namespace OzdamarDepo.Application.Auth;

public sealed class LoginCommandHandler(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    IJwtProvider jwtProvider)
    : IRequestHandler<LoginCommand, Result<LoginResultDto>>
{
    public async Task<Result<LoginResultDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.UserNameOrEmail)
                   ?? await userManager.FindByNameAsync(request.UserNameOrEmail);

        if (user is null)
            return Result<LoginResultDto>.Failure("Kullanıcı bulunamadı");

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
            return Result<LoginResultDto>.Failure("Şifre hatalı");

        var token = await jwtProvider.CreateTokenAsync(user, cancellationToken);

        var userDto = new LoginResponseDto
        {
            Id = user.Id.ToString(),
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            Email = user.Email ?? string.Empty,
            TcNo = user.TcNo ?? string.Empty,
            Phone = user.PhoneNumber ?? string.Empty
        };

        return Result<LoginResultDto>.Succeed(new LoginResultDto
        {
            User = userDto,
            AccessToken = token
        });
    }
}
