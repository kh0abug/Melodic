using Melodic.Domain.Common;

namespace Melodic.Domain.Entities
{
    public class Bill : AuditableEntity
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
