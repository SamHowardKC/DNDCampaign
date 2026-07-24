using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackEnd.Entities.Auth;
using BackEnd.Services.Auth.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BackEnd.Services.Auth.Implementation
{
    public class JwtProvider : IJwtProvider
    {
        private readonly IConfiguration _config;

        public JwtProvider(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User user)
        {
            var keyString = _config["Jwt:Key"];

            if (string.IsNullOrEmpty(keyString))
                throw new Exception("JWT Key is missing from configuration.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Identify the user with the standard "sub" claim. We read this exact
            // claim name back in the controllers, so generation and validation agree
            // without depending on ASP.NET's inbound/outbound claim-type remapping.
            // Audience is supplied once via the constructor below — adding it here too
            // would produce a duplicate "aud" entry.
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
