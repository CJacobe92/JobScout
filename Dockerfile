# Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution-level props file
COPY Directory.Packages.props ./

# Copy project files
COPY src/Platform/Api/Api.csproj ./src/Platform/Api/
COPY src/Platform/Application/Application.csproj ./src/Platform/Application/
COPY src/Platform/Domain/Domain.csproj ./src/Platform/Domain/
COPY src/Platform/Infrastructure/Infrastructure.csproj ./src/Platform/Infrastructure/
COPY src/Platform/Shared/Shared.csproj ./src/Platform/Shared/

# Restore dependencies
RUN dotnet restore ./src/Platform/Api/Api.csproj

# Copy the rest of the source
COPY . .

# Build the application
RUN dotnet build ./src/Platform/Api/Api.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish ./src/Platform/Api/Api.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
