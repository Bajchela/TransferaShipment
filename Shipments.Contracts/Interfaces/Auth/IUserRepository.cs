
using Shipments.Domain.Models.Auth;

namespace Ads.Application.Interfaces.Auth
{
    public interface IUserRepository
    {        
        /// <summary>
        /// Find by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<AppUser?> FindByEmail(string email);

        /// <summary>
        /// Find user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AppUser?> FindById(Guid id);

        /// <summary>
        /// Add new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task Add(AppUser user);
    }
}
