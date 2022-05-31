using System.ComponentModel.DataAnnotations;

namespace ModelApp.EF.Models
{
    public class UserRole : DataControl
    {
        [Key]
        public int UserRoleID { get; set; }
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
    }
}
