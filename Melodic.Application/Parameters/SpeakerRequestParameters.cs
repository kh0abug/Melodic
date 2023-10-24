using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melodic.Application.Parameters;
public class SpeakerRequestParameters : RequestParameters
{
    private int? _minPrice;
    public int? MinPrice
    {
        get
        {
            return _minPrice.GetValueOrDefault(0);
        }
        set
        {
            _minPrice = value;
        }
    }

    private int? _maxPrice;
    public int? MaxPrice
    {
        get
        {
            return _maxPrice.GetValueOrDefault(int.MaxValue);
        }
        set
        {
            _maxPrice = value;
        }
    }

    
    public bool ValidPriceRange => MaxPrice > MinPrice;

    public int? BrandId { get; set; }
    public int? EVoucherId { get; set; }
}
