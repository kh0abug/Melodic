using Azure;
using Melodic.Application.Interfaces;
using Melodic.Domain.Entities;
using Melodic.Domain.ValueObjects;
using Melodic.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

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
    public DbSet<Cart> Carts { get; set; }



    public DbSet<Payment> Payment { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfiguration(new Configurations.ApplicationUsersConfiguration());
        modelBuilder.Entity<Cart>().HasKey(c => new { c.IdUser, c.IdSpeaker });
        modelBuilder.Entity<Speaker>().HasMany(e => e.OrderDetails).WithOne(e => e.Speaker).HasForeignKey(e => e.SpeakerId);
        modelBuilder.Entity<Order>().HasKey(e => e.Id);
        modelBuilder.Entity<OrderDetail>().HasKey(e => new { e.OrderId, e.SpeakerId });
        modelBuilder.Entity<EVoucher>().HasData(
           new EVoucher()
           {
               Id = 1,
               Code = "EVOUNCHERKM5%",
               VouncherName = "KM5%",
               Description = "Discount 5% for speaker",
               Percent = 0.5
           },
           new EVoucher()
           {
               Id = 2,
               Code = "EVOUNCHERKM10%",
               VouncherName = "KM10%",
               Description = "Discount 10% for speaker",
               Percent = 0.10
           },
           new EVoucher()
           {
               Id = 3,
               Code = "EVOUNCHERKM15%",
               VouncherName = "KM15%",
               Description = "Discount 15% for speaker",
               Percent = 0.15
           },
           new EVoucher()
           {
               Id = 4,
               Code = "EVOUNCHERKM20%",
               VouncherName = "KM20%",
               Description = "Discount 20% for speaker",
               Percent = 0.20
           },
           new EVoucher()
           {
               Id = 5,
               Code = "EVOUNCHERKM25%",
               VouncherName = "KM25%",
               Description = "Discount 25% for speaker",
               Percent = 0.25
           }
        );

        modelBuilder.Entity<Brand>().HasData(new Brand()
        {
            Id = 1,
            Name = "JBL"
        },
        new Brand()
        {
            Id = 2,
            Name = "Logitech"
        }, 
        new Brand()
        {
            Id = 3,
            Name = "Sony"
        }, 
        new Brand()
        {
            Id = 4,
            Name = "Nanomax"
        }
        );

        modelBuilder.Entity<Speaker>().HasData(new Speaker()
        {
            Id = 1,
            Name = "Sony SRS-XB13",
            BrandId = 3,
            Price = 950000,
            Decription = "Portable Bluetooth speaker with light show",
            UnitInStock = 10,
            Img = "https://cdn.tgdd.vn/Products/Images/2162/249767/sony-srs-xb13-150323-031134-600x600.jpg"
        });


        base.OnModelCreating(modelBuilder);
    }



    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
