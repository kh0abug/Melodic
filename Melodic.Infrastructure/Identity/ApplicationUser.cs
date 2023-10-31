using Melodic.Domain.Entities;
using Melodic.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Melodic.Infrastructure.Identity;
public class ApplicationUser : IdentityUser
{
    public IReadOnlyCollection<Payment>? Payments { get; set; } 
}
