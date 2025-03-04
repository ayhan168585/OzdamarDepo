using MediatR;
using TS.Result;

namespace OzdamarDepo.Application.Auth
{
    public sealed record LoginCommand(
       string UserNameOrEmail,
       string Password) : IRequest<Result<LoginCommandResponse>>;
}
