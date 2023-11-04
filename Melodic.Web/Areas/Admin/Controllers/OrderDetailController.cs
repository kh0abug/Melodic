using Melodic.Infrastructure.Identity;
using Melodic.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Melodic.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ApplicationRole.Role_Admin)]
    public class OrderDetailController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OrderDetailController(ApplicationDbContext db)
        {
            _context = db;

        }
        // GET: Admin/OrderDetail
        //public async Task<IActionResult> Index(int? id)
        //{
        //    ViewBag.getTotal = _context.Carts.Sum(c => c.Quantity * c.Sp.Price);
        //    var dA_TOTNGHIEP2Context = _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product);
        //    return View(await dA_TOTNGHIEP2Context.ToListAsync());
        //}

        // GET: Admin/OrderDetail/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(i => i.Order)
                .Include(i => i.Speaker)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }
    }
}
