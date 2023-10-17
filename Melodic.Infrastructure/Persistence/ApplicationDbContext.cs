using Melodic.Application.Common.Interfaces;
using Melodic.Domain.Entities;
using Melodic.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Melodic.Infrastructure.Persistence;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
    {
    }

    public DbSet<Speaker> Speakers { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<EVoucher> EVouchers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Configurations.ApplicationUsersConfiguration());

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName.StartsWith("AspNet"))
                entityType.SetTableName(tableName.Substring(6));
        }

        modelBuilder.Entity<EVoucher>().HasData(
           new EVoucher()
            {
                Id = 1,
                Code = "EVOUNCHERKM5%",
                VouncherName = "KM5%",
                Description = "Discount for speaker"
            },
           new EVoucher()
           {
               Id = 2,
               Code = "EVOUNCHERKM10%",
               VouncherName = "KM10%",
               Description = "Discount for speaker"
           },
           new EVoucher()
           {
               Id = 3,
               Code = "EVOUNCHERKM15%",
               VouncherName = "KM15%",
               Description = "Discount for speaker"
           },
           new EVoucher()
           {
               Id = 4,
               Code = "EVOUNCHERKM20%",
               VouncherName = "KM20%",
               Description = "Discount for speaker"
           },
           new EVoucher()
           {
               Id = 5,
               Code = "EVOUNCHERKM25%",
               VouncherName = "KM25%",
               Description = "Discount for speaker"
           }
            );
        base.OnModelCreating(modelBuilder);
    }



    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
