using Melodic.Domain.Entities;
using Melodic.Domain.ValueObjects;

namespace Melodic.Web.Areas.Customer.ViewModel
{
    public class PaymentViewModel
    {
        public IEnumerable<Payment> Payments { get; set; }

    }
}
