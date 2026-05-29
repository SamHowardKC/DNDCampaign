using Microsoft.AspNetCore.Identity;
using BackEnd.DTOs.Auth;
using BackEnd.Entities.Auth;
using BackEnd.Services.Interfaces.Auth;

namespace BackEnd.Services.Implementation.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _UserRepository;
        private readonly IJwtProvider _JwtProvider;
        private readonly IPasswordHasher _PasswordHasher;

        public AuthService(
            IUserRepository userRepository, 
            IJwtProvider jwtProvider, 
            IPasswordHasher passwordHasher)
        {
            _UserRepository = userRepository;
            _JwtProvider = jwtProvider;
            _PasswordHasher = passwordHasher;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            // get user
            var user = await _UserRepository.GetByEmailAsync(request.Email);
            if (user == null || !_PasswordHasher.Verify(user.Password, request.Password))
                throw new UnauthorizedAccessException("Invalid email or password.");

            // verify password
            var ValidPassword = _PasswordHasher.Verify(user.Password, request.Password);
            if (!ValidPassword)
                throw new UnauthorizedAccessException("Invalid email or password.");

            // generate jwt
            var token = _JwtProvider.GenerateToken(user);

            // return response
            return new LoginResponse 
            { 
                Token = token 
            };
        }
    }
}
