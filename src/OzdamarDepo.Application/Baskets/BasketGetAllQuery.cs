namespace OzdamarDepo.Application.Baskets
{
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using OzdamarDepo.Domain.Abstractions;
    using OzdamarDepo.Domain.Baskets;
    using OzdamarDepo.Domain.Users;

    public sealed record BasketGetAllQuery() : IRequest<IQueryable<BasketGetAllQueryResponse>>;

    public sealed class BasketGetAllQueryResponse : EntityDto
    {
        public Guid UserId { get; set; }
        public Guid MediaItemId { get; set; }
        public string MediaItemTitle { get; set; } = default!;
        public decimal MediaItemPrice { get; set; }
        public int Quantity { get; set; } = 1;
        public string MediaItemImageUrl { get; set; } = default!;
    }

    public sealed class BasketsGetAllQueryHandler(IBasketRepository basketRepository, UserManager<AppUser> userManager) : IRequestHandler<BasketGetAllQuery, IQueryable<BasketGetAllQueryResponse>>
    {
        public Task<IQueryable<BasketGetAllQueryResponse>> Handle(BasketGetAllQuery request, CancellationToken cancellationToken)
        {
            var response = (from basket in basketRepository.GetAll()
                            join create_user in userManager.Users.AsQueryable() on basket.CreateUserId equals create_user.Id
                            join update_user in userManager.Users.AsQueryable() on basket.UpdateUserId equals update_user.Id into update_user
                            from update_users in update_user.DefaultIfEmpty()
                            select new BasketGetAllQueryResponse
                            {
                                UserId = basket.UserId,
                                MediaItemId = basket.MediaItemId,
                                MediaItemTitle = basket.MediaItemTitle,
                                MediaItemPrice = basket.MediaItemPrice,
                                Quantity = basket.Quantity,
                              
                                CreatedAt = basket.CreatedAt,
                                UpdatedAt = basket.UpdatedAt,
                                DeletedAt = basket.DeletedAt,
                                Id = basket.Id,
                                IsDeleted = basket.IsDeleted,
                                MediaItemImageUrl = basket.MediaItemImageUrl,
                                CreateUserId = basket.CreateUserId,
                                UpdateUserId = basket.UpdateUserId,
                                CreateUserName = create_user.FirstName + " " + create_user.LastName + "(" + create_user.Email + ")",
                                UpdateUserName = basket.UpdateUserId == null ? null : create_user.FirstName + " " + create_user.LastName + "(" + create_user.Email + ")"



                            }).AsQueryable();
            return Task.FromResult(response);
        }
    }

}
