FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/ARSounds.Web.Api/ARSounds.Web.Api.csproj", "src/ARSounds.Web.Api/"]
COPY ["src/ARSounds.Web.ServiceDefaults/ARSounds.Web.ServiceDefaults.csproj", "src/ARSounds.Web.ServiceDefaults/"]
COPY ["src/ARSounds.Web.Core/ARSounds.Web.Core.csproj", "src/ARSounds.Web.Core/"]
COPY ["src/ARSounds.Web.Api.Core/ARSounds.Web.Api.Core.csproj", "src/ARSounds.Web.Api.Core/"]
COPY ["src/ARSounds.Web.Api.EntityFramework.MySql/ARSounds.Web.Api.EntityFramework.MySql.csproj", "src/ARSounds.Web.Api.EntityFramework.MySql/"]
COPY ["src/ARSounds.Web.Api.EntityFramework.PostgreSQL/ARSounds.Web.Api.EntityFramework.PostgreSQL.csproj", "src/ARSounds.Web.Api.EntityFramework.PostgreSQL/"]
COPY ["src/ARSounds.Web.Api.EntityFramework.SqlServer/ARSounds.Web.Api.EntityFramework.SqlServer.csproj", "src/ARSounds.Web.Api.EntityFramework.SqlServer/"]
COPY ["src/ARSounds.Web.Api.EntityFramework/ARSounds.Web.Api.EntityFramework.csproj", "src/ARSounds.Web.Api.EntityFramework/"]

RUN dotnet restore "src/ARSounds.Web.Api/ARSounds.Web.Api.csproj"
COPY . .
WORKDIR "/src/src/ARSounds.Web.Api"
RUN dotnet build "ARSounds.Web.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ARSounds.Web.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
ENTRYPOINT ["dotnet", "ARSounds.Web.Api.dll"]