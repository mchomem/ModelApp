using System.Text.Json.Serialization;

namespace ModelApp.Domain.Entities
{
    [Serializable]
    public class Menu
    {
        public int? Id { get; set; }
        public int? ParentMenuId { get; set; }
        public Menu? ParentMenu { get; set; }
        public string? Label { get; set; }
        public string? Page { get; set; }
        public byte[]? ImageIcon { get; set; }
        public string? CssFontAwesomeIcon { get; set; }
        public bool? Active { get; set; }
        public bool? Visible { get; set; }
        public int? Order { get; set; }

        #region Properties for Fluent API

        [JsonIgnore]
        public ICollection<Menu> Menus { get; set; }

        #endregion
    }
}
