FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/OpenVision.Server/OpenVision.Server.csproj", "src/OpenVision.Server/"]
COPY ["src/ARSounds.ServiceDefaults/ARSounds.ServiceDefaults.csproj", "src/ARSounds.ServiceDefaults/"]

RUN dotnet restore "src/OpenVision.Server/OpenVision.Server.csproj"
COPY . .
WORKDIR "/src/src/OpenVision.Server"
RUN dotnet build "OpenVision.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenVision.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
ENTRYPOINT ["dotnet", "OpenVision.Server.dll"]