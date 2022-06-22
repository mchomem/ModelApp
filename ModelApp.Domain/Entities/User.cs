using System.Text.Json.Serialization;

namespace ModelApp.Domain.Entities
{
    [Serializable]
    public class User
    {
        public int? Id { get; set; }
        public int? UserRoleId { get; set; }
        [JsonIgnore]
        public UserRole? UserRole { get; set; }
        public string? Login { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? SecretPhrase { get; set; }
        public string? Email { get; set; }
        public bool? Active { get; set; }
        public byte[]? Avatar { get; set; }
    }
}
