using AspNetCoreHero.ToastNotification.Abstractions;
using Melodic.Domain.Entities;
using Melodic.Infrastructure.Identity;
using Melodic.Infrastructure.Persistence;
using Melodic.Web.Areas.Admin.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Melodic.Web.Helper;

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
        public IActionResult Index(DateTime fromDate, DateTime toDate, string SearchString = "", string Status = "")
        {
            List<Order>? orders;
            if (Status == "" || Status == null)
            {
                if (SearchString != "" && SearchString != null)
                {
                    if (fromDate.ToString("dd/MM/yyyy") != "01/01/0001" && toDate.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        orders = _context.Orders.Where(o => o.StatusId != 0 && o.Created.Date >= fromDate && o.Created.Date <= toDate && (o.Code.Contains(SearchString) || o.PhoneNumber.Contains(SearchString) || o.Address.Contains(SearchString))).OrderByDescending(o => o.Created).ToList();
                        return View(Tuple.Create<IEnumerable<Order>, IEnumerable<OrderDetail>>(orders, _context.OrderDetails.Include(o => o.Order).Include(o => o.Speaker).ToList()));
                    }
                    else
                    {
                        orders = _context.Orders.Where(o => o.StatusId != 0 && (o.Code.Contains(SearchString) || o.PhoneNumber.Contains(SearchString) || o.Address.Contains(SearchString))).ToList();
                        return View(Tuple.Create<IEnumerable<Order>, IEnumerable<OrderDetail>>(orders, _context.OrderDetails.Include(o => o.Order).Include(o => o.Speaker).ToList()));

                    }
                }
            }
            if (Status == "Delivering" && Status != null)
            {
                if (SearchString != "" && SearchString != null)
                {
                    if (fromDate.ToString("dd/MM/yyyy") != "01/01/0001" && toDate.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        orders = _context.Orders.Where(o => o.StatusId == 1 && o.Created.Date >= fromDate && o.Created.Date <= toDate && (o.Code.Contains(SearchString) || o.PhoneNumber.Contains(SearchString) || o.Address.Contains(SearchString))).OrderByDescending(o => o.Created).ToList();
                        return View(Tuple.Create<IEnumerable<Order>, IEnumerable<OrderDetail>>(orders, _context.OrderDetails.Include(o => o.Order).Include(o => o.Speaker).ToList()));
                    }
                    else
                    {
                        orders = _context.Orders.Where(o => o.StatusId == 1 && (o.Code.Contains(SearchString) || o.PhoneNumber.Contains(SearchString) || o.Address.Contains(SearchString))).OrderByDescending(o => o.Created).ToList();
                        return View(Tuple.Create<IEnumerable<Order>, IEnumerable<OrderDetail>>(orders, _context.OrderDetails.Include(o => o.Order).Include(o => o.Speaker).ToList()));
                    }
                }
                else
                {
                    orders = _context.Orders.Where(o => o.StatusId == 1).ToList();
                    return View(Tuple.Create<IEnumerable<Order>, IEnumerable<OrderDetail>>(orders, _context.OrderDetails.Include(o => o.Order).Include(o => o.Speaker).ToList()));
                }
            }
            if (Status == "Cancel" && Status != null)
            {
                if (SearchString != "" && SearchString != null)
                {
                    if (fromDate.ToString("dd/MM/yyyy") != "01/01/0001" && toDate.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        orders = _context.Orders.Where(o => o.StatusId == 2 && o.Created.Date >= fromDate && o.Created.Date <= toDate && (o.Code.Contains(SearchString) || o.PhoneNumber.Contains(SearchString) || o.Address.Contains(SearchString))).OrderByDescending(o => o.Created).ToList();
                        return View(Tuple.Create<IEnumerable<Order>, IEnumerable<OrderDetail>>(orders, _context.OrderDetails.Include(o => o.Order).Include(o => o.Speaker).ToList()));
                    }
                    else
                    {
                        orders = _context.Orders.Where(o => o.StatusId == 2 && (o.Code.Contains(SearchString) || o.PhoneNumber.Contains(SearchString) || o.Address.Contains(SearchString))).OrderByDescending(o => o.Created).ToList();
                        return View(Tuple.Create<IEnumerable<Order>, IEnumerable<OrderDetail>>(orders, _context.OrderDetails.Include(o => o.Order).Include(o => o.Speaker).ToList()));
                    }
                }
                else
                {
                    orders = _context.Orders.Where(o => o.StatusId == 2).ToList();
                    return View(Tuple.Create<IEnumerable<Order>, IEnumerable<OrderDetail>>(orders, _context.OrderDetails.Include(o => o.Order).Include(o => o.Speaker).ToList()));
                }
            }
            if (Status == "Transaction failed" && Status != null)
            {
                if (SearchString != "" && SearchString != null)
                {
                    if (fromDate.ToString("dd/MM/yyyy") != "01/01/0001" && toDate.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        orders = _context.Orders.Where(o => o.StatusId == 3 && o.Created.Date >= fromDate && o.Created.Date <= toDate && (o.Code.Contains(SearchString) || o.PhoneNumber.Contains(SearchString) || o.Address.Contains(SearchString))).OrderByDescending(o => o.Created).ToList();
                        return View(Tuple.Create<IEnumerable<Order>, IEnumerable<OrderDetail>>(orders, _context.OrderDetails.Include(o => o.Order).Include(o => o.Speaker).ToList()));
                    }
                    else
                    {
                        orders = _context.Orders.Where(o => o.StatusId == 3 && (o.Code.Contains(SearchString) || o.PhoneNumber.Contains(SearchString) || o.Address.Contains(SearchString))).OrderByDescending(o => o.Created).ToList();
                        return View(Tuple.Create<IEnumerable<Order>, IEnumerable<OrderDetail>>(orders, _context.OrderDetails.Include(o => o.Order).Include(o => o.Speaker).ToList()));
                    }
                }
                else
                {
                    orders = _context.Orders.Where(o => o.StatusId == 3).ToList();
                    return View(Tuple.Create<IEnumerable<Order>, IEnumerable<OrderDetail>>(orders, _context.OrderDetails.Include(o => o.Order).Include(o => o.Speaker).ToList()));
                }
            }
            if (Status == "Delivered" && Status != null)
            {
                if (SearchString != "" && SearchString != null)
                {
                    if (fromDate.ToString("dd/MM/yyyy") != "01/01/0001" && toDate.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        orders = _context.Orders.Where(o => o.StatusId == 4 && o.Created.Date >= fromDate && o.Created.Date <= toDate && (o.Code.Contains(SearchString) || o.PhoneNumber.Contains(SearchString) || o.Address.Contains(SearchString))).OrderByDescending(o => o.Created).ToList();
                        return View(Tuple.Create<IEnumerable<Order>, IEnumerable<OrderDetail>>(orders, _context.OrderDetails.Include(o => o.Order).Include(o => o.Speaker).ToList()));
                    }
                    else
                    {
                        orders = _context.Orders.Where(o => o.StatusId == 4 && (o.Code.Contains(SearchString) || o.PhoneNumber.Contains(SearchString) || o.Address.Contains(SearchString))).OrderByDescending(o => o.Created).ToList();
                        return View(Tuple.Create<IEnumerable<Order>, IEnumerable<OrderDetail>>(orders, _context.OrderDetails.Include(o => o.Order).Include(o => o.Speaker).ToList()));
                    }
                }
                else
                {
                    orders = _context.Orders.Where(o => o.StatusId == 4).ToList();
                    return View(Tuple.Create<IEnumerable<Order>, IEnumerable<OrderDetail>>(orders, _context.OrderDetails.Include(o => o.Order).Include(o => o.Speaker).ToList()));
                }
            }

            if (fromDate.ToString("dd/MM/yyyy") != "01/01/0001" && toDate.ToString("dd/MM/yyyy") != "01/01/0001")
            {
                return View(Tuple.Create<IEnumerable<Order>, IEnumerable<OrderDetail>>(_context.Orders.Include(i => i.Speaker)
                            .Include(i => i.OrderDetails).Where(i => i.StatusId != 0 && i.Created.Date >= fromDate && i.Created.Date <= toDate).OrderByDescending(i => i.Created).ToList()
                            , _context.OrderDetails.Include(i => i.Speaker).ToList()));
            }
            else if (fromDate.ToString("dd/MM/yyyy") == "01/01/0001" && toDate.ToString("dd/MM/yyyy") == "01/01/0001")
            {
                return View(Tuple.Create<IEnumerable<Order>, IEnumerable<OrderDetail>>(_context.Orders.Include(i => i.Speaker).Include(i => i.OrderDetails).Where(i => i.StatusId != 0).OrderByDescending(i => i.Created).ToList(), _context.OrderDetails.Include(i => i.Speaker).ToList()));
            }

            return View(Tuple.Create<IEnumerable<Order>, IEnumerable<OrderDetail>>(_context.Orders.Include(i => i.Speaker).Include(i => i.OrderDetails).Where(i => i.StatusId != 0).OrderByDescending(i => i.Created).ToList(), _context.OrderDetails.Include(i => i.Order).Include(i => i.Speaker).ToList()));
        }

        // GET: Admin/Order/Create
        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        private async Task<List<UserViewModel>> GetUser()
        {
            var users = _userManager.Users.ToList();
            var userViewModel = new List<UserViewModel>();
            foreach (ApplicationUser user in users)
            {
                var thisViewModel = new UserViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.PhoneNumber = user.PhoneNumber;
                thisViewModel.Roles = await GetUserRoles(user);
                userViewModel.Add(thisViewModel);
            }
            return userViewModel;
        }
        public async Task<IActionResult> Create(int id = 0)
        {
           
            ViewData["SpeakerId"] = new SelectList(_context.Speakers, "Id", "Id");
            ViewData["UserEmail"] = new SelectList(await GetUser(), "Email", "Email");
            //ViewData["UserId"] = new SelectList(userViewModel, "UserId", "UserId");

            if (id == 0)
                return View(new Order());

            else
            {
                var orders = await _context.Orders.FindAsync(id);
                if (orders == null)
                {
                    return NotFound();
                }
                return View(orders);
            }
        }

        // POST: Admin/Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("Id,Code,Created,Address,PhoneNumber,Total,Status,Email,Fullname,StatusId,SpeakerId,Payment")] Order order)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (id == 0)
                {
                    _context.Add(order);
                    ViewData["SpeakerId"] = new SelectList(_context.Speakers, "Id", "Id", order.SpeakerId);
                    ViewData["UserEmail"] = new SelectList(await GetUser(), "Email", "Email", order.UserId);
                    ViewData["UserId"] = new SelectList(await GetUser(), "Id", "Id", order.UserId);

                    await _context.SaveChangesAsync();
                }

                ViewData["SpeakerId"] = new SelectList(_context.Speakers, "Id", "Id", order.SpeakerId);
                ViewData["UserId"] = new SelectList(await GetUser(), "Id", "Id", order.UserId);
                return Json(new { isValid = true, html = Helper.Helper.RenderRazorViewToString(this, "_ViewPartial", _context.Orders.ToList()) });
                //return RedirectToAction(nameof(Index));
            }
            return Json(new { isValid = false, html = Helper.Helper.RenderRazorViewToString(this, "Create", order) });
        }

        // GET: Admin/Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(i => i.OrderDetails)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        [Helper.Helper.NoDirectAccess]
        // GET: Admin/Invoices/Edit/5
        public async Task<IActionResult> Edit(int id = 0)
        {
            ViewData["SpeakerId"] = new SelectList(_context.Speakers, "Id", "Id");
            ViewData["UserId"] = new SelectList(await GetUser(), "Id", "Id");


            ViewBag.Status = new List<SelectListItem>
            {
            new SelectListItem { Text = "Đang giao hàng", Value = "1" },
            new SelectListItem { Text = "Hủy đơn", Value = "2" },
            new SelectListItem { Text = "Đã giao", Value = "4" }
            };

            if (id == 0)
                return View(new Order());

            else
            {
                var orders = await _context.Orders.FindAsync(id);
                if (orders == null)
                {
                    return NotFound();
                }

                return View(orders);
            }
        }
    }
}
