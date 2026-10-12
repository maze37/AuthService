FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG GITHUB_USERNAME=maze37
WORKDIR /src
COPY ["Directory.Build.props", "Directory.Packages.props", "global.json", "nuget.config", "./"]
COPY ["backend/src/AuthService.Api/AuthService.Api.csproj", "backend/src/AuthService.Api/"]
COPY ["backend/src/AuthService.Domain/AuthService.Domain.csproj", "backend/src/AuthService.Domain/"]
COPY ["backend/src/AuthService.Application/AuthService.Application.csproj", "backend/src/AuthService.Application/"]
COPY ["backend/src/AuthService.Infrastructure/AuthService.Infrastructure.csproj", "backend/src/AuthService.Infrastructure/"]
RUN --mount=type=secret,id=github_token,required=true \
    env "NuGetPackageSourceCredentials_mazeland-private=Username=${GITHUB_USERNAME};Password=$(cat /run/secrets/github_token);ValidAuthenticationTypes=Basic" \
    dotnet restore "backend/src/AuthService.Api/AuthService.Api.csproj"
COPY . .
WORKDIR "/src/backend/src/AuthService.Api"
RUN dotnet build "./AuthService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AuthService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthService.Api.dll"]
