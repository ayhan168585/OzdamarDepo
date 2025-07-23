using MediatR;
using Microsoft.AspNetCore.Mvc;
using OzdamarDepo.Application.Orders;
using OzdamarDepo.Domain.Orders;
using TS.Result;

namespace OzdamarDepo.WebAPI.Modules
{
    public static class OrderModule
    {
        public static void RegisterOrderRoutes(this IEndpointRouteBuilder app)
        {
            RouteGroupBuilder group = app.MapGroup("/orders").WithTags("orders");

            group.MapPost("",
    async (ISender sender, [FromBody] OrderCreateCommand request, CancellationToken cancellationToken) =>
{
    var response = await sender.Send(request, cancellationToken);
    return response!.IsSuccessful
        ? Results.Ok(response)
        : Results.InternalServerError(response);
})
                .Produces<Result<string>>().
                WithName("OrderCreate");

            group.MapPut(string.Empty,
         async (ISender sender, OrderUpdateCommand request, CancellationToken cancellationToken) =>
         {
             var response = await sender.Send(request, cancellationToken);
             return response.IsSuccessful
                 ? Results.Ok(response)
                 : Results.InternalServerError(response);
         })
         .Produces<Result<string>>().
         WithName("OrderUpdate");

            group.MapPut("update-cargo-status",
                async (ISender sender, [FromBody] OrderUpdateCargoStatusCommand request, CancellationToken cancellationToken) =>
                {
                    var response = await sender.Send(request, cancellationToken);
                    return response.IsSuccessful
                        ? Results.Ok(response)
                        : Results.InternalServerError(response);
                })
                .Produces<Result<string>>()
                .WithName("OrderUpdateCargoStatus");





            group.MapGet("{id}",
                 async (ISender sender, Guid id, CancellationToken cancellationToken) =>
                 {
                     var response = await sender.Send(new OrderGetQuery(id), cancellationToken);
                     return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
                 })
                 .Produces<Result<Order>>().
                 WithName("OrderGet");

            group.MapDelete("{id}",
               async (Guid Id, ISender sender, CancellationToken cancellationToken) =>
               {
                   var response = await sender.Send(new OrderDeleteCommand(Id), cancellationToken);
                   return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
               })

           .Produces<Result<string>>().
               WithName("OrderDelete");

            group.MapGet("user/{userId}",
    async (Guid userId, ISender sender, CancellationToken cancellationToken) =>
    {
        var response = await sender.Send(new OrderGetAllQuery(userId), cancellationToken);
        return Results.Ok(response);
    })
    .Produces<List<OrderGetAllQueryResponse>>()
    .WithName("OrderGetAllByUser");

        }
    }

}
