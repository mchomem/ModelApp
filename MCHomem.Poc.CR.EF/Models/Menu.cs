using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCHomem.Poc.CR.EF.Models
{
    public class Menu : DataControl
    {
        [Key]
        public Int32 MenuID { get; set; }
        [MaxLength(20)]
        [Required]
        public String Label { get; set; }
        [MaxLength(50)]
        [Required]
        [DefaultValue("#")]
        public String Page { get; set; }

        public Int32? ParentMenuID { get; set; }
        public Menu ParentMenu { get; set; }

        public byte[] ImageIcon { get; set; }
        [MaxLength(30)]
        public String CssFontAwesomeIcon { get; set; }
        [Required]
        [DefaultValue("true")]
        public Boolean Active { get; set; }
        [Required]
        [DefaultValue("true")]
        public Boolean Visible { get; set; }
        public Int32 Order { get; set; }
    }
}
