namespace Melodic.Infrastructure.Identity;

public interface ICurrentUserService
{
    string? UserId { get; }
}
