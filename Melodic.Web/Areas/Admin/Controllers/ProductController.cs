using AspNetCoreHero.ToastNotification.Abstractions;
using Melodic.Domain.Entities;
using Melodic.Domain.ValueObjects;
using Melodic.Infrastructure.Persistence;
using Melodic.Web.ViewsModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Melodic.Web.Areas.Admin.Controllers;

[Area("Admin")]
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
    public ActionResult Index(int? pageNumber)
    {
        int pageSize = 6;
        List<Speaker> speakList = _db.Speakers.Include(x => x.Brand).ToList();
        return View(PaginatedList<Speaker>.Paging(speakList, pageNumber ?? 1, pageSize));
    }


    // GET: ProductController/Create
    public ActionResult CreateAndUpdate(int? id)
    {

        SpeakerViewModel speakerVM = new()
        {
            BrandList = _db.Brands.Select(x => new SelectListItem
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
            speakerVM.Speaker = _db.Speakers.FirstOrDefault(x => x.Id == id);
            return View(speakerVM);
        }
    }

    // POST: ProductController/Create
    [HttpPost]
    public ActionResult CreateAndUpdate(SpeakerViewModel speakerVM, IFormFile? file)
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
                //productVM.Product.ImgUrl = fileName;
            }

            if (speakerVM.Speaker.Id == 0)
            {
                _db.Speakers.Add(speakerVM.Speaker);
                _notyfService.Success("Product Added Successfully");
            }
            else
            {
                _db.Speakers.Update(speakerVM.Speaker);
                _notyfService.Success("Product Updated Successfully");
            }

            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        else
        {
            speakerVM.BrandList = _db.Brands.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View();
        }
    }


    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        Speaker? speaker = _db.Speakers.FirstOrDefault(x => x.Id == id);
        if (speaker == null) { return NotFound(); }
        return View(speaker);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteBrand(int? id)
    {
        Speaker? speaker = _db.Speakers.FirstOrDefault(x => x.Id == id);
        if (id == null || id == 0)
        {
            return NotFound();
        }
        _db.Speakers.Remove(speaker);
        _db.SaveChanges();
        _notyfService.Success("Deleted!");
        return RedirectToAction("Index");
    }

}
