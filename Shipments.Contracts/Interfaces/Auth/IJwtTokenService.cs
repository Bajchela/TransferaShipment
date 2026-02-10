using Shipments.Domain.Models.Auth;

namespace Shipments.Contracts.Interfaces.Auth
{
    public interface IJwtTokenService
    {
        /// <summary>
        /// Create Access token
        /// </summary>
        /// <param name="user"></param>
        /// <param name="expiresMinutes"></param>
        /// <returns></returns>
        string CreateAccessToken(AppUser user, int expiresMinutes);
    }

}
