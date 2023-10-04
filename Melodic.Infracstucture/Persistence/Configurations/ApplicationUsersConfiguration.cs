using Melodic.Infracstructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Melodic.Infracstructure.Persistence.Configurations;
public class ApplicationUsersConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> entity)
    {
        entity.OwnsMany(u => u.Payments);
    }
}
