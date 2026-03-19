using invoice_backend.DTOs.Auth;
using invoice_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace invoice_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AuthService service) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterUserDto request)
    {
        try
        {
            var result = await service.Register(request);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}