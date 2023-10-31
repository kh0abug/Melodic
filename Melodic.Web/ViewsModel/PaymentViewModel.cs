using Melodic.Domain.Entities;
using Melodic.Domain.ValueObjects;

namespace Melodic.Web.ViewsModel
{
    public class PaymentViewModel
    {
        public IEnumerable<Payment> Payments { get; set; }

    }
}
