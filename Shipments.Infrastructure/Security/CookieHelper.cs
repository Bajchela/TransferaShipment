using Microsoft.AspNetCore.Http;

namespace Shipments.Infrastructure.Security;

public static class CookieHelper
{
    /// <summary>
    /// Set refresh token
    /// </summary>
    /// <param name="response"></param>
    /// <param name="refreshToken"></param>
    /// <param name="days"></param>
    public static void SetRefreshToken(
        HttpResponse response,
        string refreshToken,
        int days)
    {
        response.Cookies.Append(
            "refresh_token",
            refreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(days),
                Path = "/api/auth/refresh"
            });
    }

    /// <summary>
    /// Clear refresh token
    /// </summary>
    /// <param name="response"></param>
    public static void ClearRefreshToken(HttpResponse response)
    {
        response.Cookies.Delete("refresh_token");
    }
}
