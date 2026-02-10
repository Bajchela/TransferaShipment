namespace Shipments.Contracts.Interfaces.Auth
{
    public interface IPasswordHasherService
    {
        /// <summary>
        /// Create Hash password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        string Hash(string password);

        /// <summary>
        /// Verify password
        /// </summary>
        /// <param name="hashedPassword"></param>
        /// <param name="providedPassword"></param>
        /// <returns></returns>
        bool Verify(string hashedPassword, string providedPassword);
    }
}
