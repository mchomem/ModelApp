using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCHomem.Poc.CR.EF.Models
{
    public abstract class DataControl
    {
        [Required]
        [MaxLength(20)]
        public String CreatedBy { get; set; }
        [Required]
        public DateTime CreatedIn { get; set; }
        public String UpdatedBy { get; set; }
        public DateTime? UpdatedIn { get; set; }
    }
}
