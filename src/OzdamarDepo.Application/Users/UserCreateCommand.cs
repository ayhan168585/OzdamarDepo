using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OzdamarDepo.Domain.Users;
using TS.Result;

namespace OzdamarDepo.Application.Auth
{
    public sealed record UserCreateCommand(
        string FirstName,
        string LastName,
        string UserName,
        string Email,
        string Password

        ) : IRequest<Result<string>>;
    public sealed class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
    {
        public UserCreateCommandValidator()
        {
            RuleFor(p => p.FirstName).NotEmpty().WithMessage("Kişi adı boş olamaz");
            RuleFor(p => p.FirstName).MinimumLength(2).WithMessage("Kişi adı en az 2 karakter olmalıdır");
            RuleFor(p => p.LastName).NotEmpty().WithMessage("Kişi soyadı boş olamaz");
            RuleFor(p => p.LastName).MinimumLength(2).WithMessage("Kişi soyadı en az 2 karakter olmalıdır");
            RuleFor(p => p.UserName).NotEmpty().WithMessage("Kullanıcıadı boş olamaz");
            RuleFor(p => p.UserName).MinimumLength(2).WithMessage("Kullanıcıadı en az 2 karakter olmalıdır");
            RuleFor(p => p.Email).NotEmpty().WithMessage("Email boş olamaz");
            RuleFor(p => p.Password).NotEmpty().WithMessage("Şifre boş olamaz");
            RuleFor(p => p.Password).MinimumLength(1).WithMessage("Şifre en az 1 karakter olmalıdır");
        }
    }

    internal sealed class UserCreateCommandHandler(UserManager<AppUser> userManager) : IRequestHandler<UserCreateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
        {
            var user = new AppUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                Email = request.Email,
                NormalizedEmail = request.Email.ToUpperInvariant(),
                NormalizedUserName = request.UserName.ToUpperInvariant(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                CreatedAt = DateTimeOffset.UtcNow,
                IsDeleted = false,
                DeletedAt = DateTimeOffset.MinValue,
            };

            IdentityResult result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return Result<string>.Failure(errors);
            }

            return "Kullanıcı başarıyla kaydedildi";
        }
    }
}
