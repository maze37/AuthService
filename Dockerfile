FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8003

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG GITHUB_USERNAME=maze37
WORKDIR /src
COPY ["Directory.Build.props", "Directory.Packages.props", "global.json", "nuget.config", "./"]
COPY ["src/AuthService.Web/AuthService.Web.csproj", "src/AuthService.Web/"]
COPY ["src/AuthService.Domain/AuthService.Domain.csproj", "src/AuthService.Domain/"]
COPY ["src/AuthService.Application/AuthService.Application.csproj", "src/AuthService.Application/"]
COPY ["src/AuthService.Infrastructure/AuthService.Infrastructure.csproj", "src/AuthService.Infrastructure/"]

RUN --mount=type=secret,id=github_token,required=true \
    env "NuGetPackageSourceCredentials_mazeland-private=Username=${GITHUB_USERNAME};Password=$(cat /run/secrets/github_token);ValidAuthenticationTypes=Basic" \
    dotnet restore "src/AuthService.Web/AuthService.Web.csproj"
COPY . .
WORKDIR "/src/src/AuthService.Web"
RUN dotnet build "./AuthService.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AuthService.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthService.Web.dll"]
