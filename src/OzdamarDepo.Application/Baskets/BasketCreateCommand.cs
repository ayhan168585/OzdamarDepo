using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using OzdamarDepo.Domain.Baskets;
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
        IUnitOfWork unitOfWork) : IRequestHandler<BasketCreateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(BasketCreateCommand request, CancellationToken cancellationToken)
        {

            Basket basket = request.Adapt<Basket>();

            basket.MediaItemTitle = request.MediaItemTitle;
            basket.MediaItemImageUrl = request.MediaItemImageUrl;
            basket.UserId = request.UserId;
            basket.MediaItemId = request.MediaItemId;
            basket.MediaItemPrice = request.MediaItemPrice;
            basket.Quantity = request.Quantity;


           
            await basketRepository.AddAsync(basket);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Sepet başarıyla eklendi!");
        }

        
    }
}
