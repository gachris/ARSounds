using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);
var databaseProvider = builder.Configuration.GetSection("Parameters").GetValue<string>("databaseProvider")!;

IResourceBuilder<IResourceWithConnectionString> visionResourceBuilder;
IResourceBuilder<IResourceWithConnectionString> arsoundsResourceBuilder;

if (databaseProvider == "SqlServer")
{
    var resource = builder.AddSqlServer("sqlserver")
        .WithDataVolume("ARSounds.Aspire.AppHost-sqlserver-data");

    visionResourceBuilder = resource.AddDatabase("vision");
    arsoundsResourceBuilder = resource.AddDatabase("arsounds");
}
else if (databaseProvider == "MySql")
{
    var resource = builder.AddMySql("mysql")
        .WithDataVolume("ARSounds.Aspire.AppHost-mysql-data");

    visionResourceBuilder = resource.AddDatabase("vision");
    arsoundsResourceBuilder = resource.AddDatabase("arsounds");
}
else
{
    var resource = builder.AddPostgres("postgresql")
        .WithDataVolume("ARSounds.Aspire.AppHost-postgresql-data");

    visionResourceBuilder = resource.AddDatabase("vision");
    arsoundsResourceBuilder = resource.AddDatabase("arsounds");
}

var databaseProviderParameter = builder.AddParameter("databaseProvider");

builder.AddProject<Projects.OpenVision_Client>("openvision-client");
builder.AddProject<Projects.OpenVision_Server>("openvision-server")
       .WithEnvironment("DatabaseProvider", databaseProviderParameter)
       .WithReference(visionResourceBuilder);

builder.AddProject<Projects.ARSounds_Server>("arsounds-server")
       .WithEnvironment("DatabaseProvider", databaseProviderParameter)
       .WithReference(arsoundsResourceBuilder);

builder.Build().Run();