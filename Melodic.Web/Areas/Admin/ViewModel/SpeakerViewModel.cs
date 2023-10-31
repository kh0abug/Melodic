using Melodic.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Melodic.Web.Areas.Admin.ViewModel
{
    public class SpeakerViewModel
    {
        public Speaker Speaker { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> BrandList { get; set; }
    }
}
