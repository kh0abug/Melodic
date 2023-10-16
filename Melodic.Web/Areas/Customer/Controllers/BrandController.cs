using Melodic.Application.Interfaces;
using Melodic.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Melodic.Web.Areas.Customer.Controllers;

[Area("Customer")]
public class BrandController : Controller
{
    //private readonly ApplicationDbContext _context;
    //public BrandController(ApplicationDbContext context)
    //{
    //    _context = context;
    //}
    private readonly IBrandRepository _brandRepository;
    public BrandController(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }
    //get all brand
    public async Task<IActionResult> GetAllBrand()
    {
        IEnumerable<Brand> brands = await _brandRepository.GetAllBrand();
        return View(brands);
    }

    //get detail brand 
    public async Task<IActionResult> DetailBrand(int id)
    {
        var brandwid = await _brandRepository.GetBrandById(id);
        return View(brandwid);
    }
}
