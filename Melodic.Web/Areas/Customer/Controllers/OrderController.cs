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
            };
            _dbContext.Orders.Add(order);
            foreach (var speaker in Speakers) {
                var orderdetail = new OrderDetail
                {
                    OrderId=id,
                    SpeakerId=speaker.Id,
                };
            }
            _dbContext.SaveChanges();
            return View();
        }


        public IActionResult CheckOut(double totalPrice, double tax, double discount, double total)
        {
            ApplicationUser currentUser = _userManager.GetUserAsync(HttpContext.User).Result;

            var payments = currentUser.Payment.Select(u => u.CardNumber).ToList();
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
= _dbContext.Orders.FirstOrDefault(u => u.Id.Equals(randomString.ToString()));

                return randomString.ToString();
            }
            return GenerateRandomString();
        }


        public IActionResult Transistor(){
            return View();
        }


    }

}
