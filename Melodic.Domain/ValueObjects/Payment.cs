namespace Melodic.Domain.ValueObjects;
public record Payment(string? CardNumber, int? CVV, DateTime? ExpiryDate);