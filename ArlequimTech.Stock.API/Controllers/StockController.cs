using ArlequimTech.Stock.API.Base;
using ArlequimTech.Stock.Application.Handlers.Contracts;
using ArlequimTech.Stock.Domain.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArlequimTech.Stock.API.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : BaseController
{
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddStock([FromServices] IStockHandler handler,
        [FromBody] AddStockCommand command)
    {
        return await CreateResponse(await handler.Handle(command));
    }
}
