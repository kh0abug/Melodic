using Melodic.Application.ExtensionMethods;
using Melodic.Application.Parameters;
using Melodic.Domain.Entities;
using Melodic.Infrastructure.Persistence;
using Melodic.Web.Areas.Customer.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using System.Reflection.Metadata;

namespace Melodic.Web.Areas.Customer.Controllers;

[Area("Customer")]
public class StoreController : Controller
{
    private readonly ApplicationDbContext _context;

    public StoreController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(SpeakerRequestParameters parameter)
    {
        ViewBag.SearchTerm = parameter.SearchTerm;


        var speakers = _context.Speakers.Include( s => s.Brand).AsQueryable();


        var brands = _context.Brands.AsNoTracking().Take(10);



        if (!parameter.ValidPriceRange)
        {
            ModelState.AddModelError("maxprice", "Please fill in the appropriate price range.");
        }

        if (string.IsNullOrEmpty(parameter.SearchTerm))
        {
            return View(new StoreViewModel
            {
                Speakers = await speakers.Include(s => s.Brand).AsNoTracking().OrderByDescending(s => s.Id).PaginatedListAsync(parameter.PageNumber ?? 1, parameter.PageSize),

                RequestParameters = parameter,
                Brands = await brands.ToListAsync()
            }); ;
        }

        if (!ModelState.IsValid)
        {
            return View(new StoreViewModel
            {
                RequestParameters = parameter,
                Brands = await brands.ToListAsync()
            });
        }
        else
        {
            speakers = speakers.Include(s => s.Brand).Where(s => s.Name!.Contains(parameter.SearchTerm!));
            speakers = speakers.Where(s => s.Price >= parameter.MinPrice && s.Price <= parameter.MaxPrice);
        }

        if (parameter.BrandId is not null)
        {
            speakers = speakers.Include(s => s.Brand).Where(s => s.BrandId == parameter.BrandId);
        }

        speakers = parameter.OrderBy?.ToLower() switch
        {
            "price_desc" => speakers.Include(s => s.Brand).OrderByDescending(s => s.Price),
            "latest" => speakers.OrderByDescending(s => s.Id),
            _ => speakers.OrderBy(s => s.Price),
        };

        var result = await speakers.Include(s => s.Brand).AsNoTracking().PaginatedListAsync(parameter.PageNumber ?? 1, parameter.PageSize);
        if (!result.Items.Any())
        {
            result = null;
        }
        return View(new StoreViewModel
        {
            Speakers = result,
            RequestParameters = parameter,
            Brands = await brands.ToListAsync()
        });
    }

    public async Task<IActionResult> Store()
    {
        var speakers = _context.Speakers.Include(s => s.Brand).OrderByDescending(s => s.Id).AsQueryable();
        var brands = _context.Brands.AsNoTracking().Take(10);
        
        ViewBag.ListSpeaker = speakers.ToList();
        ViewBag.ListBrands = brands.ToList();

        return View();
    }


    public async Task<IActionResult> SortSpeakerByPriceIsc()
    {
        var speakers = _context.Speakers.Include(s => s.Brand).AsQueryable();
        var brands = _context.Brands.AsNoTracking().Take(10);

        ViewBag.ListSpeaker = speakers.OrderBy(s => s.Price).ToList();
        ViewBag.ListBrands = brands.ToList();

        return View("Store");
    }

   
    public async Task<IActionResult> SortSpeakerByPriceDesc()
    {


        var speakers = _context.Speakers.Include(s => s.Brand).AsQueryable();
        var brands = _context.Brands.AsNoTracking().Take(10);

        ViewBag.ListSpeaker = speakers.OrderByDescending(s => s.Price).ToList();
        ViewBag.ListBrands = brands.ToList();

        return View("Store");
    }
    public async Task<IActionResult> Detail(int? id)
    {
        if (id == null && id == 0)
        {
            return NotFound();
        }
        var speaker = await _context.Speakers.Include(s => s.Brand)
            .AsNoTracking()
            .Include(s => s.Brand)
            .Where(s => s.Id == id).FirstAsync();

        //var brands = await _context.Speakers.Include(s => s.Brand).Where(s => s.BrandId == speaker.BrandId).Take(4).ToListAsync();
        //ViewBag.Brands = brands;

        ViewBag.Brands = _context.Speakers.Include(s => s.Brand).Where(s => s.BrandId == speaker.BrandId).OrderBy(x => Guid.NewGuid()).Take(4).ToList();
        return View(speaker);
    }

    public async Task<IActionResult> GetSpeakerByBrandID(int id)
    {
        var speakers = _context.Speakers.Include(s => s.Brand).Where(s => s.Brand.Id == id).ToList();
        var brands = _context.Brands.AsNoTracking().Take(10);
        if (speakers == null)
        {
            TempData["NotFoundMessage"] = "No Speaker Found.";
        }
        ViewBag.ListSpeaker = speakers.ToList();
        ViewBag.ListBrands = brands.ToList();
        return View("Store");
    }
}

