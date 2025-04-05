using System.Text.Json;
using System.Text.Json.Serialization;
using ARSounds.Server.Core.Auth;
using ARSounds.Server.Core.Configuration;
using ARSounds.Server.Core.Contracts;
using ARSounds.Server.Core.Mappers;
using ARSounds.Server.Core.Services;
using ARSounds.Server.Core.Utils;
using ARSounds.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ARSounds.Server.Core.Middlewares;
using Microsoft.AspNetCore.Diagnostics;
using OpenVision.Shared.Exceptions;
using OpenVision.Shared.Responses;
using OpenVision.Shared;
using System.Net;
using ARSounds.Server.Core.Filters;

namespace Microsoft.Extensions.Hosting;

/// <summary>
/// Extension methods for configuring and enhancing a WebApplicationBuilder in ASP.NET Core.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Adds default services and configurations for an API application.
    /// </summary>
    /// <param name="builder">The WebApplicationBuilder instance.</param>
    /// <param name="apiConfiguration">API configuration settings.</param>
    /// <param name="connectionString">Database connection string.</param>
    /// <param name="databaseProviderConfiguration">Database provider configuration.</param>
    /// <returns>The WebApplicationBuilder instance.</returns>
    public static IHostApplicationBuilder AddARSoundsServerDefaults(
        this WebApplicationBuilder builder,
        ApiConfiguration apiConfiguration,
        string connectionString,
        DatabaseProviderConfiguration databaseProviderConfiguration,
        OidcOptions oidcOptions, 
        OpenVisionResourcesOptions openVisionOptions)
    {
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        // Add AutoMapper with MappingProfile
        builder.Services.AddAutoMapper(typeof(MappingProfile));

        // Add transient services
        builder.Services.AddTransient<ITargetsService, TargetsService>();

        // Add DbContext based on database provider
        builder.Services.AddDbContext(connectionString, databaseProviderConfiguration);

        // Add HttpContextAccessor
        builder.Services.AddHttpContextAccessor();

        // Add AddOpenVisionResourceFactory
        builder.Services.AddOpenVisionResources(options =>
        {
            options.ApplicationName = openVisionOptions.ApplicationName;
            options.ServerUrl = openVisionOptions.ServerUrl;
            options.DatabaseApiKey = openVisionOptions.DatabaseApiKey;
        });

        // Add UriService
        builder.Services.AddUriService();

        // Add authentication services
        builder.Services.AddAuthentication(oidcOptions);

        // Add authorization policies
        builder.Services.AddAuthorizationPolicy(oidcOptions);

        // Add CORS policies
        builder.Services.AddCors(apiConfiguration);

        // Add API controllers
        builder.Services.AddApiControllers();

        // Add API explorer endpoints
        builder.Services.AddEndpointsApiExplorer();

        // Add Swagger generation
        builder.Services.AddSwaggerGen(apiConfiguration, oidcOptions);

        return builder;
    }

    public static IServiceCollection AddOpenVisionResources(this IServiceCollection services, Action<OpenVisionResourcesOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configureOptions);

        services.AddSingleton<IOpenVisionResources, OpenVisionResources>();
        services.Configure(configureOptions);

        return services;
    }

    /// <summary>
    /// Adds DbContext configuration based on the database provider type.
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <param name="connectionString">Database connection string.</param>
    /// <param name="databaseProviderConfiguration">Database provider configuration.</param>
    /// <returns>The IServiceCollection instance.</returns>
    public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString, DatabaseProviderConfiguration databaseProviderConfiguration)
    {
        var migrationAssembly = MigrationAssemblyConfiguration.GetMigrationAssemblyByProvider(databaseProviderConfiguration);

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            switch (databaseProviderConfiguration.ProviderType)
            {
                case DatabaseProviderType.MySql:
                    options.UseMySQL(connectionString, mySqlOptions =>
                    {
                        mySqlOptions.MigrationsAssembly(migrationAssembly);
                        mySqlOptions.EnableRetryOnFailure();
                    });
                    break;
                case DatabaseProviderType.PostgreSQL:
                    options.UseNpgsql(connectionString, npgsqlOptions =>
                    {
                        npgsqlOptions.MigrationsAssembly(migrationAssembly);
                        npgsqlOptions.EnableRetryOnFailure();
                    });
                    break;
                case DatabaseProviderType.SqlServer:
                    options.UseSqlServer(connectionString, sqlServerOptions =>
                    {
                        sqlServerOptions.MigrationsAssembly(migrationAssembly);
                        sqlServerOptions.EnableRetryOnFailure();
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        });

        return services;
    }

    /// <summary>
    /// Configures authentication services.
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <param name="apiConfiguration">API configuration settings.</param>
    /// <returns>The IServiceCollection instance.</returns>
    public static IServiceCollection AddAuthentication(this IServiceCollection services, OidcOptions oidcOptions)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.Authority = oidcOptions.Authority;
            options.RequireHttpsMetadata = oidcOptions.RequireHttpsMetadata;
            options.Audience = oidcOptions.Audience;
        });

        return services;
    }

    /// <summary>
    /// Adds authorization policies based on API configuration settings.
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <param name="oidcOptions">API configuration settings.</param>
    /// <returns>The IServiceCollection instance.</returns>
    public static IServiceCollection AddAuthorizationPolicy(this IServiceCollection services, OidcOptions oidcOptions)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(AuthorizationConsts.BearerPolicy, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireScope();
                foreach (var scope in oidcOptions.Scopes)
                {
                    policy.RequireClaim("scope", scope);
                }
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            });

        return services;
    }

    /// <summary>
    /// Adds a singleton UriService instance.
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <returns>The IServiceCollection instance.</returns>
    public static IServiceCollection AddUriService(this IServiceCollection services)
    {
        services.AddSingleton<IUriService>(serviceProvider =>
        {
            var accessor = serviceProvider.GetRequiredService<IHttpContextAccessor>() ?? throw new InvalidOperationException("IHttpContextAccessor is not registered.");
            var context = accessor.HttpContext ?? throw new InvalidOperationException("HttpContext is not available.");
            var request = context.Request;
            var uri = $"{request.Scheme}://{request.Host.ToUriComponent()}";

            return new UriService(uri);
        });

        return services;
    }

    /// <summary>
    /// Adds CORS policies based on API configuration settings.
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <param name="apiConfiguration">API configuration settings.</param>
    /// <returns>The IServiceCollection instance.</returns>
    public static IServiceCollection AddCors(this IServiceCollection services, ApiConfiguration apiConfiguration)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                builder =>
                {
                    if (apiConfiguration.CorsAllowAnyOrigin)
                    {
                        builder.AllowAnyOrigin();
                    }
                    else if (apiConfiguration.CorsAllowOrigins != null && apiConfiguration.CorsAllowOrigins.Length > 0)
                    {
                        builder.WithOrigins(apiConfiguration.CorsAllowOrigins);
                    }

                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
        });

        return services;
    }

    /// <summary>
    /// Adds API controllers with customized JSON serialization settings.
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <returns>The IServiceCollection instance.</returns>
    public static IServiceCollection AddApiControllers(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add(new ValidateModelFilter());
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        });

        return services;
    }

    /// <summary>
    /// Adds Swagger generation with OAuth2 security definitions.
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <param name="apiConfiguration">API configuration settings.</param>
    /// <returns>The IServiceCollection instance.</returns>
    public static IServiceCollection AddSwaggerGen(this IServiceCollection services, ApiConfiguration apiConfiguration, OidcOptions oidcOptions)
    {
        services.AddSwaggerGen(options =>
        {
            var openApiInfo = new OpenApiInfo { Title = apiConfiguration.Name, Version = apiConfiguration.Version };
            options.SwaggerDoc(apiConfiguration.Version, openApiInfo);

            // Define OAuth2 security scheme for Swagger UI
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{oidcOptions.Authority}/connect/authorize"),
                        TokenUrl = new Uri($"{oidcOptions.Authority}/connect/token"),
                        Scopes = oidcOptions.Scopes.ToDictionary(x => x, y => y)
                    }
                }
            });

            // Add custom operation filter for authorization checks
            options.OperationFilter<AuthorizeCheckOperationFilter>();
        });

        return services;
    }

    /// <summary>
    /// Configures application middleware and pipeline for the API application.
    /// </summary>
    /// <param name="app">The WebApplication instance.</param>
    /// <param name="apiConfiguration">API configuration settings.</param>
    /// <returns>The WebApplication instance.</returns>
    public static IApplicationBuilder AddARSoundsServerDefaults(this WebApplication app, ApiConfiguration apiConfiguration, OidcOptions oidcOptions)
    {
        app.UseDefaultFiles();

        app.MapStaticAssets();

        // Add headers forwarding configuration
        app.AddForwardHeaders();

        // Configure development environment settings
        app.ConfigureDevelopmentEnvironment();

        // Use Swagger and Swagger UI
        app.UseSwagger(apiConfiguration, oidcOptions);

        // Configure exception handling middleware
        app.ConfigureExceptionHandler();

        // Enable CORS
        app.UseCors();

        // Enable HTTPS redirection
        app.UseHttpsRedirection();

        // Enable routing
        app.UseRouting();

        // Use authentication middleware
        app.UseAuthentication();

        // Use custom challenge middleware
        app.UseMiddleware<ChallengeMiddleware>();

        // Use authorization middleware
        app.UseAuthorization();

        // Map API controllers
        app.MapControllers();

        // Migrate database
        app.MigrateDatabase();

        return app;
    }

    /// <summary>
    /// Configures headers forwarding options for the application.
    /// </summary>
    /// <param name="app">The WebApplication instance.</param>
    /// <returns>The WebApplication instance.</returns>
    public static IApplicationBuilder AddForwardHeaders(this WebApplication app)
    {
        var forwardingOptions = new ForwardedHeadersOptions()
        {
            ForwardedHeaders = ForwardedHeaders.All
        };

        forwardingOptions.KnownNetworks.Clear();
        forwardingOptions.KnownProxies.Clear();

        app.UseForwardedHeaders(forwardingOptions);

        return app;
    }

    /// <summary>
    /// Configures development environment settings for the application.
    /// </summary>
    /// <param name="app">The WebApplication instance.</param>
    /// <returns>The WebApplication instance.</returns>
    public static IApplicationBuilder ConfigureDevelopmentEnvironment(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.MapOpenApi();
        }

        return app;
    }

    /// <summary>
    /// Configures Swagger and Swagger UI for API documentation.
    /// </summary>
    /// <param name="app">The WebApplication instance.</param>
    /// <param name="apiConfiguration">API configuration settings.</param>
    /// <returns>The WebApplication instance.</returns>
    public static IApplicationBuilder UseSwagger(this WebApplication app, ApiConfiguration apiConfiguration, OidcOptions oidcOptions)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint(apiConfiguration.SwaggerEndpoint, apiConfiguration.Name);

            c.OAuthClientId(oidcOptions.SwaggerUIClientId);
            c.OAuthAppName(apiConfiguration.Name);
            c.OAuthUsePkce();
        });

        return app;
    }

    /// <summary>
    /// Migrates the database schema to the latest version.
    /// </summary>
    /// <param name="app">The WebApplication instance.</param>
    /// <returns>The WebApplication instance.</returns>
    public static IApplicationBuilder MigrateDatabase(this WebApplication app)
    {
        var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
        using var serviceScope = serviceScopeFactory.CreateScope();
        using var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();

        return app;
    }

    /// <summary>
    /// Configures global exception handling middleware to handle and log exceptions.
    /// </summary>
    /// <param name="app">The application builder instance.</param>
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        const string ContentType = "application/json";

        var JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };

        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    context.Response.ContentType = "application/json";
                    if (contextFeature.Error is HttpException exception)
                    {
                        // Handle known HTTP exceptions
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                        var result = JsonSerializer.Serialize(exception.ErrorResponseMessage, JsonSerializerOptions);

                        await context.Response.WriteAsync(result);
                    }
                    else
                    {
                        // Handle other unexpected exceptions
                        var errorCollection = new List<Error>();

                        var error = new Error(ResultCode.InternalServerError, contextFeature.Error.Message);

                        errorCollection.Add(error);

                        var response = new ResponseMessage(Guid.NewGuid(), StatusCode.Failed, errorCollection);

                        var result = JsonSerializer.Serialize(response, JsonSerializerOptions);

                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        context.Response.ContentType = ContentType;

                        await context.Response.WriteAsync(result);
                    }
                }
            });
        });
    }
}