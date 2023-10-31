using AspNetCoreHero.ToastNotification.Abstractions;
using Azure.Core;
using Melodic.Application.ExtensionMethods;
using Melodic.Domain.Entities;
using Melodic.Infrastructure.Identity;
using Melodic.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Melodic.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = ApplicationRole.Role_Admin)]
public class BrandController : Controller
{
    private readonly ApplicationDbContext _db;
    public INotyfService _notyfService { get; }
    public BrandController(ApplicationDbContext db, INotyfService notyfService)
    {
        _db = db;
        _notyfService = notyfService;
    }
    // GET: ProductController
    public async Task<IActionResult> Index(int? pageNumber)
    {
        var paginatedList = await _db.Brands.PaginatedListAsync(pageNumber ?? 1, 4);
        return View(paginatedList);
    }
    
    // GET: BrandController/Create
    public async Task<IActionResult> CreateAndUpdate(int? id)
    {
        if (id == null || id == 0)
        {
            Brand brand = new Brand();
            return View(brand);
        }
        else
        {
            Brand brand = await _db.Brands.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return View(brand);
        }
    }

    // POST: BrandController/Create
    [HttpPost]
    public async Task<IActionResult> CreateAndUpdate(Brand brand)
    {
        if (ModelState.IsValid)
        {
            //Create new
            if (brand.Id == 0)
            {
                _db.Brands.Add(brand);
                _notyfService.Success("Brand Added Successfully");
            }
            else
            {
                _db.Brands.Update(brand);
                _notyfService.Success("Brand Updated Successfully");
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int? id)
    {
        Brand? brand = await _db.Brands.FirstOrDefaultAsync(x => x.Id == id);
        if (id == null || id == 0)
        {
            return NotFound();
        }
        _db.Brands.Remove(brand);
        await _db.SaveChangesAsync();
        _notyfService.Success("Deleted!");
        return RedirectToAction("Index");
    }
}
