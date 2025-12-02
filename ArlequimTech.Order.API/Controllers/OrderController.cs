using ArlequimTech.Order.API.Base;
using ArlequimTech.Order.Application.Handlers.Contracts;
using ArlequimTech.Order.Domain.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArlequimTech.Order.API.Controllers;

[Route("api/order")]
[ApiController]
public class OrderController : BaseController
{
    [Authorize(Roles = "Admin, User")]
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromServices] IOrderHandler handler,
        [FromBody] CreateOrderCommand command)
    {
        return await CreateResponse(await handler.Handle(command));
    }
}
