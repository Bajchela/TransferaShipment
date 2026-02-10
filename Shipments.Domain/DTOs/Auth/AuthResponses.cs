namespace Shipments.Domain.DTOs.Auth;

public record AuthResponse(
    string AccessToken,
    int ExpiresInSeconds,
    UserDto User
);

public record AuthResult(
    string AccessToken,
    string RefreshToken,
    int ExpiresInSeconds,
    UserDto User
);
