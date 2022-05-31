using System;
using System.ComponentModel.DataAnnotations;

namespace ModelApp.EF.Models
{
    public class UserRole : DataControl
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
