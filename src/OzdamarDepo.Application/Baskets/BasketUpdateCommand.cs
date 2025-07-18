using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using OzdamarDepo.Domain.Baskets;
using TS.Result;

namespace OzdamarDepo.Application.Baskets
{
    public sealed record BasketUpdateCommand(
        Guid Id,
        Guid UserId,
        Guid MediaItemId,
        string MediaItemTitle,
        decimal MediaItemPrice,
        int Quantity,
        string MediaItemImageUrl) : IRequest<Result<string>>;

    public sealed class BasketUpdateCommandValidator : AbstractValidator<BasketUpdateCommand>
    {
        public BasketUpdateCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("Kullanıcı boş olamaz!");
            RuleFor(x => x.MediaItemId).NotEmpty().WithMessage("Ürün boş olamaz!");
            RuleFor(x => x.MediaItemPrice).GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır!");
            RuleFor(x => x.MediaItemTitle).NotEmpty().WithMessage("Ürün adı boş olamaz!");
            RuleFor(x => x.MediaItemImageUrl).NotEmpty().WithMessage("Ürün görseli boş olamaz!");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Adet 0'dan büyük olmalıdır!");
            RuleFor(x => x.Id).NotEmpty().WithMessage("Sepet öğesi ID'si boş olamaz!");


        }
    }

    public sealed class BasketUpdateCommandHandler(
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork) : IRequestHandler<BasketUpdateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(BasketUpdateCommand request, CancellationToken cancellationToken)
        {

            var basket = await basketRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (basket is null)
            {
                return Result<string>.Failure("Sepet bulunamadı!");
            }

           

            request.Adapt(basket);

            basket.MediaItemTitle = request.MediaItemTitle;
            basket.MediaItemImageUrl = request.MediaItemImageUrl;
            basket.UserId = request.UserId;
            basket.MediaItemId = request.MediaItemId;
            basket.MediaItemPrice = request.MediaItemPrice;
            basket.Quantity = request.Quantity;
            basket.UpdateUserId = request.UserId; // Güncelleyen kullanıcı ID'si
            basket.UpdatedAt = DateTime.UtcNow; // Güncelleme zamanı
            basket.IsDeleted = false; // Silinmemiş olarak işaretle
            basket.CreatedAt = basket.CreatedAt == default ? DateTime.UtcNow : basket.CreatedAt; // İlk oluşturulma zamanı
            basket.CreateUserId = basket.CreateUserId == default ? request.UserId : basket.CreateUserId; // Oluşturan kullanıcı ID'si




            basketRepository.Update(basket);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Sepet öğesi başarıyla güncellendi!";
        }

       
    }
}
