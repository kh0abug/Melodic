using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Melodic.Domain.Entities;

public class Speaker
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }

    [ForeignKey("Brand")]
    public int BrandId { get; set; }
    public Brand? Brand { get; set; }
    [Required]
    public double Price { get; set; }
    public string? Decription { get; set; }
    public string? Img { get; set; }
}
