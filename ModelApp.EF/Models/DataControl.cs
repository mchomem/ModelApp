using System;
using System.ComponentModel.DataAnnotations;

namespace ModelApp.EF.Models
{
    public abstract class DataControl
    {
        [Required]
        [MaxLength(20)]
        public string CreatedBy { get; set; }
        [Required]
        public DateTime CreatedIn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedIn { get; set; }
    }
}
