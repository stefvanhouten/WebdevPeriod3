using FluentMigrator.Runner;

namespace WebdevPeriod3.Services
{
    /// <summary>
    /// This service can be used to update the database to the latest schema
    /// </summary>
    public class MigrationService
    {
        private readonly IMigrationRunner _migrationRunner;

        public MigrationService(IMigrationRunner migrationRunner)
        {
            _migrationRunner = migrationRunner;
        }

        /// <summary>
        /// Update the database to the latest schema
        /// </summary>
        public void UpdateDatabase()
        {
            _migrationRunner.MigrateUp();
        }
    }
}
