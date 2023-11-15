using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melodic.Domain.Entities
{
    public class EVoucher
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string VouncherName { get; set; }
        [Required]
        [Range(0.1, 1, ErrorMessage = "Please enter value in 0.0 - 1")]
        public double Percent { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
