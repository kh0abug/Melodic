using Melodic.Domain.Entities;
using Melodic.Domain.ValueObjects;
using Melodic.Infrastructure.Identity;
using Melodic.Infrastructure.Persistence;
using Melodic.Web.Areas.Customer.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Melodic.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private CartViewModel cartViewModel;
        public CartController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            cartViewModel = new CartViewModel();
        }

        public IActionResult Cart()
        {
            ApplicationUser currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            

            if (currentUser != null)
            {
                cartViewModel.CartItems = _dbContext.Carts
                    .Where(cart => cart.IdUser == currentUser.Id)
                    .ToList();
            }
            else
            {
                cartViewModel.CartItems = new List<Cart>();
            }

            // Lấy danh sách IdSpeaker từ danh sách CartItems
            var speakerIds = cartViewModel.CartItems.Select(cartItem => cartItem.IdSpeaker).ToList();

            // Lấy thông tin sản phẩm dựa vào danh sách IdSpeaker
            cartViewModel.Speakers = _dbContext.Speakers
                .Where(speaker => speakerIds.Contains(speaker.Id))
                .ToList();

            double? TotalPrice = 0;

            foreach (var speaker in cartViewModel.CartItems)
            {
                var speakers = cartViewModel.Speakers.FirstOrDefault(s => s.Id == speaker.IdSpeaker);
                var Price = speaker.Quantity * speakers.Price;
                TotalPrice += Price; // Cộng giá trị Price vào TotalPrice

            }
            double? Tax = TotalPrice * 0.08;
            cartViewModel.Tax = Tax;
            // Truyền giá trị TotalPrice vào ViewBag
            cartViewModel.TotalPrice = TotalPrice;
            if (TempData.ContainsKey("Discount"))
            {
                double? discount = Convert.ToDouble(TempData["Discount"]);
                double? total = TotalPrice + Tax - discount;
                cartViewModel.Total = total;
                cartViewModel.Discount = discount;
            }

            else
            {
                double? total = TotalPrice + Tax;
                cartViewModel.Total = total;

            }
           

            if (currentUser.Payment.Count != 0)
            {
                ViewBag.payment = 1;
            }
                return View(cartViewModel);
            
        }

        public IActionResult RemoveFromCart(int id)
        {
            ApplicationUser currentUser = _userManager.GetUserAsync(HttpContext.User).Result;

            if (currentUser != null)
            {
                var cartItemToRemove = _dbContext.Carts.FirstOrDefault(cart => cart.IdUser == currentUser.Id && cart.IdSpeaker == id);

                if (cartItemToRemove != null)
                {
                    _dbContext.Carts.Remove(cartItemToRemove);
                    _dbContext.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu.
                }
            }

            return RedirectToAction("Cart"); // Chuyển hướng đến trang giỏ hàng sau khi xóa sản phẩm.
        }

        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            ApplicationUser currentUser = _userManager.GetUserAsync(HttpContext.User).Result;

            if (currentUser != null)
            {
                // Lấy thông tin sản phẩm dựa vào id (từ cơ sở dữ liệu hoặc nguồn dữ liệu khác)
                var product = _dbContext.Speakers.FirstOrDefault(p => p.Id == id);

                if (product != null)
                {
                    // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng của người dùng chưa.
                    var existingCartItem = _dbContext.Carts.FirstOrDefault(cart => cart.IdUser == currentUser.Id && cart.IdSpeaker == id);

                    if (existingCartItem != null)
                    {
                        // Nếu sản phẩm đã tồn tại, bạn có thể tăng số lượng (quantity).
                        existingCartItem.Quantity += 1;
                    }
                    else
                    {
                        // Nếu sản phẩm chưa tồn tại trong giỏ hàng, bạn có thể tạo một mục mới.
                        var newCartItem = new Cart
                        {
                            IdUser = currentUser.Id,
                            IdSpeaker = id,
                            Quantity = 1 // Số lượng ban đầu là 1
                        };
                        _dbContext.Carts.Add(newCartItem);
                    }

                    _dbContext.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu.

                    // Trả về Json đối tượng có success là true và message là "Added successfully"
                    return Json(new { success = true, message = "Added successfully" });
                }
                else
                {
                    // Nếu sản phẩm không tồn tại, trả về Json đối tượng có success là false và message là "Product not found"
                    return Json(new { success = false, message = "Product not found" });
                }
            }
            else
            {
                // Nếu người dùng không tồn tại, trả về Json đối tượng có success là false và message là "User not found"
                return Json(new { success = false, message = "User not found" });
            }
        }

        public IActionResult Voucher(string voucher, double? totalPrice)
        {
            var Voucher = _dbContext.EVouchers.FirstOrDefault(vou => vou.Code.Equals(voucher));
            if (Voucher != null)
            {
                double? discount = totalPrice * Voucher.Percent;
                TempData["Discount"] = discount.ToString();

                return RedirectToAction("Cart");
            }
            //string a = "Voucher is not available";
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int itemId, int newQuantity)
        {
            ApplicationUser currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var productToUpdate = _dbContext.Carts.FirstOrDefault(cart => cart.IdUser == currentUser.Id && cart.IdSpeaker == itemId);

            if (productToUpdate == null)
            {
                return Json(new { success = false, message = "Product not found" });
            }

            productToUpdate.Quantity = newQuantity;
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Quantity updated successfully" });
        }

        public IActionResult ConFirmChange() { return RedirectToAction("Cart"); }


    }

}
