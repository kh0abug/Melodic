namespace Melodic.Domain.ValueObjects;
public record Payment(string?FullName ,string? CardNumber, string? CVV, string? ExpiryDate);