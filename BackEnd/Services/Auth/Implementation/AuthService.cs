using BackEnd.DTOs.Auth;
using BackEnd.Entities.Auth;
using BackEnd.ErrorHandling; // <-- This is where Result<T> should live
using BackEnd.Services.Auth.Interface;
using Microsoft.AspNetCore.Components.Forms;

namespace BackEnd.Services.Auth.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IPasswordHasher _passwordHasher;
        public AuthService(
            IUserRepository userRepository,
            IJwtProvider jwtProvider,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _passwordHasher = passwordHasher;
        }


        public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request)
        {
            // get user
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
                return Result<AuthResponse>.Fail("Invalid email or password.");

            // verify password
            bool validPassword = _passwordHasher.Verify(request.Password, user.Password);
            if (!validPassword)
                return Result<AuthResponse>.Fail("Invalid email or password.");

            // generate jwt
            var token = _jwtProvider.GenerateToken(user);

            var response = new AuthResponse
            {
                Token = token,
                UserID = user.UserID,
                Username = user.Username
            };

            return Result<AuthResponse>.Ok(response);
        }

        public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);

            if (existingUser != null)
                return Result<AuthResponse>.Fail("Email already in use.");

            var user = new User
            {
                Email = request.Email,
                Username = request.Username,
                Password = _passwordHasher.Hash(request.Password),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _userRepository.CreateAsync(user);

            var token = _jwtProvider.GenerateToken(user);

            var response = new AuthResponse
            {
                Token = token,
                UserID = user.UserID,
                Username = user.Username
            };

            return Result<AuthResponse>.Ok(response);
        }
    }
}
