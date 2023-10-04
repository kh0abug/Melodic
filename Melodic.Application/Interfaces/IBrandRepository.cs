using Melodic.Domain.Entities;

namespace Melodic.Application.Interfaces;

public interface IBrandRepository
{
    Task<IEnumerable<Brand>> GetAllBrand();
    Task<Brand> GetBrandById(int id);
}
