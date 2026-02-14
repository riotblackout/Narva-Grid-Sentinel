# STAGE 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["NarvaSentinel.csproj", "./"]
RUN dotnet restore "NarvaSentinel.csproj"

# Copy everything else and build
COPY . .
RUN dotnet publish "NarvaSentinel.csproj" -c Release -o /app/publish /p:UseAppHost=false

# STAGE 2: Runtime (The "Production" Image)
FROM mcr.microsoft.com/dotnet/runtime:8.0-alpine AS final
WORKDIR /app
COPY --from=build /app/publish .

# Run as non-root user (Security Best Practice)
USER app
ENTRYPOINT ["dotnet", "NarvaSentinel.dll"]