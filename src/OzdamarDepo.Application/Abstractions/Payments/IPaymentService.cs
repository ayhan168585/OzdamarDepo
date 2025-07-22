using OzdamarDepo.Application.Payments.DTOs;

namespace OzdamarDepo.Application.Abstractions.Payments;

public interface IPaymentService
{
    Task<string?> InitializePaymentAsync(CheckoutFormRequestDto dto);
}