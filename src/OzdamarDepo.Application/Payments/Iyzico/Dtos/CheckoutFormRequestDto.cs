using Iyzipay.Model;
using OzdamarDepo.Domain.Abstractions;

namespace OzdamarDepo.Application.Payments.DTOs;

public class CheckoutFormRequestDto
{
    public string BasketId { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public string CallbackUrl { get; set; } = string.Empty;

    public Buyer Buyer { get; set; } = new();
    public AddressDto ShippingAddress { get; set; } = new();
    public AddressDto BillingAddress { get; set; } = new();
    public List<BasketItem> BasketItems { get; set; } = new();
}
