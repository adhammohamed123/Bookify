using Microsoft.EntityFrameworkCore;

namespace Bookify.Api.Extentions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task ApplayMigrationAsync(this WebApplication app)
        {
            var scope = app.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<Bookify.Infrastracture.ApplicationDbContext>();

            try
            {

                await context.Database.MigrateAsync();
                app.Logger.LogInformation("Database migration applied successfully.");
            }
            catch (Exception ex)
            {
                app.Logger.LogError(ex, "An error occurred while applying database migration.");
                throw;
            }
        }
    }
}
