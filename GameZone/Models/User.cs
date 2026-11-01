using GameZone.Enums;

namespace GameZone.Models

{
    public class User : BaseEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public Role Role { get; set; } = Role.User;
        public bool IsVerified { get; set; }
        public string? Code { get; set; }
        public Cart? Cart { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
