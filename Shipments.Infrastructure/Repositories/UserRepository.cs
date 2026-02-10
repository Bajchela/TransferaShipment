using Ads.Application.Interfaces.Auth;
using Microsoft.EntityFrameworkCore;
using Shipments.Domain.Models.Auth;
using Shipments.Infrastructure.Persistance;

namespace Ads.Infrastructure.Repositories.Auth;

/// <summary>
/// Repo za korisnike (EF Core).
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly ShipmentsDbContext _db;

    public UserRepository(ShipmentsDbContext db) => _db = db;

    public Task<AppUser?> FindByEmail(string normalizedEmail)
        => _db.Users.FirstOrDefaultAsync(x => x.Email == normalizedEmail);

    public Task<AppUser?> FindById(Guid id)
        => _db.Users.FirstOrDefaultAsync(x => x.Id == id);

    public async Task Add(AppUser user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
    }
}
