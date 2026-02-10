using Shipments.Domain.Models.Auth;

namespace Shipments.Domain.Models.Auth;

public class AppUser
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Email { get; set; } = default!;
    public string NormalizedEmail { get; set; } = default!;

    public string? FullName { get; set; }

    public string PasswordHash { get; set; } = default!;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }

    public List<RefreshToken> RefreshTokens { get; set; } = new();
    public List<PasswordResetToken> PasswordResetTokens { get; set; } = new();
}
