﻿using MediatR;
using OzdamarDepo.Application.Auth;
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
               
        }
    }
}
