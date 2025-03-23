using MediatR;
using OzdamarDepo.Application.MediaItems;
using OzdamarDepo.Domain.MediaItems;
using TS.Result;

namespace OzdamarDepo.WebAPI.Modules;

public static class MediaItemModule
{
    public static void RegisterMediaItemRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/mediaitems").WithTags("mediaItems");

        group.MapPost("",
            async (ISender sender, MediaItemCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.InternalServerError(response);
            })
            .Produces<Result<string>>().
            WithName("MediaItemCreate");

        group.MapPut(string.Empty,
          async (ISender sender, MediaItemUpdateCommand request, CancellationToken cancellationToken) =>
          {
              var response = await sender.Send(request, cancellationToken);
              return response.IsSuccessful
                  ? Results.Ok(response)
                  : Results.InternalServerError(response);
          })
          .Produces<Result<string>>().
          WithName("MediaItemUpdate");

        group.MapPut("update-status",
       async (ISender sender, MediaItemDurumUpdateCommand request, CancellationToken cancellationToken) =>
       {
           var response = await sender.Send(request, cancellationToken);
           return response.IsSuccessful
               ? Results.Ok(response)
               : Results.InternalServerError(response);
       })
       .Produces<Result<string>>().
       WithName("MediaItemDurumUpdate");

        group.MapGet("{id}",
             async (ISender sender, Guid id, CancellationToken cancellationToken) =>
             {
                 var response = await sender.Send(new MediaItemGetQuery(id), cancellationToken);
                 return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
             })
             .Produces<Result<MediaItem>>().
             WithName("MediaItemGet");

        group.MapDelete("{id}",
           async (Guid Id, ISender sender, CancellationToken cancellationToken) =>
           {
               var response = await sender.Send(new MediaItemDeleteCommand(Id), cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
           })

       .Produces<Result<string>>().
           WithName("MediaItemDelete");
    }
}