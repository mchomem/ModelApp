using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ModelApp.EF.Models
{
    public class Menu : DataControl
    {
        [Key]
        public int MenuID { get; set; }
        [MaxLength(20)]
        [Required]
        public string Label { get; set; }
        [MaxLength(50)]
        [Required]
        [DefaultValue("#")]
        public string Page { get; set; }

        public int? ParentMenuID { get; set; }
        public Menu ParentMenu { get; set; }

        public byte[] ImageIcon { get; set; }
        [MaxLength(30)]
        public string CssFontAwesomeIcon { get; set; }
        [Required]
        [DefaultValue("true")]
        public bool Active { get; set; }
        [Required]
        [DefaultValue("true")]
        public bool Visible { get; set; }
        public int Order { get; set; }
    }
}
