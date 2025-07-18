using MediatR;
using OzdamarDepo.Domain.Baskets;
using TS.Result;

namespace OzdamarDepo.Application.Baskets
{
    public sealed record BasketGetQuery(
         Guid Id) : IRequest<Result<Basket>>;

    public sealed class BasketGetQueryHandler(IBasketRepository basketRepository) : IRequestHandler<BasketGetQuery, Result<Basket>>
    {
        public async Task<Result<Basket>> Handle(BasketGetQuery request, CancellationToken cancellationToken)
        {
            var basket = await basketRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (basket is null)
            {
                return Result<Basket>.Failure("Sepet bulunamadı!");
            }

            return basket;
        }
    }
}
