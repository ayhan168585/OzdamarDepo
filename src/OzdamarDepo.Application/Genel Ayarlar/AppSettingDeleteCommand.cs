using MediatR;
using TS.Result;

namespace OzdamarDepo.Application.GenelAyarlar.Commands;

public sealed record AppSettingDeleteCommand(string Key) : IRequest<Result<string>>;
