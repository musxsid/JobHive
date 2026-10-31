# Multi-Stage Dockerfile for JobHive (Cross-Platform .NET 8)
# Supports Linux containers running on Windows, macOS, and Linux

# Stage 1: Base Runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage 2: SDK Build Environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files first for optimal NuGet package caching
COPY ["JobHive.Models/JobHive.Models.csproj", "JobHive.Models/"]
COPY ["JobHive.DataAccess/JobHive.DataAccess.csproj", "JobHive.DataAccess/"]
COPY ["JobHive.Utility/JobHive.Utility.csproj", "JobHive.Utility/"]
COPY ["JobHive.Web/JobHive.Web.csproj", "JobHive.Web/"]

# Restore packages
RUN dotnet restore "JobHive.Web/JobHive.Web.csproj"

# Copy the rest of the application source code
COPY . .
WORKDIR "/src/JobHive.Web"
RUN dotnet build "JobHive.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish App Binaries
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "JobHive.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Production Runtime Image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set up data & uploads directories with correct write permissions
RUN mkdir -p /app/wwwroot/uploads/resumes /app/data

# Environment Defaults
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "JobHive.Web.dll"]

