using System.Reflection;
using MySqlMigrationAssembly = ARSounds.EntityFramework.MySql.Helpers.MigrationAssembly;
using PostgreSQLMigrationAssembly = ARSounds.EntityFramework.PostgreSQL.Helpers.MigrationAssembly;
using SqlMigrationAssembly = ARSounds.EntityFramework.SqlServer.Helpers.MigrationAssembly;

namespace ARSounds.Server.Core.Configuration;

/// <summary>
/// Configuration class to retrieve migration assembly names based on database provider type.
/// </summary>
public static class MigrationAssemblyConfiguration
{
    /// <summary>
    /// Retrieves the migration assembly name based on the specified database provider type.
    /// </summary>
    /// <param name="databaseProvider">The database provider configuration.</param>
    /// <returns>The name of the migration assembly.</returns>
    public static string? GetMigrationAssemblyByProvider(DatabaseProviderConfiguration databaseProvider)
    {
        return databaseProvider.ProviderType switch
        {
            DatabaseProviderType.SqlServer => typeof(SqlMigrationAssembly).GetTypeInfo().Assembly.GetName().Name,
            DatabaseProviderType.PostgreSQL => typeof(PostgreSQLMigrationAssembly).GetTypeInfo().Assembly.GetName().Name,
            DatabaseProviderType.MySql => typeof(MySqlMigrationAssembly).GetTypeInfo().Assembly.GetName().Name,
            _ => throw new ArgumentOutOfRangeException(nameof(databaseProvider.ProviderType), databaseProvider.ProviderType, "Unsupported database provider type.")
        };
    }
}
