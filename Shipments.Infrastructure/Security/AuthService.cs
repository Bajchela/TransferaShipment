using Microsoft.Extensions.Configuration;
using Shipments.Contracts.Interfaces.Auth;
using System.Security.Authentication;
using Ads.Application.Interfaces.Auth;
using Shipments.Domain.DTOs.Auth;
using static Shipments.Domain.DTOs.Auth.AuthRequests;
using Shipments.Domain.Models.Auth;

namespace Shipments.Infrastructure.Security;

public class AuthService
{
    private readonly IUserRepository _users;
    private readonly IRefreshTokenRepository _refresh;
    private readonly IPasswordHasherService _hasher;
    private readonly IJwtTokenService _jwt;
    private readonly IConfiguration _cfg;

    public AuthService(
        IUserRepository users,
        IRefreshTokenRepository refresh,
        IPasswordHasherService hasher,
        IJwtTokenService jwt,
        IConfiguration cfg)
    {
        _users = users;
        _refresh = refresh;
        _hasher = hasher;
        _jwt = jwt;
        _cfg = cfg;
    }

    /// <summary>
    /// Register user
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<AuthResult> Register(RegisterRequest req)
    {
        var email = NormalizeEmail(req.Email);

        var exists = await _users.FindByEmail(email);
        if (exists != null)
            throw new Exception("Email već postoji.");

        var user = new AppUser
        {
            Email = email,
            FullName = req.FullName,
            PasswordHash = _hasher.Hash(req.Password),
            NormalizedEmail = email.ToUpperInvariant()
        };

        await _users.Add(user);

        return await IssueTokens(user);
    }
    
    /// <summary>
    /// Login user
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    /// <exception cref="InvalidCredentialException"></exception>
    public async Task<AuthResult> Login(LoginRequest req)
    {
        var email = NormalizeEmail(req.Email);

        var user = await _users.FindByEmail(email)
            ?? throw new InvalidCredentialException("Pogrešan email ili lozinka.");

        if (!_hasher.Verify(user.PasswordHash, req.Password))
            throw new InvalidCredentialException("Pogrešan email ili lozinka.");

        return await IssueTokens(user);
    }

    /// <summary>
    /// Refresh token
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<AuthResult> Refresh(RefreshRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.RefreshToken))
            throw new Exception("Refresh token je prazan.");

        var rt = await _refresh.FindValid(req.RefreshToken)
            ?? throw new Exception("Refresh token nije validan.");

        var user = await _users.FindById(rt.UserId)
            ?? throw new Exception("User ne postoji.");

        var refreshDays = GetInt("Jwt:RefreshTokenDays", fallback: 30);
        var accessMin = GetInt("Jwt:AccessTokenMinutes", fallback: 15);
               
        rt.RevokedAtUtc = DateTime.UtcNow;

        var newRt = await _refresh.Create(user.Id, refreshDays);
        rt.ReplacedByToken = newRt.Token;

        await _refresh.Update(rt);
                
        var access = _jwt.CreateAccessToken(user, accessMin);

        return new AuthResult(
            AccessToken: access,
            RefreshToken: newRt.Token,
            ExpiresInSeconds: accessMin * 60,
            User: new UserDto(user.Id, user.Email, user.FullName)
        );
    }
       
    /// <summary>
    /// Issue tokens
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    private async Task<AuthResult> IssueTokens(AppUser user)
    {
        var refreshDays = GetInt("Jwt:RefreshTokenDays", fallback: 30);
        var accessMin = GetInt("Jwt:AccessTokenMinutes", fallback: 15);

        var access = _jwt.CreateAccessToken(user, accessMin);
        var rt = await _refresh.Create(user.Id, refreshDays);

        return new AuthResult(
            AccessToken: access,
            RefreshToken: rt.Token,
            ExpiresInSeconds: accessMin * 60,
            User: new UserDto(user.Id, user.Email, user.FullName)
        );
    }    

    /// <summary>
    /// Normalize email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    private static string NormalizeEmail(string email)
        => (email ?? "").Trim().ToLowerInvariant();       

    /// <summary>
    /// Get int
    /// </summary>
    /// <param name="key"></param>
    /// <param name="fallback"></param>
    /// <returns></returns>
    private int GetInt(string key, int fallback)
    {
        try
        {            
            var v = _cfg.GetValue<int?>(key);
            return v ?? fallback;
        }
        catch
        {
            return fallback;
        }
    }
}
