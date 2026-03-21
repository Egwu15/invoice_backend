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
        var result = await service.Register(request);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
    {
        var result = await service.Login(dto);
        return Ok(result);
    }
}