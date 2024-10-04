namespace BlogManagementAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; } // Note: Store hashed passwords in production
        public string? Role { get; set; } // e.g., "Admin", "User"
    }
}
