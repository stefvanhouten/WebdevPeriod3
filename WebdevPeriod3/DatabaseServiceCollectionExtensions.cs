using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using WebdevPeriod3.Migrations;
using WebdevPeriod3.Services;

namespace WebdevPeriod3
{
    public static class DatabaseServiceCollectionExtensions
    {
        /// <summary>
        /// Add the services required to run database migrations
        /// </summary>
        /// <param name="services"></param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddMigrationRunner(this IServiceCollection services)
        {
            return services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add SQLite support to FluentMigrator
                    .AddMySql5()
                    // Set the connection string
                    .WithGlobalConnectionString("Server=127.0.0.1;Database=mydb;Uid=stef;Pwd=password")
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(AddUsersTable).Assembly).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .AddScoped<MigrationService>();
        }
    }
}
