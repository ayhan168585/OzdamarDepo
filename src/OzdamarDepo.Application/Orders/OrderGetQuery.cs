using Mapster;
using MediatR;
using OzdamarDepo.Domain.Orders;
using TS.Result;

namespace OzdamarDepo.Application.Orders
{
    public sealed record OrderGetQuery(Guid Id) : IRequest<Result<OrderGetQueryResponse>>;


    public sealed class OrderGetQueryHandler(IOrderRepository orderRepository) : IRequestHandler<OrderGetQuery, Result<OrderGetQueryResponse>>
    {
        public async Task<Result<OrderGetQueryResponse>> Handle(OrderGetQuery request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (order is null)
                return Result<OrderGetQueryResponse>.Failure("Sipariş bulunamadı!");

            var dto = order.Adapt<OrderGetQueryResponse>();
            return dto;
        }
    }
}
