using GenericRepository;
using MediatR;
using OzdamarDepo.Application.Orders;
using OzdamarDepo.Domain.Orders;
using TS.Result;

public sealed class OrderUpdateCargoStatusCommandHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<OrderUpdateCargoStatusCommand, Result<string>>
{
    public async Task<Result<string>> Handle(OrderUpdateCargoStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (order is null)
            return Result<string>.Failure("Sipariş bulunamadı!");

        order.CargoStatus = request.CargoStatus;
        order.UpdatedAt = DateTime.UtcNow;

        orderRepository.Update(order);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Kargo durumu başarıyla güncellendi!";
    }
}

