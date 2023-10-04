

using Melodic.Domain.Entities;

namespace Melodic.Web.ViewsModel;

public class StoreViewModel
{
    public IEnumerable<Speaker>? Speakers { get; set; }
    public IEnumerable<Brand>? Brands { get; set; }

}
