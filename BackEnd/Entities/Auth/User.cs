namespace BackEnd.Entities.Auth
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public bool IsActive { get; set; } = true;
    }
}
