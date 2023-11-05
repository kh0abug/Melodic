using Melodic.Domain.Entities;

namespace Melodic.Web.Areas.Admin.ViewModel
{
    public class OrderDetailViewModel
    {
        public List<OrderDetail> OrderDetails {  get; set; }
        public Order Order { get; set; }
    }
}
