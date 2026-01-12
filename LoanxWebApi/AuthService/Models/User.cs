namespace AuthService.Models
{
    public class User
    {
        public int Id { get; set; }          // User number
        public string? Username { get; set; } // Login name
        public string? Password { get; set; } // Password
    }
}
