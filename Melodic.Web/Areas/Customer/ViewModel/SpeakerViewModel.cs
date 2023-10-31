using Melodic.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Melodic.Web.Areas.Customer.ViewModel;

public class SpeakerViewModel
{

    public int Id { get; set; }
    public string? Name { get; set; }

    public int BrandId { get; set; }

    public Brand? Brand { get; set; }

    public double Price { get; set; }

    public string? Decription { get; set; }

    public int Quantity { get; set; }

    public string? Img { get; set; }

    public int QuantitySold { get; set; }

}
