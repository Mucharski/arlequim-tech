using ArlequimTech.Product.API.Base;
using ArlequimTech.Product.Application.Handlers.Contracts;
using ArlequimTech.Product.Domain.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArlequimTech.Product.API.Controllers;

[Route("api/product")]
[ApiController]
public class ProductController : BaseController
{
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromServices] IProductHandler handler,
        [FromBody] CreateProductCommand command)
    {
        return await CreateResponse(await handler.Handle(command));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> ListProducts([FromServices] IProductHandler handler)
    {
        return await CreateResponse(await handler.Handle(new ListProductsCommand()));
    }

    [Authorize(Roles = "Admin, User")]
    [HttpGet("consult")]
    public async Task<IActionResult> ConsultProduct([FromServices] IProductHandler handler,
        [FromQuery] string name)
    {
        return await CreateResponse(await handler.Handle(new ConsultProductByNameCommand(name)));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<IActionResult> UpdateProduct([FromServices] IProductHandler handler,
        [FromBody] UpdateProductCommand command)
    {
        return await CreateResponse(await handler.Handle(command));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete]
    public async Task<IActionResult> DeleteProduct([FromServices] IProductHandler handler,
        [FromQuery] Guid id)
    {
        return await CreateResponse(await handler.Handle(new DeleteProductCommand(id)));
    }
}
