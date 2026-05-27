
using Npgsql;
using DotNetEnv;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BackEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            {
                // Load environment variables from .env
                Env.Load();

                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container
                builder.Services.AddControllers();

                // Add Swagger support (works with .NET 8)
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();

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
}
