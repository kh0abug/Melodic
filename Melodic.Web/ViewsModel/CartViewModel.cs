using Melodic.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Melodic.Web.ViewsModel
{
    public class CartViewModel
    {
        public IEnumerable<Cart> CartItems { get; set; }
        public IEnumerable<Speaker>? Speakers { get; set; }

    }
}
