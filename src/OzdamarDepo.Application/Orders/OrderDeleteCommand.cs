using GenericRepository;
using MediatR;
using OzdamarDepo.Domain.Orders;
using TS.Result;

namespace OzdamarDepo.Application.Orders
{
    public sealed record OrderDeleteCommand(
     Guid Id) : IRequest<Result<string>>;

    public sealed class OrderDeleteCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork) : IRequestHandler<OrderDeleteCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(OrderDeleteCommand request, CancellationToken cancellationToken)
        {
            Order? order = await orderRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (order is null)
            {
                return Result<string>.Failure("Sipariş bulunamadı!");
            }



            order.IsDeleted = true;
            orderRepository.Update(order);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return ("Sipariş başarıyla silindi!");

        }
    }
}
