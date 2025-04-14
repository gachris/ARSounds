using ARSounds.Server.Core;
using ARSounds.Server.Core.Configuration;
using ARSounds.Server.Core.Middlewares;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Microsoft.Extensions.Hosting;

internal static class ProgramHelper
{
    /// <summary>
    /// Retrieves the application configuration.
    /// </summary>
    /// <typeparam name="T">The type whose user secrets are being loaded (typically the entry point type).</typeparam>
    /// <param name="args">Command-line arguments passed to the application.</param>
    /// <returns>The application configuration.</returns>
    public static IConfiguration GetConfiguration<T>(string[] args) where T : class
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var isDevelopment = environment == Environments.Development;

        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddJsonFile("serilog.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"serilog.{environment}.json", optional: true, reloadOnChange: true);

        if (isDevelopment)
        {
            configurationBuilder.AddUserSecrets<T>(optional: true);
        }

        configurationBuilder.AddCommandLine(args);
        configurationBuilder.AddEnvironmentVariables();

        return configurationBuilder.Build();
    }

    /// <summary>
    /// Configures the host builder for the application.
    /// </summary>
    public static void ConfigureHostBuilder<T>(this WebApplicationBuilder builder, string[] args) where T : class
    {
        builder.Configuration.AddJsonFile("serilog.json", optional: true, reloadOnChange: true);
        var env = builder.Environment;
        builder.Configuration.AddJsonFile($"serilog.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

        if (env.IsDevelopment())
        {
            builder.Configuration.AddUserSecrets<T>(optional: true);
        }

        builder.Configuration.AddEnvironmentVariables();
        builder.Configuration.AddCommandLine(args);

        builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();

        builder.Host.UseSerilog((hostContext, loggerConfig) =>
        {
            loggerConfig
                .ReadFrom.Configuration(hostContext.Configuration)
                .Enrich.WithProperty("ApplicationName", hostContext.HostingEnvironment.ApplicationName);
        });
    }

    /// <summary>
    /// Adds the default services and configurations for the ARSounds API application.
    /// </summary>
    /// <param name="builder">The WebApplicationBuilder instance.</param>
    /// <returns>The updated WebApplicationBuilder instance.</returns>
    public static IHostApplicationBuilder ConfigureARSoundsServices(this WebApplicationBuilder builder)
    {
        // Retrieve configurations
        var databaseConfiguration = builder.Configuration
            .GetSection(nameof(DatabaseConfiguration))
            .Get<DatabaseConfiguration>()!;

        var corsConfiguration = builder.Configuration
            .GetSection(nameof(CorsConfiguration))
            .Get<CorsConfiguration>()!;

        var swaggerConfiguration = builder.Configuration
            .GetSection(nameof(SwaggerConfiguration))
            .Get<SwaggerConfiguration>()!;

        var oidcConfiguration = builder.Configuration
            .GetSection(nameof(OidcConfiguration))
            .Get<OidcConfiguration>()!;

        var openVisionOptions = builder.Configuration
            .GetSection(nameof(OpenVisionOptions))
            .Get<OpenVisionOptions>()!;

        var connectionString = builder.Configuration.GetConnectionString(databaseConfiguration.ConnectionName)!;

        // Add ServiceDefault
        builder.AddServiceDefaults();

        // Add MediatR
        builder.Services.AddDefaultMediatR();

        // Add GraphQL (with pooling flag)
        builder.Services.AddGraphQL(databaseConfiguration.UsePooledDbContext);

        // Register AutoMapper, repositories, and core services
        builder.Services.AddAutoMapperConfiguration();
        builder.Services.AddRepositories();
        builder.Services.AddARSoundsCoreServices();
        builder.Services.AddCurrentUserService();

        // Register DbContext based on pooling preference
        if (databaseConfiguration.UsePooledDbContext)
        {
            builder.Services.AddPooledDbContextFactory(connectionString, databaseConfiguration, options =>
            {
                options.UseLazyLoadingProxies();
            });
        }
        else
        {
            builder.Services.AddDbContext(connectionString, databaseConfiguration, options =>
            {
                options.UseLazyLoadingProxies();
            });
        }

        // Add HttpContextAccessor
        builder.Services.AddHttpContextAccessor();

        // Register OpenVision resources
        builder.Services.AddOpenVisionResources(options =>
        {
            options.ApplicationName = openVisionOptions.ApplicationName;
            options.ServerUrl = openVisionOptions.ServerUrl;
            options.DatabaseApiKey = openVisionOptions.DatabaseApiKey;
        });

        // Register authentication
        builder.Services.AddDefaultAuthentication()
            .AddDefaultJwtBearer(oidcConfiguration);

        // Register authorization
        builder.Services.AddAuthorizationBuilder()
            .AddDefaultPolicy(oidcConfiguration);

        // Register CORS
        builder.Services.AddDefaultCors(corsConfiguration);

        // Register API controllers, endpoints explorer, and Swagger generation
        builder.Services.AddDefaultControllers()
            .AddDefaultJsonOptions();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddDefaultSwaggerGen(swaggerConfiguration);

        return builder;
    }

    /// <summary>
    /// Configures the middleware and HTTP request pipeline for the ARSounds API application.
    /// </summary>
    /// <param name="app">The WebApplication instance.</param>
    /// <returns>The updated WebApplication instance.</returns>
    public static IApplicationBuilder ConfigureARSoundsPipeline(this WebApplication app)
    {
        var databaseConfiguration = app.Configuration
            .GetSection(nameof(DatabaseConfiguration))
            .Get<DatabaseConfiguration>()!;

        var swaggerConfiguration = app.Configuration
            .GetSection(nameof(SwaggerConfiguration))
            .Get<SwaggerConfiguration>()!;

        // Serve static files and default documents
        app.UseStaticFiles();
        app.UseDefaultFiles();

        // Forward headers (e.g., for proxy scenarios)
        app.UseDefaultsForwardHeaders();

        // Configure development environment (e.g., developer exception page, OpenAPI mapping)
        if (app.Environment.IsDevelopment())
        {
            // Use developer exception page
            app.UseDeveloperExceptionPage();
        }

        // Configure global exception handling
        app.ConfigureExceptionHandler();

        // Enable CORS
        app.UseCors();

        // Enforce HTTPS redirection
        app.UseHttpsRedirection();

        // Set up routing
        app.UseRouting();

        // Use authentication middleware
        app.UseAuthentication();

        // Use custom challenge middleware
        app.UseMiddleware<ChallengeMiddleware>();

        // Apply authorization
        app.UseAuthorization();

        // Map API controllers and GraphQL endpoints
        app.MapControllers()
            .RequireAuthorization();

        app.MapGraphQL()
            .RequireAuthorization();

        // Enable Swagger and Swagger UI
        app.UseSwagger(swaggerConfiguration);

        // Migrate the database
        app.MigrateDatabase(databaseConfiguration.UsePooledDbContext);

        // Fallback routing to serve the SPA
        app.MapFallbackToFile("/index.html");

        return app;
    }
}