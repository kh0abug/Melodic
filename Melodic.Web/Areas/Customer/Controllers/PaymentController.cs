using Melodic.Domain.ValueObjects;
using Melodic.Infrastructure.Identity;
using Melodic.Infrastructure.Persistence;
using Melodic.Web.Areas.Customer.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Melodic.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class PaymentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public PaymentController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public IActionResult Payment()
        {
            var payement = new PaymentViewModel();
            ApplicationUser currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            payement.Payments = currentUser.Payment;
            if (payement.Payments.Any())
            {
                return RedirectToAction("Bill", "Bill");
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddPayment(string? fullname, string? cardnumber, string? exdate, string? cvv)
        {
            ApplicationUser currentUser = _userManager.GetUserAsync(HttpContext.User).Result;

            // Kiểm tra xem tất cả các trường thông tin thanh toán đã được cung cấp.
            if (!string.IsNullOrEmpty(fullname) && !string.IsNullOrEmpty(cardnumber) && !string.IsNullOrEmpty(exdate) && !string.IsNullOrEmpty(cvv))
            {
                // Tạo một đối tượng Payment từ dữ liệu đầu vào.
                var payment = new Payment(fullname, cardnumber, exdate, cvv);




                // Thêm thông tin thanh toán vào danh sách thông tin thanh toán của người dùng.
                 currentUser.Payment.Add(payment);

                // Lưu thay đổi vào cơ sở dữ liệu.
                var result = _userManager.UpdateAsync(currentUser).Result;

                if (result.Succeeded)
                {
                    
                    _dbContext.SaveChanges();
                    return RedirectToAction("CheckOut","Bill");
                }
                else
                {
                    // Xảy ra lỗi khi thêm thông tin thanh toán, xử lý lỗi ở đây (ví dụ: trả về trang lỗi).
                    return View("Error"); // Tạo trang lỗi hoặc sử dụng trang lỗi có sẵn.
                }
            }
            else
            {
                // Hiển thị thông báo lỗi nếu thiếu thông tin thanh toán.
                ViewBag.ErrorMessage = "Please provide all payment information.";
                return View();
            }
        }

    }
}
