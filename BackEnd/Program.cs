
using BackEnd.Data;
using BackEnd.Services.Implementation.Auth;
using BackEnd.Services.Interfaces.Auth;
using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Npgsql;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace BackEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            // Load environment variables from .env
            Env.Load();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // register DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(Environment.GetEnvironmentVariable("SUPABASE_CONNECTION_STRING")));

            // register services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IJwtProvider, JwtProvider>();
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

            // add controllers
            builder.Services.AddControllers();

            // add swagger support
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseCors("AllowFrontend");

            // Enable Swagger only in development
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
            
        }
    }
}
