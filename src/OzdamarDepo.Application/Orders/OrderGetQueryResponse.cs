using OzdamarDepo.Domain.Abstractions;
using OzdamarDepo.Domain.Orders;

namespace OzdamarDepo.Application.Orders;

public sealed class OrderGetQueryResponse
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = default!;
    public DateTimeOffset Date { get; set; }

    public Guid UserId { get; set; }
    public string FullName { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string City { get; set; } = default!;
    public string District { get; set; } = default!;
    public string FullAdress { get; set; } = default!;

    public CargoStatusEnum CargoStatus { get; set; }
    public string CargoStatusText => CargoStatus.ToString();

    public List<BasketWithMediaItemDto> Baskets { get; set; } = new();
    public decimal Total => Baskets.Sum(b => b.Total);
}
