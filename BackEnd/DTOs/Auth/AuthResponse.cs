namespace BackEnd.DTOs.Auth
{
    public class AuthResponse
    {
        public required string Token { get; set; }
        public required Guid UserID { get; set; }
        public required string Username { get; set; }
    }
}
