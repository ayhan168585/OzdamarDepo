using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using OzdamarDepo.Application.MediaItems;
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
        return builder.GetEdmModel();
    }

  

    [HttpGet("MediaItems")]
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
} 