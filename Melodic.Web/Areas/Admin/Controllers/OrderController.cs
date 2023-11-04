using AspNetCoreHero.ToastNotification.Abstractions;
using Melodic.Domain.Entities;
using Melodic.Infrastructure.Identity;
using Melodic.Infrastructure.Persistence;
using Melodic.Web.Areas.Admin.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Melodic.Application.Pagination;
using Melodic.Application.ExtensionMethods;
using Microsoft.AspNetCore.Hosting;

namespace Melodic.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ApplicationRole.Role_Admin)]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public INotyfService _notyfService { get; }
        public OrderController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, INotyfService notyfService)
        {
            _userManager = userManager;
            _context = db;
            _notyfService = notyfService;
        }


        private Task<List<UserViewModel>> GetUser()
        {
            var users = _userManager.Users.ToList();
            var userViewModel = new List<UserViewModel>();
            foreach (ApplicationUser user in users)
            {
                var thisViewModel = new UserViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.PhoneNumber = user.PhoneNumber;
                userViewModel.Add(thisViewModel);
            }
            return Task.FromResult(userViewModel);
        }

        //GET : Admin/Order/Index
        public async Task<IActionResult> Index(int? pageNumber, DateTime fromDate, DateTime toDate, string SearchString = "")
        {
            //List<Order> orders;
            PaginatedList<Order> paginatedList;

            if (SearchString != "" && SearchString != null)
            {
                if (fromDate.ToString("dd/MM/yyyy") != "01/01/0001" && toDate.ToString("dd/MM/yyyy") != "01/01/0001")
                {
                    paginatedList = await _context.Orders.Where(o => o.Created.Date >= fromDate && o.Created.Date <= toDate && (o.Id.Contains(SearchString) || o.PhoneNumber.Contains(SearchString) || o.Address.Contains(SearchString) || o.FullName.Contains(SearchString))).OrderByDescending(o => o.Created).PaginatedListAsync(pageNumber ?? 1, 4);
                    return View(paginatedList);
                }
                else
                {
                    paginatedList = await _context.Orders.Where(o => o.Id.Contains(SearchString) || o.PhoneNumber.Contains(SearchString) || o.Address.Contains(SearchString) || o.FullName.Contains(SearchString)).PaginatedListAsync(pageNumber ?? 1, 4);
                    return View(paginatedList);

                }
            }

            if (fromDate.ToString("dd/MM/yyyy") != "01/01/0001" && toDate.ToString("dd/MM/yyyy") != "01/01/0001")
            {
                paginatedList = await _context.Orders.Where(i => i.Created.Date >= fromDate && i.Created.Date <= toDate).OrderByDescending(i => i.Created).PaginatedListAsync(pageNumber ?? 1, 4);
                return View(paginatedList);

            }
            else if (fromDate.ToString("dd/MM/yyyy") == "01/01/0001" && toDate.ToString("dd/MM/yyyy") == "01/01/0001")
            {
                paginatedList = await _context.Orders.OrderByDescending(i => i.Created).PaginatedListAsync(pageNumber ?? 1, 4);
                return View(paginatedList);

            }
            paginatedList = await _context.Orders.OrderByDescending(i => i.Created).PaginatedListAsync(pageNumber ?? 1, 4);
            return View(paginatedList);

        }

        // GET: Admin/Order/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            OrderViewModel orderVM = new()
            {
                Order = new Order()
            };

            if (id == null)
                return NotFound();

            else
            {
                orderVM.Order = await _context.Orders
                               .AsNoTracking()
                               .FirstOrDefaultAsync(x => x.Id == id);
                if (orderVM.Order == null)
                {
                    return NotFound();
                }

                foreach (var item in await GetUser())
                {
                    if (item.UserId == orderVM.Order.UserId)
                    {
                        ViewData["User"] = item.Email;
                        break;
                    }
                }
                return View(orderVM);
            }
        }
        // POST: Admin/Order/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(OrderViewModel OrderVM)
        {
            if (ModelState.IsValid)
            {
                if (OrderVM.Order.Id == null)
                {
                    return NotFound();
                }
                else
                {
                    _context.Orders.Update(OrderVM.Order);
                    _notyfService.Success("Order Update Successfully");
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }
        // POST: Admin/Order/Edit/5

        [HttpPost]
        public async Task<IActionResult> Delete(string? id)
        {
            Order? order = await _context.Orders.Include(x => x.OrderDetails).FirstOrDefaultAsync(x => x.Id == id);
            if (id == null)
            {
                return NotFound();
            }
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            _notyfService.Success("Deleted!");
            return RedirectToAction("Index");
        }
    }
}
