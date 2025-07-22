using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OzdamarDepo.Application.MediaItems;
using OzdamarDepo.Domain.Abstractions;
using OzdamarDepo.Domain.MediaItems;
using TS.Result;

namespace OzdamarDepo.WebAPI.Modules;

public static class MediaItemModule
{
    public static void RegisterMediaItemRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/mediaitems").WithTags("mediaItems");

        group.MapPost("",
            async (
                HttpRequest httpRequest,
                [FromForm] MediaItemCreateRequest request,
                [FromForm] IFormFile imageFile,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                if (imageFile is null || imageFile.Length == 0)
                    return Results.BadRequest("Lütfen bir resim dosyası seçin.");

                // Resmi byte[]'e çevir
                using var memoryStream = new MemoryStream();
                await imageFile.CopyToAsync(memoryStream, cancellationToken);
                byte[] imageBytes = memoryStream.ToArray();

                // Command'e dönüştür
                var command = new MediaItemCreateCommand(
     Title: request.Title,
     ArtistOrActor: request.ArtistOrActor,
     MediaType: new MediaType
     {
         Category = request.MediaCategory,
         Format = request.MediaFormat
     },
     Price: request.Price,
     ReleaseDate: DateOnly.FromDateTime(request.ReleaseDate),
     MediaCondition: new MediaCondition
     {
         ConditionScore = request.ConditionScore,
         Description = request.ConditionDescription
     },
     IsBoxSet: request.IsBoxSet,
     DiscCount: request.DiscCount,
     Image: imageBytes,
     ImageFileName: imageFile.FileName,
     HttpRequest: httpRequest // ✅ EKLENDİ

 );

                var result = await sender.Send(command, cancellationToken);
                return result.IsSuccessful ? Results.Ok(result) : Results.InternalServerError(result);
            })
            .DisableAntiforgery()
            .Accepts<MediaItemCreateRequest>("multipart/form-data")
            .Produces<Result<string>>()
            .WithName("MediaItemCreate");

        group.MapPut(string.Empty,
            async (ISender sender, MediaItemUpdateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .WithName("MediaItemUpdate");

        group.MapPut("update-status",
            async (ISender sender, MediaItemDurumUpdateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .WithName("MediaItemDurumUpdate");

        group.MapGet("{id}",
            async (ISender sender, Guid id, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new MediaItemGetQuery(id), cancellationToken);
                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.InternalServerError(response);
            })
            .Produces<Result<MediaItem>>()
            .WithName("MediaItemGet");

        group.MapDelete("{id}",
            async (Guid Id, ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new MediaItemDeleteCommand(Id), cancellationToken);
                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .WithName("MediaItemDelete");
    }
}
