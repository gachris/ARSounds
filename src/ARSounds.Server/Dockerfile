# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 443

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS with-node
RUN apt-get update
RUN apt-get install curl
RUN curl -sL https://deb.nodesource.com/setup_20.x | bash
RUN apt-get -y install nodejs
RUN npm install -g @angular/cli

FROM with-node AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY Directory.*.props ./
COPY Directory.Build.targets ./
COPY ["src/ARSounds.Server/ARSounds.Server.csproj", "src/ARSounds.Server/"]
COPY ["src/arsounds.client/arsounds.client.esproj", "src/arsounds.client/"]
COPY ["src/ARSounds.Server.Core/ARSounds.Server.Core.csproj", "src/ARSounds.Server.Core/"]
COPY ["src/ARSounds.EntityFramework/ARSounds.EntityFramework.csproj", "src/ARSounds.EntityFramework/"]
COPY ["src/ARSounds.EntityFramework.MySql/ARSounds.EntityFramework.MySql.csproj", "src/ARSounds.EntityFramework.MySql/"]
COPY ["src/ARSounds.EntityFramework.PostgreSQL/ARSounds.EntityFramework.PostgreSQL.csproj", "src/ARSounds.EntityFramework.PostgreSQL/"]
COPY ["src/ARSounds.EntityFramework.SqlServer/ARSounds.EntityFramework.SqlServer.csproj", "src/ARSounds.EntityFramework.SqlServer/"]
COPY ["src/OpenVision.ServiceDefaults/OpenVision.ServiceDefaults.csproj", "src/OpenVision.ServiceDefaults/"]
RUN dotnet restore "src/ARSounds.Server/ARSounds.Server.csproj"
COPY . .
WORKDIR "/src/src/ARSounds.Server"
RUN dotnet build "ARSounds.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ARSounds.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
ENTRYPOINT ["dotnet", "ARSounds.Server.dll"]
