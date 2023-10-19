using Melodic.Application.Interfaces;
using Melodic.Infrastructure.Persistence;
using Melodic.Web.Models;
using Melodic.Web.ViewsModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Melodic.Web.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private StoreViewModel storeVM = new StoreViewModel();


    private readonly ISpeakerRepository _storeRepository;
    private readonly IBrandRepository _brandRepository;
    public HomeController(ILogger<HomeController> logger, ISpeakerRepository storeRepository, IBrandRepository brandRepository, ApplicationDbContext context)
    {
        _logger = logger;
         _storeRepository = storeRepository;
        _brandRepository = brandRepository;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
