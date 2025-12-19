using Bookify.Domain.Apartment;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Api.Extentions
{
    public static class ApplicationBuilderExtensions
    {

        public static void AddDamyData(this WebApplication app)
        {
            var scope = app.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<Bookify.Infrastracture.ApplicationDbContext>();
            try
            {
                if (!context.Set<ApartmentModel>().Any())
                {
                    var apartments = Infrastracture.Configurations.DamyData.GetApartmentDemyData();
                    context.Set<ApartmentModel>().AddRange(apartments);
                    context.SaveChanges();
                    app.Logger.LogInformation("Damy data added successfully.");
                }
            }
            catch (Exception ex)
            {
                app.Logger.LogError(ex, "An error occurred while adding damy data.");
                throw;
            }
        }

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
