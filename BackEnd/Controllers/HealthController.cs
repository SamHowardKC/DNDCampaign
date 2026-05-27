using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet("health")]
        public IActionResult ApiHealth()
        {
            return Ok(new { status = "API is running" });
        }

        [HttpGet("db-test")]
        public async Task<IActionResult> DbTest()
        {
            var connString = Environment.GetEnvironmentVariable("SUPABASE_CONNECTION_STRING");

            // Null or empty connection string check
            if (string.IsNullOrWhiteSpace(connString))
            {
                return StatusCode(500, new
                {
                    error = "SUPABASE_CONNECTION_STRING environment variable is missing or empty"
                });
            }

            try
            {
                await using var conn = new NpgsqlConnection(connString);
                await conn.OpenAsync();

                await using var cmd = new NpgsqlCommand("SELECT NOW()", conn);
                var result = await cmd.ExecuteScalarAsync();

                return Ok(new
                {
                    message = "Connected!",
                    serverTime = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Database connection failed",
                    details = ex.Message
                });
            }
        }
    }
}
