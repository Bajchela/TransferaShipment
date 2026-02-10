namespace Shipments.Domain.DTOs.Auth;

public record UserDto(
    Guid Id,
    string Email,
    string? FullName
);
