using MediatR;
using TS.Result;

namespace OzdamarDepo.Application.GenelAyarlar.Queries;

public sealed record AppSettingGetByKeyQuery(string Key) : IRequest<Result<string>>;
