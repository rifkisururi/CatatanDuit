using Microsoft.EntityFrameworkCore;

namespace CatatanDuit.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(WebApplication app)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            logger.LogInformation("Attempting to connect to database...");
            await dbContext.Database.MigrateAsync();
            logger.LogInformation("Database connection successful and migrations applied");
        }
        catch (Npgsql.NpgsqlException ex)
        {
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogWarning(ex, "PostgreSQL database is not available. Please ensure PostgreSQL is running and the connection string in appsettings.json is correct.");
            logger.LogWarning("Application will continue but database features will not work until database is available.");
        }
        catch (Exception ex)
        {
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogWarning(ex, "Database migration failed. Application will continue with limited functionality.");
        }
    }
}
