using Melodic.Domain.Entities;
using Melodic.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Melodic.Infrastructure.Identity;
public class ApplicationUser : IdentityUser
{
    public List<Payment>? Payment { get; set; } 
}
