using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCHomem.Poc.CR.EF.Models
{
    public class Customer : DataControl
    {
        [Key]
        public Int32 CustomerID { get; set; }
        [MaxLength(100)]
        [Required]
        public String Name { get; set; }
        public DateTime DateBirth { get; set; }
        [Required]
        [MaxLength(20)]
        public String PhoneNumber { get; set; }
        [Required]
        [MaxLength(100)]
        public String Address { get; set; }
        [Required]
        [DefaultValue("true")]
        public Boolean Active { get; set; }
    }
}
