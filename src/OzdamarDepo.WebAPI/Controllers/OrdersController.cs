using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OzdamarDepo.Application.Orders;

namespace OzdamarDepo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // kullanıcının login olması gerekiyor
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var userIdClaim = User.Claims.FirstOrDefault(x =>
                x.Type == ClaimTypes.NameIdentifier || x.Type == "user-id");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized("Kullanıcı bilgisi alınamadı.");

            var result = await _mediator.Send(new OrderGetAllQuery(userId));
            return Ok(result.ToList());
        }
    }
}
