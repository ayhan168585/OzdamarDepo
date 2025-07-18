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
        Guid Id,
       string OrderNumber,
    DateTimeOffset Date,
    Guid UserId,
    string FullName,
    string PhoneNumber,
    string City,
    string District,
    string FullAdress,
    string CartNumber,
    string CartOwnerName,
    string ExpiresDate,
    int Cvv,
    string InstallmentOptions,
    string Status,
    List<Basket> Baskets) : IRequest<Result<string>>;

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
            RuleFor(x => x.CartNumber).NotEmpty().WithMessage("Kart Numarası boş olamaz!");
            RuleFor(x => x.CartOwnerName).NotEmpty().WithMessage("Kart Sahibi Adı boş olamaz!");
            RuleFor(x => x.ExpiresDate).NotEmpty().WithMessage("Son Kullanma Tarihi boş olamaz!");
            RuleFor(x => x.Cvv).GreaterThan(0).WithMessage("CVV 0'dan büyük olmalıdır!");
            RuleFor(x => x.InstallmentOptions).NotEmpty().WithMessage("Taksit Seçenekleri boş olamaz!");
            RuleFor(x => x.Status).NotEmpty().WithMessage("Sipariş Durumu boş olamaz!");


        }
    }

    public sealed class OrderUpdateCommandHandler(
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork) : IRequestHandler<OrderUpdateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(OrderUpdateCommand request, CancellationToken cancellationToken)
        {

            var order = await orderRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (order is null)
            {
                return Result<string>.Failure("Sipariş bulunamadı!");
            }



            request.Adapt(order);

            order.OrderNumber = request.OrderNumber;
            order.Date = request.Date;
            order.UserId = request.UserId;
            order.FullName = request.FullName;
            order.PhoneNumber = request.PhoneNumber;
            order.City = request.City;
            order.District = request.District;
            order.FullAdress = request.FullAdress;
            order.CartNumber = request.CartNumber;
            order.CartOwnerName = request.CartOwnerName;
            order.ExpiresDate = request.ExpiresDate;
            order.Cvv = request.Cvv;
            order.InstallmentOptions = request.InstallmentOptions;
            order.Status = request.Status;
            order.Baskets = request.Baskets;





            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Sipariş başarıyla güncellendi!";
        }


    }
}
