using GenericRepository;
using MediatR;
using OzdamarDepo.Domain.Baskets;
using TS.Result;

namespace OzdamarDepo.Application.Baskets
{
    public sealed record BasketDeleteCommand(
     Guid Id) : IRequest<Result<string>>;

    public sealed class BasketDeleteCommandHandler(IBasketRepository basketRepository, IUnitOfWork unitOfWork) : IRequestHandler<BasketDeleteCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(BasketDeleteCommand request, CancellationToken cancellationToken)
        {
            Basket? basket = await basketRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (basket is null)
            {
                return Result<string>.Failure("Sepet bulunamadı!");
            }

           

            basket.IsDeleted = true;
            basketRepository.Update(basket);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return ("Sepet başarıyla silindi!");

        }
    }
}
