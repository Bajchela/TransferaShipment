using Microsoft.AspNetCore.Identity;
using Shipments.Contracts.Interfaces.Auth;

namespace Shipments.Infrastructure.Security;
public class PasswordHasherService : IPasswordHasherService
{
    private readonly PasswordHasher<object> _hasher = new();

    public string Hash(string password)
        => _hasher.HashPassword(new object(), password);

    public bool Verify(string hashedPassword, string providedPassword)
        => _hasher.VerifyHashedPassword(new object(), hashedPassword, providedPassword)
           != PasswordVerificationResult.Failed;
}

