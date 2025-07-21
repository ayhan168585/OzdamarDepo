namespace OzdamarDepo.Application.Baskets
{
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using OzdamarDepo.Domain.Abstractions;
    using OzdamarDepo.Domain.Baskets;
    using OzdamarDepo.Domain.Users;

    public record BasketGetAllQuery() : IRequest<IQueryable<BasketGetAllQueryResponse>>;

    public class BasketGetAllQueryResponse : EntityDto
    {
        public Guid UserId { get; set; }
        public Guid MediaItemId { get; set; }
        public string MediaItemTitle { get; set; } = string.Empty;
        public decimal MediaItemPrice { get; set; }
        public int Quantity { get; set; } = 1;
        public string MediaItemImageUrl { get; set; } = string.Empty;
        public bool IsInBasket { get; init; } // ✅ Bunu ekle


        // Audit info

    }

    public class BasketGetAllQueryHandler(
        IBasketRepository basketRepository,
        UserManager<AppUser> userManager)
        : IRequestHandler<BasketGetAllQuery, IQueryable<BasketGetAllQueryResponse>>
    {
        public Task<IQueryable<BasketGetAllQueryResponse>> Handle(BasketGetAllQuery request, CancellationToken cancellationToken)
        {
            var query = from basket in basketRepository.GetAll()
                        join createUser in userManager.Users.AsQueryable()
                            on basket.CreateUserId equals createUser.Id into createUserGroup
                        from createUser in createUserGroup.DefaultIfEmpty()

                        join updateUser in userManager.Users.AsQueryable()
                            on basket.UpdateUserId equals updateUser.Id into updateUserGroup
                        from updateUser in updateUserGroup.DefaultIfEmpty()

                        select new BasketGetAllQueryResponse
                        {
                            Id = basket.Id,
                            UserId = basket.UserId,
                            MediaItemId = basket.MediaItemId,
                            MediaItemTitle = basket.MediaItemTitle,
                            MediaItemPrice = basket.MediaItemPrice,
                            Quantity = basket.Quantity,
                            MediaItemImageUrl = basket.MediaItemImageUrl,
                            IsInBasket = basket.IsInBasket, // ✅ Sepette mi değil mi?


                            CreatedAt = basket.CreatedAt,
                            UpdatedAt = basket.UpdatedAt,
                            DeletedAt = basket.DeletedAt,
                            IsDeleted = basket.IsDeleted,
                            CreateUserId = basket.CreateUserId,
                            UpdateUserId = basket.UpdateUserId,

                            CreateUserName = createUser == null
                                ? null
                                : $"{createUser.FirstName} {createUser.LastName} ({createUser.Email})",

                            UpdateUserName = updateUser == null
                                ? null
                                : $"{updateUser.FirstName} {updateUser.LastName} ({updateUser.Email})"
                        };

            return Task.FromResult(query.AsQueryable());
        }
    }
}
