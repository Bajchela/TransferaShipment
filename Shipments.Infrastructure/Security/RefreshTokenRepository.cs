using Microsoft.EntityFrameworkCore;
using Shipments.Contracts.Interfaces.Auth;
using Shipments.Domain.Models.Auth;
using Shipments.Infrastructure.Persistance;

namespace Shipments.Infrastructure.Security;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ShipmentsDbContext _db;
    public RefreshTokenRepository(ShipmentsDbContext db) => _db = db;
    public Task<RefreshToken?> FindValid(string token)
        => _db.RefreshTokens.FirstOrDefaultAsync(x =>
            x.Token == token &&
            x.RevokedAtUtc == null &&
            x.ExpiresAtUtc > DateTime.UtcNow);

    public async Task<RefreshToken> Create(Guid userId, int days)
    {
        var rt = new RefreshToken
        {
            UserId = userId,
            Token = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N"),
            ExpiresAtUtc = DateTime.UtcNow.AddDays(days)
        };

        _db.RefreshTokens.Add(rt);
        await _db.SaveChangesAsync();
        return rt;
    }
    public async Task Update(RefreshToken rt)
    {
        _db.RefreshTokens.Update(rt);
        await _db.SaveChangesAsync();
    }
}

