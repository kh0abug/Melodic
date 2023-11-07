using Melodic.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Melodic.Web.Areas.Customer.ViewModel
{
    public class CartViewModel
    {
        public IEnumerable<Cart> CartItems { get; set; }
        public IEnumerable<Speaker>? Speakers { get; set; }

        public double? Total { get; set; }
        public double? TotalPrice { get; set; }
        public double? Tax { get; set; }
        public double? Discount { get; set; }
    }
}
