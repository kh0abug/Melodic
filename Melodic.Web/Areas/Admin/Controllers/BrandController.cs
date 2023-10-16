using AspNetCoreHero.ToastNotification.Abstractions;
using Melodic.Domain.Entities;
using Melodic.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Melodic.Web.Areas.Admin.Controllers;

[Area("Admin")]
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
    public ActionResult Index()
    {
        IEnumerable<Brand> speakList = _db.Brands;
        return View(speakList);
    }

    // GET: BrandController/Create
    public ActionResult CreateAndUpdate(int? id)
    {
        if (id == null || id == 0)
        {
            Brand brand = new Brand();
            return View(brand);
        }
        else
        {
            Brand brand = _db.Brands.FirstOrDefault(x => x.Id == id);
            return View(brand);
        }
    }

    // POST: BrandController/Create
    [HttpPost]
    public ActionResult CreateAndUpdate(Brand brand)
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
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View();
    }

    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        Brand? brand = _db.Brands.FirstOrDefault(x => x.Id == id);
        if (brand == null) { return NotFound(); }
        return View(brand);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteBrand(int? id)
    {
        Brand? brand = _db.Brands.FirstOrDefault(x => x.Id == id);
        if (id == null || id == 0)
        {
            return NotFound();
        }
        _db.Brands.Remove(brand);
        _db.SaveChanges();
        _notyfService.Success("Deleted!");
        return RedirectToAction("Index");
    }
}
