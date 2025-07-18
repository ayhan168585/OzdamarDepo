using MediatR;
using OzdamarDepo.Domain.Orders;
using TS.Result;

namespace OzdamarDepo.Application.Orders
{
    public sealed record OrderGetQuery(
         Guid Id) : IRequest<Result<Order>>;

    public sealed class OrderGetQueryHandler(IOrderRepository orderRepository) : IRequestHandler<OrderGetQuery, Result<Order>>
    {
        public async Task<Result<Order>> Handle(OrderGetQuery request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (order is null)
            {
                return Result<Order>.Failure("Sipariş bulunamadı!");
            }

            return order;
        }
    }
}
