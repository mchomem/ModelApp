using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ModelApp.EF.Models
{
    public class User : DataControl
    {
        [Key]
        public int UserID { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [MaxLength(500)]
        [Required]
        public string Password { get; set; }
        [MaxLength(500)]
        public string SecretPhrase { get; set; }
        [MaxLength(100)]
        [Required]
        public string Email { get; set; }
        [Required]
        [DefaultValue("true")]
        public bool Active { get; set; }
        [Required]
        public int UserRoleID { get; set; }
        public UserRole UserRole { get; set; }
        public byte[] Avatar { get; set; }
    }
}
