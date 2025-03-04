using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OzdamarDepo.Application.MediaItems;
using MediatR;
using TS.Result;
using OzdamarDepo.Domain.MediaItems;

namespace OzdamarDepo.WebAPI.Modules;

public static class MediaItemModule
{
    public static void RegisterMediaItemRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/mediaitems").WithTags("MediaItems");

        group.MapPost("",
            async (ISender sender, MediaItemCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();

        group.MapGet(string.Empty,
             async (ISender sender, Guid id, CancellationToken cancellationToken) =>
             {
                 var response = await sender.Send(new MediaItemGetQuery(id), cancellationToken);
                 return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
             })
             .Produces<Result<MediaItem>>();
    }
}