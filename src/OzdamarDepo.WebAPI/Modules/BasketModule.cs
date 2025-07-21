using MediatR;
using OzdamarDepo.Application.Baskets;
using OzdamarDepo.Application.MediaItems;
using OzdamarDepo.Domain.Baskets;
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

            group.MapPut("{id}",
         async (Guid id, ISender sender, BasketUpdateCommand request, CancellationToken cancellationToken) =>
         {
             if (id != request.Id)
                 return Results.BadRequest("URL'deki id ile gönderilen id eşleşmiyor");

             var response = await sender.Send(request, cancellationToken);
             return response.IsSuccessful
                 ? Results.Ok(response)
                 : Results.InternalServerError(response);
         })
         .Produces<Result<string>>()
         .WithName("BasketUpdate");



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

            group.MapGet("{id}",
                 async (ISender sender, Guid id, CancellationToken cancellationToken) =>
                 {
                     var response = await sender.Send(new BasketGetQuery(id), cancellationToken);
                     return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
                 })
                 .Produces<Result<Basket>>().
                 WithName("BasketGet");

            group.MapDelete("{id}",
               async (Guid Id, ISender sender, CancellationToken cancellationToken) =>
               {
                   var response = await sender.Send(new BasketDeleteCommand(Id), cancellationToken);
                   return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
               })

           .Produces<Result<string>>().
               WithName("BasketDelete");
        }
    }
}
