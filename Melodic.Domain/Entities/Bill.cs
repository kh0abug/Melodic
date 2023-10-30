using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melodic.Domain.Entities
{
     public class Bill
    {
        public string? BuildId { get; set; }
        public string? IdUser { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public double? Tax { get; set; }
        public double? Discount { get; set; }
        public double? Total { get; set; }
        public double? TotalPrice { get; set; }
        public string? Payment {  get; set; }
        
    }
}
