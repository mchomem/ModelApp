using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCHomem.Poc.CR.EF.Models
{
    public class UserRole: DataControl
    {
        [Key]
        public Int32 UserRoleID { get; set; }
        [MaxLength(100)]
        [Required]
        public String Name { get; set; }
        [MaxLength(1000)]
        public String Description { get; set; }
    }
}
