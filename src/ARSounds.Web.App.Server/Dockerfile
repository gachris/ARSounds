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
COPY ["src/ARSounds.Web.App.Server/ARSounds.Web.App.Server.csproj", "ARSounds.Web.App.Server/"]
COPY ["src/arsounds.web.app.client/arsounds.web.app.client.esproj", "arsounds.web.app.client/"]
COPY ["src/ARSounds.Web.ServiceDefaults/ARSounds.Web.ServiceDefaults.csproj", "src/ARSounds.Web.ServiceDefaults/"]
COPY ["src/ARSounds.Web.Core/ARSounds.Web.Core.csproj", "src/ARSounds.Web.Core/"]
COPY ["src/ARSounds.Web.Api.Core/ARSounds.Web.Api.Core.csproj", "src/ARSounds.Web.Api.Core/"]
COPY ["src/ARSounds.Web.Api.EntityFramework.MySql/ARSounds.Web.Api.EntityFramework.MySql.csproj", "src/ARSounds.Web.Api.EntityFramework.MySql/"]
COPY ["src/ARSounds.Web.Api.EntityFramework.PostgreSQL/ARSounds.Web.Api.EntityFramework.PostgreSQL.csproj", "src/ARSounds.Web.Api.EntityFramework.PostgreSQL/"]
COPY ["src/ARSounds.Web.Api.EntityFramework.SqlServer/ARSounds.Web.Api.EntityFramework.SqlServer.csproj", "src/ARSounds.Web.Api.EntityFramework.SqlServer/"]
COPY ["src/ARSounds.Web.Api.EntityFramework/ARSounds.Web.Api.EntityFramework.csproj", "src/ARSounds.Web.Api.EntityFramework/"]
RUN dotnet restore "src/ARSounds.Web.App.Server/ARSounds.Web.App.Server.csproj"
COPY . .
WORKDIR "/src/src/ARSounds.Web.App.Server"
RUN dotnet build "ARSounds.Web.App.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ARSounds.Web.App.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
ENTRYPOINT ["dotnet", "ARSounds.Web.App.Server.dll"]
