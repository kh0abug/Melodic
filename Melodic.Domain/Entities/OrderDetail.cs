using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melodic.Domain.Entities;

public class OrderDetail
{
    public int SpeakerId { get; set; }
    public string OrderId{ get; set; }
    public int Quantity { get; set; }

    public Order Order { get; set; } = null!;
    public Speaker Speaker { get; set; } = null!;
}
