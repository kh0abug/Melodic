using Melodic.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Melodic.Infracstructure.Identity;
public class ApplicationUser : IdentityUser
{
    public IReadOnlyCollection<Payment>? Payments { get; set; }
}
