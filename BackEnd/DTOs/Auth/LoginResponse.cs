namespace BackEnd.DTOs.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; } = default!;
        public bool Success { get; set; }
    }
}
