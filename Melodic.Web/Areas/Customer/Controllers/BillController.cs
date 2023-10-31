using Microsoft.AspNetCore.Mvc;

namespace Melodic.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class BillController : Controller
    {
        public IActionResult Bill()
        {
            return View();
        }
        public IActionResult CheckOut()
        {
            return View();
        }
    }
}
