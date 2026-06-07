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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:5173",               
                                        "https://dnd-campaign-wine.vercel.app",
                                        "https://dnd-campaign-30u65f2z6-sam-howard-s-projects.vercel.app")
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

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            // JWT Authentication (reads token from HttpOnly cookie)
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // Read JWT from cookie instead of Authorization header
                        if (context.Request.Cookies.ContainsKey("jwt"))
                        {
                            context.Token = context.Request.Cookies["jwt"];
                        }

                        return Task.CompletedTask;
                    }
                };

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

            app.UseCors("AllowFrontend");

            // Authentication + Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
