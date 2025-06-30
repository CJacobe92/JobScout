# 🧱 Base stage for runtime (Alpine + .NET ASP.NET Core)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
ARG APP_UID=1001
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true
USER $APP_UID
WORKDIR /app
EXPOSE 8081

# 🛠️ Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["src/JobScout.API/JobScout.API.csproj", "JobScout.API/"]
COPY ["src/JobScout.Core/JobScout.Core.csproj", "JobScout.Core/"]
COPY ["src/JobScout.Domain/JobScout.Domain.csproj", "JobScout.Domain/"]
COPY ["src/JobScout.Infrastructure/JobScout.Infrastructure.csproj", "JobScout.Infrastructure/"]

RUN dotnet restore "JobScout.API/JobScout.API.csproj"

# Copy source code
COPY src/ .

WORKDIR /src/JobScout.API
RUN dotnet build "JobScout.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 🧪 Publish stage
FROM build AS publish
RUN dotnet publish "JobScout.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 🚀 Final minimal image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JobScout.API.dll"]
