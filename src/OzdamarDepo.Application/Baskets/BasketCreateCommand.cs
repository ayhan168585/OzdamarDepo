using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using OzdamarDepo.Domain.Baskets;
using OzdamarDepo.Domain.MediaItems;
using TS.Result;

namespace OzdamarDepo.Application.MediaItems
{
    public sealed record BasketCreateCommand(
    Guid UserId,
    Guid MediaItemId,
    string MediaItemTitle,
    decimal MediaItemPrice,
    int Quantity,
    string MediaItemImageUrl) : IRequest<Result<string>>;

    public sealed class BasketCreateCommandValidator : AbstractValidator<BasketCreateCommand>
    {
        public BasketCreateCommandValidator()
        {
           
        }
    }

    public sealed class BasketCreateCommandHandler(
    IBasketRepository basketRepository,
    IMediaItemRepository mediaItemRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<BasketCreateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(BasketCreateCommand request, CancellationToken cancellationToken)
        {
            var mediaItem = await mediaItemRepository.GetByIdAsync(request.MediaItemId);
            if (mediaItem == null)
                return Result<string>.Failure("Media item bulunamadı!");

            Basket basket = new()
            {
                UserId = request.UserId,
                MediaItemId = mediaItem.Id,
                MediaItemTitle = mediaItem.Title,
                MediaItemPrice = mediaItem.Price,
                MediaItemImageUrl = mediaItem.ImageUrl,
                Quantity = request.Quantity,
                IsInBasket = true // ✅ Yeni eklenen ürün sepette olarak işaretleniyor

            };

            await basketRepository.AddAsync(basket);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Sepet başarıyla eklendi!");
        }
    }

}
