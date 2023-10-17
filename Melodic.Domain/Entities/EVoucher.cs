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

        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string VouncherName { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
