using Microsoft.AspNetCore.Mvc;
using OzdamarDepo.Application.Abstractions.Payments;
using OzdamarDepo.Application.Payments.DTOs;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<IActionResult> Initialize(CheckoutFormRequestDto dto)
    {
        var url = await _paymentService.InitializePaymentAsync(dto);
        if (string.IsNullOrEmpty(url))
        {
            return BadRequest("Ödeme başlatılamadı.");
        }

        return Ok(new { url });
    }
}

