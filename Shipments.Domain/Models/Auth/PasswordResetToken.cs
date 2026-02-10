namespace Shipments.Domain.Models.Auth;

public class PasswordResetToken
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }
    public AppUser User { get; set; } = default!;

    public string TokenHash { get; set; } = default!;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAtUtc { get; set; }

    public DateTime? UsedAtUtc { get; set; }
    public DateTime? RevokedAtUtc { get; set; }

    public bool IsUsed => UsedAtUtc != null;
    public bool IsValid => !IsUsed && RevokedAtUtc == null && DateTime.UtcNow < ExpiresAtUtc;
}