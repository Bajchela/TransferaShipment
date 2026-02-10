
using Shipments.Domain.Models.Auth;

namespace Shipments.Contracts.Interfaces.Auth
{

    public interface IRefreshTokenRepository
    {        
        /// <summary>
        /// Create new token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        Task<RefreshToken> Create(Guid userId, int days);
                
        /// <summary>
        /// Find valid token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<RefreshToken?> FindValid(string token);
                
        /// <summary>
        /// Update token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task Update(RefreshToken token);
    }
}
