using System;
using System.ComponentModel.DataAnnotations;

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
