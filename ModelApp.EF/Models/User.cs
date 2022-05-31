using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ModelApp.EF.Models
{
    public class User : DataControl
    {
        [Key]
        public Int32 UserID { get; set; }
        [MaxLength(50)]
        [Required]
        public String Name { get; set; }
        [MaxLength(500)]
        [Required]
        public String Password { get; set; }
        [MaxLength(500)]
        public String SecretPhrase { get; set; }
        [MaxLength(100)]
        [Required]
        public String Email { get; set; }
        [Required]
        [DefaultValue("true")]
        public Boolean Active { get; set; }
        [Required]
        public Int32 UserRoleID { get; set; }
        public UserRole UserRole { get; set; }
        public Byte[] Avatar { get; set; }
    }
}
