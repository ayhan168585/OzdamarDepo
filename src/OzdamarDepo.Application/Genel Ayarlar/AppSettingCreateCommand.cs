using MediatR;
using OzdamarDepo.Domain.Genel_Ayarlar;
using TS.Result;

namespace OzdamarDepo.Application.GenelAyarlar.Commands;

public sealed class AppSettingCreateCommand : IRequest<Result<string>>
{
    public string Key { get; init; }= string.Empty;
    public string Value { get; init; }= string.Empty;
    public AppSettingValueType ValueType { get; init; }
}
