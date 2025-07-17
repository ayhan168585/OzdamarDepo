using MediatR;
using OzdamarDepo.Application.MediaItems;
using OzdamarDepo.Domain.MediaItems;
using TS.Result;

namespace OzdamarDepo.WebAPI.Modules
{
    public static class BasketModule
    {
        public static void RegisterBasketRoutes(this IEndpointRouteBuilder app)
        {
            RouteGroupBuilder group = app.MapGroup("/baskets").WithTags("baskets");

            group.MapPost("",
                async (ISender sender, BasketCreateCommand request, CancellationToken cancellationToken) =>
                {
                    var response = await sender.Send(request, cancellationToken);
                    return response.IsSuccessful
                        ? Results.Ok(response)
                        : Results.InternalServerError(response);
                })
                .Produces<Result<string>>().
                WithName("BasketCreate");

           

           // group.MapPut("update-status",
           //async (ISender sender, MediaItemDurumUpdateCommand request, CancellationToken cancellationToken) =>
           //{
           //    var response = await sender.Send(request, cancellationToken);
           //    return response.IsSuccessful
           //        ? Results.Ok(response)
           //        : Results.InternalServerError(response);
           //})
           //.Produces<Result<string>>().
           //WithName("MediaItemDurumUpdate");

           // group.MapGet("{id}",
           //      async (ISender sender, Guid id, CancellationToken cancellationToken) =>
           //      {
           //          var response = await sender.Send(new MediaItemGetQuery(id), cancellationToken);
           //          return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
           //      })
           //      .Produces<Result<MediaItem>>().
           //      WithName("MediaItemGet");

           // group.MapDelete("{id}",
           //    async (Guid Id, ISender sender, CancellationToken cancellationToken) =>
           //    {
           //        var response = await sender.Send(new MediaItemDeleteCommand(Id), cancellationToken);
           //        return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
           //    })

           //.Produces<Result<string>>().
           //    WithName("MediaItemDelete");
        }
    }
}
