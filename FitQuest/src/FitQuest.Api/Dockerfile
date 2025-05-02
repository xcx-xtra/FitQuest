# Base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["FitQuest.Api/FitQuest.Api.csproj", "FitQuest.Api/"]
RUN dotnet restore "FitQuest.Api/FitQuest.Api.csproj"
COPY . .
WORKDIR "/src/FitQuest.Api"
RUN dotnet build "FitQuest.Api.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "FitQuest.Api.csproj" -c Release -o /app/publish

# Runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FitQuest.Api.dll"]