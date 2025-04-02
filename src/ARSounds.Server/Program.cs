using ARSounds.Server.Core.Configuration;
using ARSounds.Server.Core.Helpers;
using ARSounds.ServiceDefaults;
using Serilog;

const string ConnectionStringName = "arsounds";

var configuration = StartupHelper.GetConfiguration<Program>(args);

Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

try
{
    DockerHelpers.ApplyDockerConfiguration(configuration);

    var builder = WebApplication.CreateBuilder(args);
    builder.ConfigureHostBuilder<Program>(args);

    var connectionString = builder.Configuration.GetConnectionString(ConnectionStringName)!;
    var databaseProviderConfiguration = builder.Configuration.GetSection(nameof(DatabaseProviderConfiguration)).Get<DatabaseProviderConfiguration>()!;
    var apiConfiguration = builder.Configuration.GetSection(nameof(ApiConfiguration)).Get<ApiConfiguration>()!;

    builder.AddServiceDefaults();
    builder.AddApiServiceDefaults(apiConfiguration, connectionString, databaseProviderConfiguration);

    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    var app = builder.Build();

    app.AddApplicationDefaults(apiConfiguration);

    app.MapDefaultEndpoints();

    app.MapFallbackToFile("/index.html");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}