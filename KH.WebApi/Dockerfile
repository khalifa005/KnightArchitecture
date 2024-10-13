#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["KH.WebApi/KH.WebApi.csproj", "KH.WebApi/"]
COPY ["KH.Services/KH.Services.csproj", "KH.Services/"]
COPY ["KH.PersistenceInfra/KH.PersistenceInfra.csproj", "KH.PersistenceInfra/"]
COPY ["KH.Dto/KH.Dto.csproj", "KH.Dto/"]
COPY ["KH.Domain/KH.Domain.csproj", "KH.Domain/"]
COPY ["KH.BuildingBlocks/KH.BuildingBlocks.csproj", "KH.BuildingBlocks/"]
RUN dotnet restore "./KH.WebApi/KH.WebApi.csproj"
COPY . .
WORKDIR "/src/KH.WebApi"
RUN dotnet build "./KH.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./KH.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KH.WebApi.dll"]