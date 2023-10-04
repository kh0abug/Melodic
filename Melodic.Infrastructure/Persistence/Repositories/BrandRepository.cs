using Melodic.Application.Interfaces;
using Melodic.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Melodic.Infrastructure.Persistence.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly ApplicationDbContext _context;
    public BrandRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Brand>> GetAllBrand()
    {
        return await _context.Brands.ToListAsync();
    }

    public async Task<Brand> GetBrandById(int id)
    {
        return await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);
    }
}
