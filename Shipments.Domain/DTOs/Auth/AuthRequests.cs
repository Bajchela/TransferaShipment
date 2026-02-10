namespace Shipments.Domain.DTOs.Auth;

public class AuthRequests
{
    public record RegisterRequest(string Email, string Password, string? FullName);
    public record LoginRequest(string Email, string Password);
    public record RefreshRequest(string RefreshToken);
}

