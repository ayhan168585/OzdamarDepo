using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using OzdamarDepo.Domain.Baskets;
using OzdamarDepo.Domain.Orders;
using TS.Result;

namespace OzdamarDepo.Application.Orders
{

    public sealed record OrderUpdateCommand(
            Guid OrderId, // 👈 EKLENDİ

  string OrderNumber,
    DateTimeOffset Date,
    Guid UserId,
    string FullName,
    string PhoneNumber,
    string City,
    string District,
    string FullAdress,
    CargoStatusEnum CargoStatus, // 👈 sadece tip ve ad
    List<Guid> BasketIds) : IRequest<Result<string>>;

    public sealed class OrderUpdateCommandValidator : AbstractValidator<OrderUpdateCommand>
    {
        public OrderUpdateCommandValidator()
        {
            RuleFor(x => x.OrderNumber).NotEmpty().WithMessage("Order No boş olamaz!");
            RuleFor(x => x.Date).NotEmpty().WithMessage("Tarih boş olamaz!");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("Kullanıcı ID boş olamaz!");
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Ad Soyad boş olamaz!");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Telefon Numarası boş olamaz!");
            RuleFor(x => x.City).NotEmpty().WithMessage("Şehir boş olamaz!");
            RuleFor(x => x.District).NotEmpty().WithMessage("İlçe boş olamaz!");
            RuleFor(x => x.FullAdress).NotEmpty().WithMessage("Adres boş olamaz!");


        }
    }

    public sealed class OrderUpdateCommandHandler(
      IOrderRepository orderRepository,
      IUnitOfWork unitOfWork,
      IBasketRepository basketRepository // 👈 Bunu ekle
  ) : IRequestHandler<OrderUpdateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(OrderUpdateCommand request, CancellationToken cancellationToken)
        {

            var order = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

            if (order is null)
            {
                return Result<string>.Failure("Sipariş bulunamadı!");
            }




            order.OrderNumber = request.OrderNumber;
            order.Date = request.Date;
            order.FullName = request.FullName;
            order.PhoneNumber = request.PhoneNumber;
            order.City = request.City;
            order.District = request.District;
            order.FullAdress = request.FullAdress;
            order.CargoStatus = request.CargoStatus;
            // Sepet sadece Bekliyor durumundayken güncellenebilir
            if (order.CargoStatus == CargoStatusEnum.Bekliyor)
            {
                var baskets = await basketRepository.GetByIdsAsync(request.BasketIds, cancellationToken);
                order.Baskets = baskets;
            }




            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Sipariş başarıyla güncellendi!";
        }


    }
}
