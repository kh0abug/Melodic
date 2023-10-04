using Melodic.Application.Interfaces;
using Melodic.Domain.Entities;
using Melodic.Infrastructure.Persistence;
using Melodic.Web.ViewsModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Melodic.Web.Controllers;

public class StoreController : Controller
{
    private StoreViewModel storeVM = new StoreViewModel();

    private readonly ApplicationDbContext _context;

    //public StoreController(ApplicationDbContext context)
    //{
    //    _context = context;
    //}
    private readonly ISpeakerRepository _storeRepository;
    private readonly IBrandRepository _brandRepository;

    public StoreController(ISpeakerRepository storeRepository, IBrandRepository brandRepository, ApplicationDbContext context)
    {
        _storeRepository = storeRepository;
        _brandRepository = brandRepository;
        _context = context;


    }
    private IEnumerable<Speaker> speakerList = new List<Speaker>();
    private IEnumerable<Brand> brandList = new List<Brand>();





    //show list
    public async Task<IActionResult> Store()
    {
        storeVM.Speakers = await _storeRepository.GetAllSpeaker();
        storeVM.Brands = await _brandRepository.GetAllBrand();

        return View(storeVM);
    }

    public async Task<IActionResult> DetailSpeaker(int id)
    {
        if (id == null && id == 0)
        {
            return NotFound();
        }
        Speaker detailSpeaker = await _storeRepository.GetSpeakerById(id);
        return View(detailSpeaker);
    }

    //search by name
    public async Task<IActionResult> SearchByName(string name)
    {

        if (name == null)
        {
            //return RedirectToAction("Store");
            return RedirectToAction("StoreViewModel");
        }
        storeVM.Brands = await _brandRepository.GetAllBrand();
        storeVM.Speakers = await _storeRepository.GetSpeakerByName(name);


        if (storeVM.Speakers == null || !storeVM.Speakers.Any())
        {
            TempData["NotFoundMessage"] = "No Speaker Found.";
        }
        ViewBag.Name = name;
        return View("Store", storeVM);
    }

    //get brand left nhung dan gbi loi
    public async Task<IActionResult> GetSpeakerByBrand(int id)
    {
        if (id == null && id == 0)
        {
            //return RedirectToAction("Store");
            return RedirectToAction("Store");
        }

        storeVM.Speakers = await _storeRepository.GetSpeakerByBrand(id);
        storeVM.Brands = await _brandRepository.GetAllBrand();
        //return error mess
        if (storeVM.Speakers == null || !storeVM.Speakers.Any())
        {
            TempData["NotFoundMessage"] = "No Speaker Found.";
        }

        return View("Store", storeVM);
    }

    //sort z-a
    public async Task<IActionResult> SortSpeakerByNameDesc()
    {
        storeVM.Speakers = await _storeRepository.SortSpeakerByNameDesc();
        storeVM.Brands = await _brandRepository.GetAllBrand();
        if (storeVM.Speakers == null)
        {
            return RedirectToAction("Store");
        }
        return View("Store", storeVM);
    }

    //sort a-z
    public async Task<IActionResult> SortSpeakerByNameIsc()
    {
        storeVM.Speakers = await _storeRepository.SortSpeakerByNameIcs();
        storeVM.Brands = await _brandRepository.GetAllBrand();
        if (storeVM.Speakers == null)
        {
            return RedirectToAction("Store");
        }
        return View("Store", storeVM);
    }

    //sort pice from min to max
    public async Task<IActionResult> SortByPriceMinToMax(double minPrice, double maxPrice)
    {


        if (minPrice == null)
        {
            minPrice = 0;
        }
        if (maxPrice == null)
        {
            var price = _context.Speakers.Max(x => x.Price);
            maxPrice = price;
        }
        storeVM.Brands = await _brandRepository.GetAllBrand();
        storeVM.Speakers = await _storeRepository.SortByPriceMinToMax(minPrice, maxPrice);
        if (storeVM.Speakers == null && !storeVM.Speakers.Any())
        {
            TempData["NotFoundInRange"] = "No Speaker Found In Range Price.";
        }

        return View("Store", storeVM);
    }


    //sort price isc
    public async Task<IActionResult> SortSpeakerByPriceIsc()
    {
        storeVM.Brands = await _brandRepository.GetAllBrand();
        storeVM.Speakers = await _storeRepository.SortSpeakerByPriceIcs();
        if (storeVM.Speakers == null)
        {
            return RedirectToAction("Store");
        }

        //speakerList = listSpeakerIsc.ToList();
        return View("Store", storeVM);
    }

    //sort price desc
    public async Task<IActionResult> SortSpeakerByPriceDesc()
    {


        storeVM.Speakers = await _storeRepository.SortSpeakerByPriceDesc();
        storeVM.Brands = await _brandRepository.GetAllBrand();
        if (storeVM.Speakers == null)
        {
            return RedirectToAction("Store");
        }
        return View("Store", storeVM);
    }
}
