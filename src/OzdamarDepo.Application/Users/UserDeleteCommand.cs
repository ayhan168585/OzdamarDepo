﻿using GenericRepository;
using MediatR;
using OzdamarDepo.Domain.MediaItems;
using OzdamarDepo.Domain.Users;
using TS.Result;

namespace OzdamarDepo.Application.Users
{
    public sealed record UserDeleteCommand(
     Guid Id) : IRequest<Result<string>>;

    internal sealed class UserDeleteCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : IRequestHandler<UserDeleteCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
        {
            AppUser? user = await userRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (user is null)
            {
                return Result<string>.Failure("Kullanıcı bulunamadı!");
            }          

            user.IsDeleted = true;
            userRepository.Update(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return ("Kullanıcı başarıyla silindi!");

        }
    }
}
