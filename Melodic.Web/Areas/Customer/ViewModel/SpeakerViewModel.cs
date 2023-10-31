namespace Melodic.Web.Areas.Customer.ViewModel;

public class SpeakerViewModel
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public int BrandId { get; set; }

    public double Price { get; set; }

    public string? Img { get; set; }

    public int QuantitySold { get; set; }

    public string? Decription { get; set; }

    public DateTime Created { get; set; }
}
