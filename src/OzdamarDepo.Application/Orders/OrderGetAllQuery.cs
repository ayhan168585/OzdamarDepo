using MediatR;
using Microsoft.AspNetCore.Identity;
using OzdamarDepo.Domain.Abstractions;
using OzdamarDepo.Domain.Baskets;
using OzdamarDepo.Domain.Orders;
using OzdamarDepo.Domain.Users;

namespace OzdamarDepo.Application.Orders
{
    public sealed record OrderGetAllQuery() : IRequest<IQueryable<OrderGetAllQueryResponse>>;

    public sealed class OrderGetAllQueryResponse : EntityDto
    {
        public string OrderNumber { get; set; } = default!;
        public DateTimeOffset Date { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string City { get; set; } = default!;
        public string District { get; set; } = default!;
        public string FullAdress { get; set; } = default!;
        public string CartNumber { get; set; } = default!;
        public string CartOwnerName { get; set; } = default!;
        public string ExpiresDate { get; set; } = default!;
        public int Cvv { get; set; }
        public string InstallmentOptions { get; set; } = default!;
        public string Status { get; set; } = default!;

        public List<Basket> Baskets { get; set; } = new();

    }

    public sealed class OrdersGetAllQueryHandler(IOrderRepository orderRepository, UserManager<AppUser> userManager) : IRequestHandler<OrderGetAllQuery, IQueryable<OrderGetAllQueryResponse>>
    {
        public Task<IQueryable<OrderGetAllQueryResponse>> Handle(OrderGetAllQuery request, CancellationToken cancellationToken)
        {
            var response = (from order in orderRepository.GetAll()
                            join create_user in userManager.Users.AsQueryable() on order.CreateUserId equals create_user.Id
                            join update_user in userManager.Users.AsQueryable() on order.UpdateUserId equals update_user.Id into update_user
                            from update_users in update_user.DefaultIfEmpty()
                            select new OrderGetAllQueryResponse
                            {
                                OrderNumber = order.OrderNumber,
                                Date = order.Date,
                                UserId = order.UserId,
                                FullName = order.FullName,
                                PhoneNumber = order.PhoneNumber,
                                City = order.City,
                                District = order.District,
                                FullAdress = order.FullAdress,
                                CartNumber = order.CartNumber,
                                CartOwnerName = order.CartOwnerName,
                                ExpiresDate = order.ExpiresDate,
                                Cvv = order.Cvv,
                                InstallmentOptions = order.InstallmentOptions,
                                Status = order.Status,
                                CreatedAt = order.CreatedAt,
                                UpdatedAt = order.UpdatedAt,
                                DeletedAt = order.DeletedAt,
                                Id = order.Id,
                                IsDeleted = order.IsDeleted,
                                CreateUserId = order.CreateUserId,
                                UpdateUserId = order.UpdateUserId,


                                    

                                CreateUserName = create_user.FirstName + " " + create_user.LastName + "(" + create_user.Email + ")",
                                UpdateUserName = order.UpdateUserId == null ? null : create_user.FirstName + " " + create_user.LastName + "(" + create_user.Email + ")"



                            }).AsQueryable();
            return Task.FromResult(response);
        }
    }

}

