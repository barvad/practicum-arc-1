FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy project files
COPY . .

# Restore dependencies
RUN dotnet restore "./SmartHome.DeviceRegistryService.API/SmartHome.DeviceRegistryService.API.csproj"




# Build application
WORKDIR "/source/SmartHome.DeviceRegistryService.API"
RUN dotnet build "SmartHome.DeviceRegistryService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartHome.DeviceRegistryService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Install curl for health checks
RUN apt-get update && \
    apt-get install -y curl && \
    rm -rf /var/lib/apt/lists/*

# Create health check endpoint
RUN echo 'using Microsoft.AspNetCore.Builder; using Microsoft.AspNetCore.Http; var app = WebApplication.Create(); app.MapGet("/health", () => Results.Ok("Healthy")); app.Run("http://*:5000");' > HealthCheck.cs && \
    dotnet run HealthCheck.cs &

ENTRYPOINT ["dotnet", "SmartHome.DeviceRegistryService.API.dll"]