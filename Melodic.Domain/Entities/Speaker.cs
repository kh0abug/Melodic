using Melodic.Domain.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Melodic.Domain.Entities;

public class Speaker : AuditableEntity
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }

    [ForeignKey("Brand")]
    public int BrandId { get; set; }

    [ValidateNever]
    public Brand? Brand { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
    public double Price { get; set; }

    [Required]
    public string? Decription { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
    public int? UnitInStock { get; set; }

    [ValidateNever]
    public string? Img { get; set; }

    public List<OrderDetail> OrderDetails { get; set; } = new();
}
