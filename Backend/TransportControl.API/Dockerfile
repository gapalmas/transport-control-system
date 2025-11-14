# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["TransportControl.API/TransportControl.API.csproj", "TransportControl.API/"]
COPY ["TransportControl.Core/TransportControl.Core.csproj", "TransportControl.Core/"]
COPY ["TransportControl.Infrastructure/TransportControl.Infrastructure.csproj", "TransportControl.Infrastructure/"]
RUN dotnet restore "TransportControl.API/TransportControl.API.csproj"

# Copy all source code and build
COPY . .
WORKDIR "/src/TransportControl.API"
RUN dotnet build "TransportControl.API.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "TransportControl.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TransportControl.API.dll"]
