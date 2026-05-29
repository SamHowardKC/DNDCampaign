namespace BackEnd.Entities.Auth
{
    public class User
    {
        public Guid UserID { get; set; } = Guid.NewGuid();

        public string Email { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
