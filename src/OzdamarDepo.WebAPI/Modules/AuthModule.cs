using MediatR;
using OzdamarDepo.Application.Auth;
using OzdamarDepo.Application.MediaItems;
using OzdamarDepo.Application.Users;
using TS.Result;

namespace OzdamarDepo.WebAPI.Modules
{
    public static class AuthModule
    {
        public static void RegisterAuthRoutes(this IEndpointRouteBuilder app)
        {
            RouteGroupBuilder group = app.MapGroup("/auth").WithTags("Auth");
            group.MapPost("login",
                async (ISender sender, LoginCommand request, CancellationToken cancellationToken) =>
                {
                    var response = await sender.Send(request, cancellationToken);
                    return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
                })
                .Produces<Result<LoginCommandResponse>>();

            group.MapPost("register",
               async (ISender sender, UserCreateCommand request, CancellationToken cancellationToken) =>
               {
                   var response = await sender.Send(request);
                   return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
               })
               .Produces<Result<string>>().AllowAnonymous();

            group.MapDelete("{id}",
           async (Guid Id, ISender sender, CancellationToken cancellationToken) =>
           {
               var response = await sender.Send(new UserDeleteCommand(Id), cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);

           })

       .Produces<Result<string>>().
           WithName("UserDelete");

        }
    }
}
