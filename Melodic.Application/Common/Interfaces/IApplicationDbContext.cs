namespace Melodic.Application.Common.Interfaces;
public interface IApplicationDbContext
{

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
