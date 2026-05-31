using Microsoft.AspNetCore.Identity;
using BackEnd.DTOs.Auth;
using BackEnd.Entities.Auth;
using BackEnd.Services.Interfaces.Auth;
using Microsoft.AspNetCore.Identity.Data;
using BackEnd.Data;

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

        public async Task<LoginResponse> LoginAsync(BackEnd.DTOs.Auth.LoginRequest request)
        {
            // get user
            var user = await _UserRepository.GetByEmailAsync(request.Email);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password.");

            // verify password
            var ValidPassword = _PasswordHasher.Verify(request.Password, user.Password);
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

        public async Task<RegisterResponse> RegisterAsync(BackEnd.DTOs.Auth.RegisterRequest request)
        {

            var _ExistingUser = await _UserRepository.GetByEmailAsync(request.Email);

            if (_ExistingUser != null)
                throw new InvalidOperationException("Email already in use.");

            var user = new User
            {
                Email     = request.Email,
                Username  = request.Username,
                Password  = _PasswordHasher.Hash(request.Password),
                CreatedAt = DateTime.UtcNow,
                IsActive  = true
            };

            await _UserRepository.CreateAsync(user);

            var token = _JwtProvider.GenerateToken(user);

            return new RegisterResponse
            {
                Token = token,
                UserID = user.UserID,
                Username = user.Username,
                Email = user.Email
            };

        }
    }
}
