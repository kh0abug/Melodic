using Melodic.Domain.Entities;
using Melodic.Infrastructure.Identity;
using Melodic.Infrastructure.Persistence;
using Melodic.Web.ViewsModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Melodic.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public CartController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public IActionResult Cart()
        {
            ApplicationUser currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var cartViewModel = new CartViewModel();

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

                    return RedirectToAction("Cart"); // Sau khi thêm vào giỏ hàng, chuyển hướng đến trang giỏ hàng.
                }
                else
                {
                    // Xử lý khi sản phẩm không tồn tại.
                    // Ví dụ: return NotFound();
                }
            }

            return RedirectToAction("Login"); // Chuyển hướng người dùng đến trang đăng nhập nếu họ chưa đăng nhập.
        }
    }
}
