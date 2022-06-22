namespace ModelApp.Domain.Dtos
{
    [Serializable]
    public class UserDto
    {
        public int? Id { get; set; }
        public string? UserRole { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool? Active { get; set; }
        public byte[]? Avatar { get; set; }
    }
}
