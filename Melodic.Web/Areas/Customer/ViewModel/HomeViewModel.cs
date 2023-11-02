using Melodic.Domain.Entities;

namespace Melodic.Web.Areas.Customer.ViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<Speaker> Speakers { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
    }
}
