using GenericRepository;

namespace OzdamarDepo.Domain.Baskets
{
    public interface IBasketRepository : IRepository<Basket>
    {
        Task<List<Basket>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken);
        Task UpdateRangeAsync(IEnumerable<Basket> baskets);
    }
}
