using Microsoft.AspNetCore.Mvc;

namespace Melodic.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class PaymentController : Controller
    {

        public IActionResult Payment()
        {
            return View();
        }
    }
}
