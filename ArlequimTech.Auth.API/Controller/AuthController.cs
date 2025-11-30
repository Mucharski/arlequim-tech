using ArlequimTech.Auth.API.Base;
using ArlequimTech.Auth.Application.Handlers.Contracts;
using ArlequimTech.Auth.Domain.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArlequimTech.Auth.API.Controller;

[Route("api/auth")]
[ApiController]
public class AuthController : BaseController
{
    [AllowAnonymous]
    [HttpPost("createUser")]
    public async Task<IActionResult> CreateUser([FromServices] IAuthHandler handler,
        [FromBody] CreateUserCommand command)
    {
        try
        {
            return await CreateResponse(await handler.Handle(command));
        }
        catch
        {
            return StatusCode(500, "Ocorreu um erro ao processar a requisição. Contate o suporte.");
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromServices] IAuthHandler handler,
        [FromBody] LoginCommand command)
    {
        try
        {
            return await CreateResponse(await handler.Handle(command));
        }
        catch
        {
            return StatusCode(500, "Ocorreu um erro ao processar a requisição. Contate o suporte.");
        }
    }
}