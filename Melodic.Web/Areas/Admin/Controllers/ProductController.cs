using AspNetCoreHero.ToastNotification.Abstractions;
using Melodic.Application.ExtensionMethods;
using Melodic.Domain.Entities;
using Melodic.Domain.ValueObjects;
using Melodic.Infrastructure.Identity;
using Melodic.Infrastructure.Persistence;
using Melodic.Web.Areas.Admin.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Melodic.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = ApplicationRole.Role_Admin)]
public class ProductController : Controller
{
    private readonly ApplicationDbContext _db;
    public INotyfService _notyfService { get; }
    private readonly IWebHostEnvironment _webHostEnvironment;
    public ProductController(ApplicationDbContext db, INotyfService notyfService, IWebHostEnvironment webHostEnvironment)
    {
        _db = db;
        _webHostEnvironment = webHostEnvironment;
        _notyfService = notyfService;
    }
    // GET: ProductController
    public async Task<ActionResult> Index(int? pageNumber)
    {
        var paginatedList = await _db.Speakers
                            .Include(x => x.Brand)
                            .PaginatedListAsync(pageNumber ?? 1, 4);
        return View(paginatedList);
    }

    // GET: ProductController/Create
    public async Task<ActionResult> CreateAndUpdateAsync(int? id)
    {

        SpeakerViewModel speakerVM = new()
        {
            BrandList = _db.Brands
            .AsNoTracking()
            .Select(x => new SelectListItem
             {
                 Text = x.Name,
                 Value = x.Id.ToString()
             }),
            Speaker = new Speaker()
        };
        if (id == null || id == 0)
        {
            return View(speakerVM);
        }
        else
        {
            speakerVM.Speaker = await _db.Speakers
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.Id == id);
            return View(speakerVM);
        }
    }

    // POST: ProductController/Create
    [HttpPost]
    public async Task<IActionResult> CreateAndUpdate(SpeakerViewModel speakerVM, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            String wwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                String fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                String productPath = Path.Combine(wwwRootPath, @"images\product");
                if (!string.IsNullOrEmpty(speakerVM.Speaker.Img))
                {
                    //Delete old image
                    var oldImage = Path.Combine(wwwRootPath, speakerVM.Speaker.Img.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImage))
                    {
                        System.IO.File.Delete(oldImage);
                    }
                }
                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                speakerVM.Speaker.Img = @"\images\product\" + fileName;
            }

            if (speakerVM.Speaker.Id == 0)
            {
                await _db.Speakers.AddAsync(speakerVM.Speaker);
                _notyfService.Success("Product Added Successfully");
            }
            else
            {
                _db.Speakers.Update(speakerVM.Speaker);
                _notyfService.Success("Product Updated Successfully");
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        else
        {
            speakerVM.BrandList = _db.Brands
            .AsNoTracking()
            .Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int? id)
    {
        Speaker? speaker = await _db.Speakers.FirstOrDefaultAsync(x => x.Id == id);
        if (id == null || id == 0)
        {
            return NotFound();
        }
        _db.Speakers.Remove(speaker);
        await _db.SaveChangesAsync();
        _notyfService.Success("Deleted!");
        return RedirectToAction("Index");
    }

}
