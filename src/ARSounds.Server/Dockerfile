# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 443


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS with-node
RUN apt-get update
RUN apt-get install curl
RUN curl -sL https://deb.nodesource.com/setup_20.x | bash
RUN apt-get -y install nodejs
RUN npm install -g @angular/cli

FROM with-node AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ARSounds.Server/ARSounds.Server.csproj", "ARSounds.Server/"]
COPY ["src/arsounds.client/arsounds.client.esproj", "arsounds.client/"]
COPY ["src/ARSounds.Server.Core/ARSounds.Server.Core.csproj", "src/ARSounds.Server.Core/"]
COPY ["src/ARSounds.Server.EntityFramework/ARSounds.Server.EntityFramework.csproj", "src/ARSounds.Server.EntityFramework/"]
COPY ["src/ARSounds.Server.EntityFramework.MySql/ARSounds.Server.EntityFramework.MySql.csproj", "src/ARSounds.Server.EntityFramework.MySql/"]
COPY ["src/ARSounds.Server.EntityFramework.PostgreSQL/ARSounds.Server.EntityFramework.PostgreSQL.csproj", "src/ARSounds.Server.EntityFramework.PostgreSQL/"]
COPY ["src/ARSounds.Server.EntityFramework.SqlServer/ARSounds.Server.EntityFramework.SqlServer.csproj", "src/ARSounds.Server.EntityFramework.SqlServer/"]
COPY ["src/ARSounds.ServiceDefaults/ARSounds.ServiceDefaults.csproj", "src/ARSounds.ServiceDefaults/"]
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
