using System.Runtime.CompilerServices;

namespace BackEnd.DTOs.Auth
{
    public class RegisterResponse
    {
        public string Token { get; set; } = default!;
        public Guid UserID { get; set; } = default!;
        public string Username { get; set; } = default!;
    }
}
