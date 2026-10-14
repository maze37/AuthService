# AuthService

Заготовка ASP.NET Core API на .NET 10. Проекты и настройки находятся в `backend`.

## Сборка

Из корня репозитория:

```sh
dotnet restore backend/AuthService.slnx
dotnet build backend/AuthService.slnx --no-restore
```

Пакеты `Mazeland.*` доступны в приватном NuGet-источнике GitHub Packages.
Для локального restore нужна авторизация этого источника в NuGet.

## Запуск через Docker

```zsh
cd backend
cp -n .env.example .env
export GITHUB_USERNAME=maze37
read -rs 'GITHUB_TOKEN?GitHub token: '
echo
export GITHUB_TOKEN
docker compose up -d --build
unset GITHUB_TOKEN
```

Используйте GitHub personal access token (classic) с `read:packages` и доступом
к пакетам. Токен передаётся в restore через BuildKit secret.

Swagger: http://localhost:8080/swagger (Development).
Проверка базы: http://localhost:8080/health.
Seq: http://localhost:8083; приём логов с хоста — http://localhost:5342.
Порт API задаётся переменной `HTTP_PORT` в `.env`.

## Локальный запуск API

```sh
cd backend
docker compose up -d postgres seq
dotnet run --project src/AuthService.Web
```

Настройки Development рассчитаны на PostgreSQL `localhost:15435` и Seq
`localhost:5342`. Если изменены реквизиты БД в `.env`, задайте соответствующую
строку подключения через `ConnectionStrings__AuthDatabase`.

Контроллеров и моделей пока нет: это инфраструктурная заготовка сервиса.
