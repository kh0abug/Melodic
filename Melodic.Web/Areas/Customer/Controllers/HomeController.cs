using Melodic.Domain.Entities;
using Melodic.Infrastructure.Persistence;
using Melodic.Web.Areas.Customer.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Melodic.Web.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    HomeViewModel homeViewModel = new HomeViewModel();

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
       
    homeViewModel.Speakers = _context.Speakers.Include(s => s.Brand).Take(4).OrderByDescending(s => s.Id).ToList();

        ViewBag.Speakers = _context.Speakers.Include(s => s.Brand).OrderBy(x => Guid.NewGuid()).Take(4).ToList();

        return View(homeViewModel);
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
