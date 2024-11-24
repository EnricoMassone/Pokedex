# Use the .NET SDK for the build stage 
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build 
WORKDIR /source

# Copy all .csproj files from the src directory and its subdirectories 
COPY src/Pokedex.Api/Pokedex.Api.csproj ./src/Pokedex.Api/
COPY src/Pokedex.Application/Pokedex.Application.csproj ./src/Pokedex.Application/
COPY src/Pokedex.Domain/Pokedex.Domain.csproj ./src/Pokedex.Domain/
COPY src/Pokedex.Infrastructure/Pokedex.Infrastructure.csproj ./src/Pokedex.Infrastructure/
COPY src/Pokedex.Framework/Pokedex.Framework.csproj ./src/Pokedex.Framework/

# Restore the application 
RUN dotnet restore ./src/Pokedex.Api/Pokedex.Api.csproj

# Copy all the other files and build the application
COPY src/. ./src/
WORKDIR /source/src/Pokedex.Api/
RUN dotnet publish -c release -o /app --no-restore

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Pokedex.Api.dll"]