using Microsoft.AspNetCore.Mvc;
using Shipments.Domain.DTOs.Auth;
using Shipments.Infrastructure.Security;
using static Shipments.Domain.DTOs.Auth.AuthRequests;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _auth;
    private readonly IConfiguration _cfg;
    public AuthController(AuthService auth, IConfiguration cfg)
    {
        _auth = auth;
        _cfg = cfg;
    }
    public record LoginReq(string Username, string Password);

    [HttpPost("login")]
    public async Task<AuthResponse> Login([FromBody] LoginRequest req)
    {
        var result = await _auth.Login(req);

        // ⬇️ refresh token ide u HttpOnly cookie
        CookieHelper.SetRefreshToken(
            Response,
            result.RefreshToken,
            _cfg.GetValue<int>("Jwt:RefreshTokenDays")
        );

        // ⬇️ front dobija SAMO access token
        return new AuthResponse(
            result.AccessToken,
            result.ExpiresInSeconds,
            result.User
        );
    }

    [HttpPost("register")]
    public async Task<AuthResponse> Register([FromBody] RegisterRequest req)
    {
        var result = await _auth.Register(req);

        CookieHelper.SetRefreshToken(
            Response,
            result.RefreshToken,
            _cfg.GetValue<int>("Jwt:RefreshTokenDays")
        );

        return new AuthResponse(
            result.AccessToken,
            result.ExpiresInSeconds,
            result.User
        );
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        CookieHelper.ClearRefreshToken(Response);
        return Ok();
    }

}
