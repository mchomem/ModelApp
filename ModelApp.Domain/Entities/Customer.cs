namespace ModelApp.Domain.Entities
{
    [Serializable]
    public class Customer
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public DateTime? DateBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool? Active { get; set; }
    }
}
