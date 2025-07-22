using FluentValidation;
using GenericRepository;
using MediatR;
using OzdamarDepo.Domain.Baskets;
using OzdamarDepo.Domain.MediaItems;
using TS.Result;

namespace OzdamarDepo.Application.MediaItems
{
    // 🔁 string yerine Guid döndürüyoruz
    public sealed record BasketCreateCommand(
        Guid UserId,
        Guid MediaItemId,
        string MediaItemTitle,
        decimal MediaItemPrice,
        int Quantity,
        string MediaItemImageUrl
    ) : IRequest<Result<Guid>>;

    public sealed class BasketCreateCommandValidator : AbstractValidator<BasketCreateCommand>
    {
        public BasketCreateCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.MediaItemId).NotEmpty();
            RuleFor(x => x.Quantity).GreaterThan(0);
            RuleFor(x => x.MediaItemTitle).NotEmpty();
            RuleFor(x => x.MediaItemPrice).GreaterThan(0);
        }
    }

    public sealed class BasketCreateCommandHandler(
        IBasketRepository basketRepository,
        IMediaItemRepository mediaItemRepository,
        IUnitOfWork unitOfWork
    ) : IRequestHandler<BasketCreateCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(BasketCreateCommand request, CancellationToken cancellationToken)
        {
            var mediaItem = await mediaItemRepository.GetByIdAsync(request.MediaItemId);
            if (mediaItem == null)
                return Result<Guid>.Failure("Media item bulunamadı!");

            var basket = new Basket
            {
                UserId = request.UserId,
                MediaItemId = mediaItem.Id,
                MediaItemTitle = mediaItem.Title,
                MediaItemPrice = mediaItem.Price,
                MediaItemImageUrl = mediaItem.ImageUrl,
                Quantity = request.Quantity,
                IsInBasket = true
            };

            await basketRepository.AddAsync(basket);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 🟢 Sepetin Id'si döndürülüyor
            return Result<Guid>.Succeed(basket.Id);
        }
    }
}
