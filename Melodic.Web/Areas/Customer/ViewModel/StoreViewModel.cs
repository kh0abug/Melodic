using Melodic.Application.Pagination;
using Melodic.Application.Parameters;
using Melodic.Domain.Entities;

namespace Melodic.Web.Areas.Customer.ViewModel;

public class StoreViewModel
{
    public IReadOnlyCollection<Brand>? Brands { get; set; }

    public PaginatedList<Speaker>? Speakers { get; set; }

    public SpeakerRequestParameters? RequestParameters { get; set; }
}
