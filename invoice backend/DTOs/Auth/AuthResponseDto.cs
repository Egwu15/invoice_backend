namespace invoice_backend.DTOs.Auth;

public class AuthResponseDto
{
    public int Id { get; init; }

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;
}