using Melodic.Application.ExtensionMethods;
using Melodic.Application.Pagination;
using Melodic.Domain.Entities;
using Melodic.Infrastructure.Identity;
using Melodic.Infrastructure.Persistence;
using Melodic.Web.Areas.Admin.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Melodic.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ApplicationRole.Role_Admin)]
    public class OrderDetailController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderDetailController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _context = db;
            _userManager = userManager;
        }
        // GET: Admin/OrderDetail/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            OrderDetailViewModel orderDetailVM = new OrderDetailViewModel()
            {
                OrderDetails = new List<OrderDetail>(),
                Order = new Order()
            };
            if (id == null)
            {
                return NotFound();

            }
            orderDetailVM.OrderDetails = await _context.OrderDetails.Include(x => x.Speaker).Include(x => x.Order).Where(x => x.OrderId == id).ToListAsync();
            orderDetailVM.Order = await _context.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (orderDetailVM.OrderDetails == null && orderDetailVM.Order == null)
            {
                return NotFound();
            }
            foreach (var item in await GetUser())
            {
                if (item.UserId == orderDetailVM.Order.UserId)
                {
                    ViewData["User"] = item.Email;
                    break;
                }
            }
            ViewBag.getTotal = _context.OrderDetails.Sum(c => c.Quantity * c.Speaker.Price);

            return View(orderDetailVM);
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
    }
}
