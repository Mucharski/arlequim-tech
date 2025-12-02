using ArlequimTech.Core.BaseClasses.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ArlequimTech.Product.API.Base
{
    public class BaseController : ControllerBase
    {
        protected async Task<IActionResult> CreateResponse<T>(ICommandResult<T> commandResult)
        {
            try
            {
                if (commandResult.Errors == null)
                {
                    if (commandResult.Success)
                    {
                        return Ok(new
                        {
                            commandResult.Success,
                            commandResult.Message,
                            Data = commandResult.Data
                        });
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            commandResult.Success,
                            commandResult.Message,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Erro ao processar requisição",
                    Errors = new[] { ex.Message }
                });
            }


            if (commandResult.Errors.Any())
            {
                switch (commandResult)
                {
                    default:
                        return BadRequest(new
                        {
                            Success = false,
                            Message = commandResult.Message,
                            Errors = commandResult.Errors,
                        });
                }
            }

            return StatusCode(500, new
            {
                Success = false,
                Message = "Ocorreu um erro ao processar sua requisição. Tente novamente mais tarde.",
            });
        }
    }
}