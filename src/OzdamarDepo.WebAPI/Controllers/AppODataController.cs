using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using OzdamarDepo.Application.Baskets;
using OzdamarDepo.Application.MediaItems;
using OzdamarDepo.Application.Orders;
using OzdamarDepo.Application.Users;

namespace OzdamarDepo.WebAPI.Controllers;

[Route("odata")]
[ApiController]
[EnableQuery]

public class AppODataController(ISender sender) : ODataController
{
    public static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();
        builder.EnableLowerCamelCase();
        builder.EntitySet<MediaItemGetAllQueryResponse>("mediaItems");
        builder.EntitySet<UserGetAllQueryResponse>("users");
        builder.EntitySet<BasketGetAllQueryResponse>("baskets");
        builder.EntitySet<OrderGetAllQueryResponse>("orders");
        builder.EntityType<OrderGetAllQueryResponse>()
                  .HasMany(o => o.Baskets); // Nested expand
        builder.EntityType<BasketGetAllQueryResponse>()
       .Property(x => x.UserId);
        builder.EntityType<OrderGetAllQueryResponse>()
       .Property(x => x.UserId); // tip zaten Guid

        //builder.EntityType<BasketWithMediaItemDto>()
        //       .HasRequired(b => b.MediaItem); // Eğer $expand=baskets/mediaItem yapmak istersen

        return builder.GetEdmModel();
    }

  

    [HttpGet("MediaItems")]
    [AllowAnonymous]
    public async Task<IQueryable<MediaItemGetAllQueryResponse>> GetAllMediaItems(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new MediaItemGetAllQuery(), cancellationToken);
        return response;
    }

    [HttpGet("users")]
    public async Task<IQueryable<UserGetAllQueryResponse>> GetAllUsers(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new UserGetAllQuery(), cancellationToken);
        return response;
    }

    [EnableQuery]
    [HttpGet("baskets")]
    public async Task<IQueryable<BasketGetAllQueryResponse>> GetAllBaskets(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new BasketGetAllQuery(), cancellationToken);
        return response;
    }

    //[EnableQuery]
    //[HttpGet("orders")]
    //public async Task<IQueryable<OrderGetAllQueryResponse>> GetAllOrders(CancellationToken cancellationToken)
    //{
    //    var response = await sender.Send(new OrderGetAllQuery(), cancellationToken);
    //    return response;
    //}
} 