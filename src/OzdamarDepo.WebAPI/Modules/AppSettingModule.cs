using MediatR;
using Microsoft.AspNetCore.Mvc;
using OzdamarDepo.Application.Genel_Ayarlar;
using OzdamarDepo.Application.GenelAyarlar.Commands;
using OzdamarDepo.Application.GenelAyarlar.Queries;

namespace OzdamarDepo.WebAPI.Modules
{
    public static class AppSettingModule
    {
        public static void RegisterAppSettingEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/app-settings").WithTags("Genel Ayarlar");

            // CREATE
            group.MapPost("", async (
                ISender sender,
                [FromBody] AppSettingCreateCommand command,
                CancellationToken ct) =>
            {
                var result = await sender.Send(command, ct);
                return result.IsSuccessful ? Results.Ok(result) : Results.BadRequest(result);
            });

            // GET ALL
            group.MapGet("", async (ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new AppSettingGetAllQuery(), ct);
                return Results.Ok(result);
            });

            // GET BY KEY
            group.MapGet("by-key/{key}", async (ISender sender, string key, CancellationToken ct) =>
            {
                var result = await sender.Send(new AppSettingGetByKeyQuery(key), ct);
                return result.IsSuccessful ? Results.Ok(result.Data) : Results.NotFound(result.ErrorMessages);
            });

            // UPDATE
            group.MapPut("{id}", async (
                ISender sender,
                [FromBody] AppSettingUpdateCommand command,
                CancellationToken ct) =>
            {
                var result = await sender.Send(command, ct);
                return result.IsSuccessful ? Results.Ok(result) : Results.BadRequest(result);
            });

            // DELETE
            group.MapDelete("{key}", async (
                ISender sender,
                string key,
                CancellationToken ct) =>
            {
                var result = await sender.Send(new AppSettingDeleteCommand(key), ct);
                return result.IsSuccessful ? Results.Ok(result) : Results.NotFound(result);
            });
        }
    }
}
