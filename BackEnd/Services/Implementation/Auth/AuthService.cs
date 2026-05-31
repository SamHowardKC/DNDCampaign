using BackEnd.DTOs.Auth;
using BackEnd.Entities.Auth;
using BackEnd.ErrorHandling; // <-- This is where Result<T> should live
using BackEnd.Services.Implementation.Auth;
using BackEnd.Services.Interfaces.Auth;

namespace BackEnd.Services.Implementation.Auth
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


        public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
        {
            // get user
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
                return Result<LoginResponse>.Fail("Invalid email or password.");

            // verify password
            bool validPassword = _passwordHasher.Verify(request.Password, user.Password);
            if (!validPassword)
                return Result<LoginResponse>.Fail("Invalid email or password.");

            // generate jwt
            var token = _jwtProvider.GenerateToken(user);

            var response = new LoginResponse
            {
                Token = token,
                UserID = user.UserID,
                Username = user.Username
            };

            return Result<LoginResponse>.Ok(response);
        }

        public async Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);

            if (existingUser != null)
                return Result<RegisterResponse>.Fail("Email already in use.");

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

            var response = new RegisterResponse
            {
                Token = token,
                UserID = user.UserID,
                Username = user.Username
            };

            return Result<RegisterResponse>.Ok(response);
        }
    }
}
