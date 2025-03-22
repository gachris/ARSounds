using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);
var databaseProvider = builder.Configuration.GetSection("Parameters").GetValue<string>("databaseProvider")!;

IResourceBuilder<IResourceWithConnectionString> visionResourceBuilder;
IResourceBuilder<IResourceWithConnectionString> arsoundsResourceBuilder;

if (databaseProvider == "SqlServer")
{
    var resource = builder.AddSqlServer("sqlserver")
        .WithDataVolume();

    visionResourceBuilder = resource.AddDatabase("vision");
    arsoundsResourceBuilder = resource.AddDatabase("arsounds");

}
else if (databaseProvider == "MySql")
{
    var resource = builder.AddMySql("mysql")
        .WithDataVolume();

    visionResourceBuilder = resource.AddDatabase("vision");
    arsoundsResourceBuilder = resource.AddDatabase("arsounds");
}
else
{
    var resource = builder.AddPostgres("postgres")
        .WithDataVolume();

    visionResourceBuilder = resource.AddDatabase("vision");
    arsoundsResourceBuilder = resource.AddDatabase("arsounds");
}

var databaseProviderParameter = builder.AddParameter("databaseProvider");

builder.AddProject<Projects.OpenVision_Client>("openvision-client");
builder.AddProject<Projects.OpenVision_Server>("openvision-server")
       .WithEnvironment("DatabaseProvider", databaseProviderParameter)
       .WithReference(visionResourceBuilder);

builder.AddProject<Projects.ARSounds_Web_App_Server>("arsounds-web-app-server");
builder.AddProject<Projects.ARSounds_Web_Api>("arsounds-api")
       .WithEnvironment("DatabaseProvider", databaseProviderParameter)
       .WithReference(arsoundsResourceBuilder);

builder.Build().Run();