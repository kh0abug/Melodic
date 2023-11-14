using Melodic.Domain.Entities;
using Melodic.Domain.ValueObjects;
using Melodic.Infrastructure.Identity;
using Melodic.Infrastructure.Persistence;
using Melodic.Web.Areas.Customer.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Text;

namespace Melodic.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;


        public OrderController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;

        }
        public IActionResult SearchOrder(string search)
        {
            ApplicationUser currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            if (search != null)
            {
                List<Order> Order = _dbContext.Orders.Where(u => u.UserId == currentUser.Id && (u.Id.Contains(search) || u.Total.ToString().Contains(search))).ToList();
                if (Order.Count == 0) {ViewBag.Order = null; }
                else { ViewBag.Order = Order; }
                 
                return View("History");
            }
            return RedirectToAction("History");

        }
        public IActionResult Bill(string fullname, string phonenumber, string payment, string address, double totalPrice, double tax, double discount, double total)
        {

            ApplicationUser currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            string id = GenerateRandomString();

            List<Cart> cartItems = _dbContext.Carts
                    .Where(cart => cart.IdUser == currentUser.Id)
                    .ToList();
            var speakerIds = cartItems.Select(cartItem => cartItem.IdSpeaker).ToList();
           

            List<Speaker> Speakers = _dbContext.Speakers
               .Where(speaker => speakerIds.Contains(speaker.Id))
               .ToList();
           
            ViewBag.phonenumber=phonenumber; 
            ViewBag.payment=payment; 
            ViewBag.address=address;
            ViewBag.totalPrice= totalPrice;
            ViewBag.tax = tax;
            ViewBag.total=total;
            ViewBag.discount=discount;
            ViewBag.speakers= Speakers;
            ViewBag.fullname = fullname;
            ViewBag.cartitem = cartItems;
            ViewBag.id = id;
            ViewBag.Date=DateTime.Now.ToString();
            var order = new Order
            {
                Id = id,
                UserId = currentUser.Id,
                Address = address,
                Discount = discount,
                Payment = payment,
                PhoneNumber = phonenumber,
                FullName = fullname,
                Total = total,
                Tax = tax,
                TotalPrice = totalPrice,
                Created=DateTime.Now
            };
            _dbContext.Orders.Add(order);
            foreach (var speaker in Speakers)
            {
                var cartitemexist = _dbContext.Carts.FirstOrDefault(cart => cart.IdUser == currentUser.Id && cart.IdSpeaker == speaker.Id);
                var orderdetail = new OrderDetail
                {
                    OrderId = id,
                    SpeakerId = speaker.Id,
                    Quantity=cartitemexist.Quantity
                };
                speaker.UnitInStock -= cartitemexist.Quantity;
                _dbContext.OrderDetails.Add(orderdetail);
            }
            foreach (var cartItem in cartItems)
            {
                _dbContext.Carts.Remove(cartItem);
            }
            _dbContext.SaveChanges();
            return View();
        }


        public IActionResult CheckOut(double totalPrice, double tax, double discount, double total)
        {
            ApplicationUser currentUser = _userManager.GetUserAsync(HttpContext.User).Result;

            List<Payment> payments = currentUser.Payment.ToList();
            List<Cart> cartItems = _dbContext.Carts
                    .Where(cart => cart.IdUser == currentUser.Id)
                    .ToList();
            var speakerIds = cartItems.Select(cartItem => cartItem.IdSpeaker).ToList();
            List<Speaker> Speakers = _dbContext.Speakers
               .Where(speaker => speakerIds.Contains(speaker.Id))
               .ToList();
            ViewBag.payment = payments;
            ViewBag.Speakers = Speakers;
            ViewBag.cartitem = cartItems;
            ViewBag.TotalPrice = totalPrice;
            ViewBag.Tax = tax;
            ViewBag.Discount = discount;
            ViewBag.Total = total;
           
            return View("CheckOut");
        }

        public IActionResult CreateCheckOut()
        {

            return RedirectToAction("Index");
        }
     public string GenerateRandomString()
        {
            int length = 16;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder randomString = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(0, chars.Length);
                randomString.Append(chars[index]);
            }
            var existingId = _dbContext.Orders.Select(u => u.Id);
            if (existingId != null)
            {
                var existingIds
                 = _dbContext.Orders.Where(u => u.Id == randomString.ToString());

                return randomString.ToString();
            }
            return GenerateRandomString();
        }


        public IActionResult History(){
            ApplicationUser currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            List<Order> Order = _dbContext.Orders.Where(u => u.UserId == currentUser.Id).ToList()  ;
            ViewBag.Order = Order ;
            return View("History");
        }

        public IActionResult OrderDetail(string id)

        {
            ApplicationUser currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            List<OrderDetail> cartItems = _dbContext.OrderDetails.Where(u => u.OrderId.Equals(id)).ToList();

            var Ids = cartItems.Select(cartItem => cartItem.SpeakerId).ToList();
            List<Speaker> Speakers = _dbContext.Speakers
               .Where(speaker => Ids.Contains(speaker.Id))
               .ToList();
            var order = _dbContext.Orders.FirstOrDefault(o => o.Id == id) as Order;
            ViewBag.id = order.Id;
            ViewBag.total = order.Total;
            ViewBag.fullname = order.FullName;
            ViewBag.address = order.Address;
            ViewBag.phonenumber = order.PhoneNumber;
            ViewBag.totalprice = order.TotalPrice;
            ViewBag.discount = order.Discount;
            ViewBag.tax = order.Tax;
            ViewBag.speakers = Speakers;
            ViewBag.cartitem = cartItems;
            ViewBag.Date = order.Created;
            return View("OrderDetail");
        }
        
    }

}
