using MediatR;
using TS.Result;

namespace OzdamarDepo.Application.Orders;

public sealed record OrderUpdateCargoStatusCommand(
    Guid OrderId,
    CargoStatusEnum CargoStatus
) : IRequest<Result<string>>;
