using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OzdamarDepo.Domain.Abstractions;
using OzdamarDepo.Domain.Baskets;
using OzdamarDepo.Domain.Orders;
using OzdamarDepo.Domain.Users;
using System.ComponentModel.DataAnnotations;

namespace OzdamarDepo.Application.Orders
{
    public sealed record OrderGetAllQuery(Guid UserId) : IRequest<List<OrderGetAllQueryResponse>>;

    public class OrderGetAllQueryResponse : EntityDto
    {
        public string OrderNumber { get; set; } = default!;
        public DateTimeOffset Date { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string City { get; set; } = default!;
        public string District { get; set; } = default!;
        public string FullAdress { get; set; } = default!;

        public CargoStatusEnum CargoStatus { get; set; }
        public string CargoStatusText => GetCargoStatusDisplayText(CargoStatus);

        public decimal Total { get; set; }
        public List<BasketWithMediaItemDto> Baskets { get; set; } = new();


        private static string GetCargoStatusDisplayText(CargoStatusEnum status)
        {
            var memberInfo = typeof(CargoStatusEnum).GetMember(status.ToString()).FirstOrDefault();
            if (memberInfo != null)
            {
                var displayAttr = memberInfo.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
                if (displayAttr != null)
                {
                    return displayAttr.Name!;
                }
            }

            return status.ToString();
        }
    }





    public sealed class OrdersGetAllQueryHandler(IOrderRepository orderRepository, UserManager<AppUser> userManager)
    : IRequestHandler<OrderGetAllQuery, List<OrderGetAllQueryResponse>>
    {
        public async Task<List<OrderGetAllQueryResponse>> Handle(OrderGetAllQuery request, CancellationToken cancellationToken)
        {
            var orderDtos = await orderRepository.GetOrdersWithBasketsAndMediaAsync();

            var filteredOrders = orderDtos.Where(x => x.Order.UserId == request.UserId).ToList();

            var userIds = filteredOrders
                .Select(x => x.Order.CreateUserId)
                .Union(filteredOrders.Where(x => x.Order.UpdateUserId.HasValue).Select(x => x.Order.UpdateUserId!.Value))
                .Distinct()
                .ToList();

            var users = await userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync(cancellationToken);

            var response = filteredOrders.Select(x =>
            {
                var order = x.Order;
                var createUser = users.FirstOrDefault(u => u.Id == order.CreateUserId);
                var updateUser = order.UpdateUserId.HasValue
                    ? users.FirstOrDefault(u => u.Id == order.UpdateUserId.Value)
                    : null;

                var basketDtos = x.Baskets.Select(b => new BasketWithMediaItemDto
                {
                    Id = b.Id,
                    Quantity = b.Quantity,
                    MediaItemTitle = b.MediaItem?.Title ?? string.Empty,
                    MediaItemPrice = b.MediaItem?.Price ?? 0,
                    MediaItemImageUrl = b.MediaItem?.ImageUrl ?? string.Empty,
                    MediaItem = b.MediaItem != null
                        ? new MediaItemDto
                        {
                            Title = b.MediaItem.Title,
                            Price = b.MediaItem.Price,
                            ImageUrl = b.MediaItem.ImageUrl
                        }
                        : null,
                    Total = (b.MediaItem?.Price ?? 0) * b.Quantity
                }).ToList();


                return new OrderGetAllQueryResponse
                {
                    OrderNumber = order.OrderNumber,
                    Date = order.Date,
                    UserId = order.UserId,
                    FullName = order.FullName,
                    PhoneNumber = order.PhoneNumber,
                    City = order.City,
                    District = order.District,
                    FullAdress = order.FullAdress,
                    CargoStatus = order.CargoStatus,
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt,
                    DeletedAt = order.DeletedAt,
                    Id = order.Id,
                    IsDeleted = order.IsDeleted,
                    CreateUserId = order.CreateUserId,
                    UpdateUserId = order.UpdateUserId,
                    CreateUserName = createUser != null
                        ? $"{createUser.FirstName} {createUser.LastName} ({createUser.Email})"
                        : null,
                    UpdateUserName = updateUser != null
                        ? $"{updateUser.FirstName} {updateUser.LastName} ({updateUser.Email})"
                        : null,
                    Baskets = basketDtos,
                    Total = basketDtos.Sum(b => b.Total)
                };
            }).ToList();


            return response;

        }
    }




}

