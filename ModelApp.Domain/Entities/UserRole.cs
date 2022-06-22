using System.Text.Json.Serialization;

namespace ModelApp.Domain.Entities
{
    [Serializable]
    public class UserRole
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        #region Properties for Fluent API

        [JsonIgnore]
        public ICollection<User>? Users { get; set; }

        #endregion
    }
}
