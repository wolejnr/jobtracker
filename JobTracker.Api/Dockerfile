# 1. Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["JobTracker.Api.csproj", "./"]
RUN dotnet restore "JobTracker.Api.csproj"

# Copy the remaining files and build the project
COPY . .
RUN dotnet build "JobTracker.Api.csproj" -c Release -o /app/build

# 2. Publish the application
FROM build AS publish
RUN dotnet publish "JobTracker.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 3. Use the ASP.NET runtime image for the final production container
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Render dynamically assigns a port, so bind .NET to all interfaces
# ENV ASPNETCORE_URLS=http://+:10000
# EXPOSE 10000
ENV ASPNETCORE_HTTP_PORTS=${PORT}

ENTRYPOINT ["dotnet", "JobTracker.Api.dll"]
