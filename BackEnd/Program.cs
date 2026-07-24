using BackEnd.Data;
using BackEnd.DTOs.Auth;
using BackEnd.Services.Auth.Implementation;
using BackEnd.Services.Auth.Interface;
using BackEnd.Services.Campaign.Implementation;
using BackEnd.Services.Campaign.Interface;
using BackEnd.Services.Character.Implementation;
using BackEnd.Services.Character.Interface;
using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Threading.RateLimiting;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BackEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Load environment variables
            Env.Load();

            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            builder.Services.AddRateLimiter(options =>
            {
                // Reject with 429 (the correct status) rather than the default 503.
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.AddPolicy("Fixed", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 100,                 // 100 requests
                            Window = TimeSpan.FromMinutes(1),  // per minute, per IP
                            QueueLimit = 0
                        }));

                options.AddPolicy("auth", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 10,                  // 10 attempts
                            Window = TimeSpan.FromMinutes(1),  // per minute, per IP
                            QueueLimit = 0
                        }));
            });

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:5173",               
                                        "https://dnd-campaign-wine.vercel.app",
                                        "https://dnd-campaign-git-dashboard-sam-howard-s-projects.vercel.app")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            // Validators
            builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
            builder.Services.AddFluentValidationAutoValidation();


            // DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(Environment.GetEnvironmentVariable("SUPABASE_CONNECTION_STRING")));

            // Services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IJwtProvider, JwtProvider>();
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<ICampaignRepository, CampaignRepository>();
            builder.Services.AddScoped<ICampaignService, CampaignService>();
            builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();

            // Don't remap inbound claim types, so the claim names we read in the
            // controllers ("sub") are exactly the ones JwtProvider writes.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            // JWT Authentication — the token arrives in the Authorization: Bearer
            // header (default JwtBearer behavior), so no custom token reader is needed.
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtKey = builder.Configuration["Jwt:Key"]
                    ?? throw new InvalidOperationException("JWT Key is missing from configuration.");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)
                    )
                };
            });

            // Controllers + Swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseForwardedHeaders();

            app.UseCors("AllowFrontend");

            app.UseRateLimiter();

            // Swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            // Authentication + Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
