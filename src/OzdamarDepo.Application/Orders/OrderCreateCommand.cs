using FluentValidation;
using GenericRepository;
using MediatR;
using OzdamarDepo.Domain.Baskets;
using OzdamarDepo.Domain.Orders;
using TS.Result;

namespace OzdamarDepo.Application.Orders
{
    public record OrderCreateCommand(

    string OrderNumber,
    DateTimeOffset Date,
    Guid UserId,
    string FullName,
    string PhoneNumber,
    string City,
    string District,
    string FullAdress,
    CargoStatusEnum CargoStatus, // 👈 sadece tip ve ad
    List<Guid> BasketIds


    ) : IRequest<Result<string>>;

    public sealed class OrderCreateCommandValidator : AbstractValidator<OrderCreateCommand>
    {
        public OrderCreateCommandValidator()
        {
            RuleFor(x => x.OrderNumber).NotEmpty().WithMessage("Order No boş olamaz!");
            RuleFor(x => x.Date).NotEmpty().WithMessage("Tarih boş olamaz!");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("Kullanıcı ID boş olamaz!");
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Ad Soyad boş olamaz!");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Telefon Numarası boş olamaz!");
            RuleFor(x => x.City).NotEmpty().WithMessage("Şehir boş olamaz!");
            RuleFor(x => x.District).NotEmpty().WithMessage("İlçe boş olamaz!");
            RuleFor(x => x.FullAdress).NotEmpty().WithMessage("Adres boş olamaz!");
            RuleFor(x => x.CargoStatus)
                .IsInEnum().WithMessage("Geçersiz kargo durumu!");

        }
    }

    public sealed class OrderCreateCommandHandler(
  IOrderRepository orderRepository,
  IUnitOfWork unitOfWork,
  IBasketRepository basketRepository,
  IOrderBasketCleaner basketCleaner)
  : IRequestHandler<OrderCreateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(OrderCreateCommand request, CancellationToken cancellationToken)
        {
            var baskets = await basketRepository.GetByIdsAsync(request.BasketIds, cancellationToken);
            var orderId = Guid.NewGuid();

            Order order = new()
            {
                Id = orderId,
                OrderNumber = request.OrderNumber,
                Date = request.Date,
                UserId = request.UserId,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                City = request.City,
                District = request.District,
                FullAdress = request.FullAdress,
                CargoStatus = CargoStatusEnum.Bekliyor,
                Baskets = baskets
            };

            foreach (var basket in baskets)
            {
                basket.OrderId = order.Id;
                basket.IsInBasket = false; // ✅ EKLENDİ
            }

            await basketRepository.UpdateRangeAsync(baskets);
            await orderRepository.AddAsync(order);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await basketCleaner.ClearUserBasketsAsync(request.UserId, cancellationToken);

            return Result<string>.Succeed("Sipariş başarıyla eklendi!");
        }
    }





}
