using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ModelApp.EF.Models
{
    public class Customer : DataControl
    {
        [Key]
        public int CustomerID { get; set; }
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
        public DateTime DateBirth { get; set; }
        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }
        [Required]
        [MaxLength(100)]
        public string Address { get; set; }
        [Required]
        [DefaultValue("true")]
        public bool Active { get; set; }
    }
}
