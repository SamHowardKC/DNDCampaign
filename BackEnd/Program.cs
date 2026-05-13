
using Npgsql;
using DotNetEnv;

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

                // ✅ Add Swagger support (works with .NET 8)
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();

                // ✅ Enable Swagger only in development
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();
                app.UseAuthorization();
                app.MapControllers();

                // ✅ Test endpoint to verify Supabase connection
                app.MapGet("/db-test", async () =>
                {
                    var connString = Environment.GetEnvironmentVariable("SUPABASE_CONNECTION_STRING");

                    await using var conn = new NpgsqlConnection(connString);
                    await conn.OpenAsync();

                    await using var cmd = new NpgsqlCommand("SELECT NOW()", conn);
                    var result = await cmd.ExecuteScalarAsync();

                    return new { message = "Connected!", serverTime = result };
                });

                app.Run();
            }
        }
    }
}
