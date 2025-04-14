using OpenVision.Server.Configuration;
using OpenVision.Server.Core.Configuration;
using OpenVision.Server.Core.Helpers;
using Serilog;

const string SeedArgs = "/seed";
const string MigrateOnlyArgs = "/migrateonly";

var configuration = ProgramHelper.GetConfiguration<Program>(args);

Log.Logger = new LoggerConfiguration().ReadFrom
    .Configuration(configuration)
    .CreateLogger();

try
{
    DockerHelpers.ApplyDockerConfiguration(configuration);

    var builder = WebApplication.CreateBuilder(args);
    builder.ConfigureHostBuilder<Program>(args);
    builder.ConfigureOpenVisionServer();

    var app = builder.Build();
    app.ConfigureOpenVisionServerPipeline();

    var migrationComplete = await ApplyDbMigrationsWithDataSeedAsync(args, configuration, app);
    if (await MigrateOnlyOperationAsync(args, app, migrationComplete)) return;

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

static async Task<bool> MigrateOnlyOperationAsync(string[] args, IHost host, bool migrationComplete)
{
    if (args.Any(x => x == MigrateOnlyArgs))
    {
        await host.StopAsync();

        if (!migrationComplete)
        {
            Environment.ExitCode = -1;
        }

        return true;
    }

    return false;
}

static async Task<bool> ApplyDbMigrationsWithDataSeedAsync(string[] args, IConfiguration configuration, IHost host)
{
    var applyDbMigrationWithDataSeedFromProgramArguments = args.Any(x => x == SeedArgs);
    if (applyDbMigrationWithDataSeedFromProgramArguments) args = [.. args.Except([SeedArgs])];

    var seedConfiguration = configuration.GetSection(nameof(SeedConfiguration)).Get<SeedConfiguration>()!;
    var databaseMigrationsConfiguration = configuration.GetSection(nameof(DatabaseMigrationsConfiguration))
        .Get<DatabaseMigrationsConfiguration>()!;

    // Bind the database configuration and Swagger configuration.
    var databaseConfiguration = configuration
        .GetSection(nameof(DatabaseConfiguration))
        .Get<DatabaseConfiguration>()!;

    return await ProgramHelper.ApplyDbMigrationsWithDataSeedAsync(
        host,
        applyDbMigrationWithDataSeedFromProgramArguments,
        databaseConfiguration.UsePooledDbContext,
        seedConfiguration,
        databaseMigrationsConfiguration);
}