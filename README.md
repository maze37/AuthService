# AuthService

Минимальная заготовка ASP.NET Core API на .NET 10:

- Application — регистрация обработчиков команд и запросов через Scrutor.
- Infrastructure — EF Core, PostgreSQL и проверка доступности базы.
- Api — DI, Swagger и Serilog: запуск, ошибки и HTTP-запросы в консоль.

## Локальный запуск

Из корня проекта:

```sh
cp .env.example .env
docker compose up -d postgres
dotnet build AuthService.slnx
ASPNETCORE_ENVIRONMENT=Development \
ASPNETCORE_URLS=http://localhost:8080 \
ConnectionStrings__AuthDatabase='Host=localhost;Port=15435;Database=auth_service_db;Username=postgres;Password=1234' \
dotnet run --project backend/src/AuthService.Api --no-build
```

Если `.env` уже существует, добавьте в него переменные из `.env.example`.
При изменении параметров PostgreSQL обновите строку подключения в команде запуска.
Для восстановления пакетов Mazeland нужен доступ к приватному NuGet-источнику,
как в FileService.

Swagger: http://localhost:8080/swagger (в Development).
Проверка базы: http://localhost:8080/health — HTTP 200 при доступной базе,
HTTP 503 при недоступной. Контроллеров и моделей пока нет.

## Запуск API в Docker

Для приватных пакетов задайте логин GitHub и токен в текущем терминале (zsh).
Токен вводится скрыто и передаётся в сборку через BuildKit secret:

```sh
export GITHUB_USERNAME=maze37
read -rs 'GITHUB_TOKEN?GitHub token: '
echo
export GITHUB_TOKEN
```

Используйте personal access token (classic) с `read:packages` и доступом к пакетам Mazeland.

```sh
docker compose up --build -d
```

После сборки можно удалить токен из окружения: `unset GITHUB_TOKEN`.

API доступен на порту `HTTP_PORT` из `.env` (по умолчанию 8080).
Для сборки образа доступ к приватным пакетам нужен также внутри Docker.
Миграции добавляются после появления моделей; автоматически они пока не запускаются.
# AuthService
