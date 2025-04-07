using ARSounds.Aspire.AppHost.Configuration;
using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<IResourceWithConnectionString> visionResourceBuilder;
IResourceBuilder<IResourceWithConnectionString> arsoundsResourceBuilder;

var parameters = builder.Configuration
    .GetSection("Parameters");

var databaseProviderType = parameters
    .GetValue<DatabaseProviderType>(nameof(DatabaseProviderType));

switch (databaseProviderType)
{
    case DatabaseProviderType.MySql:
        {
            var resource = builder.AddMySql("mysql")
                .WithDataVolume("ARSounds.Aspire.AppHost-mysql-data");
            visionResourceBuilder = resource.AddDatabase("vision");
            arsoundsResourceBuilder = resource.AddDatabase("arsounds");
            break;
        }
    case DatabaseProviderType.PostgreSQL:
        {
            var resource = builder.AddPostgres("postgresql")
                .WithDataVolume("ARSounds.Aspire.AppHost-postgresql-data");
            visionResourceBuilder = resource.AddDatabase("vision");
            arsoundsResourceBuilder = resource.AddDatabase("arsounds");
            break;
        }
    default:
        {
            var resource = builder.AddSqlServer("sqlserver")
                .WithDataVolume("ARSounds.Aspire.AppHost-sqlserver-data");
            visionResourceBuilder = resource.AddDatabase("vision");
            arsoundsResourceBuilder = resource.AddDatabase("arsounds");
            break;
        }
}

var databaseProviderTypeParameter = builder.AddParameter("DatabaseProviderType");
var usePooledDbContextParameter = builder.AddParameter("UsePooledDbContext");

builder.AddProject<Projects.OpenVision_Client>("openvision-client");
builder.AddProject<Projects.OpenVision_Server>("openvision-server")
       .WithEnvironment("DatabaseConfiguration:ProviderType", databaseProviderTypeParameter)
       .WithEnvironment("DatabaseConfiguration:UsePooledDbContext", usePooledDbContextParameter)
       .WithReference(visionResourceBuilder);

builder.AddProject<Projects.ARSounds_Server>("arsounds-server")
       .WithEnvironment("DatabaseConfiguration:ProviderType", databaseProviderTypeParameter)
       .WithEnvironment("DatabaseConfiguration:UsePooledDbContext", usePooledDbContextParameter)
       .WithReference(arsoundsResourceBuilder);

builder.Build().Run();