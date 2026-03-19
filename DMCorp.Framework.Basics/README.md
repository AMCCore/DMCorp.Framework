# DMCorp.Framework.Basics

Базовый пакет **DMCorp.Framework**: общие контракты и вспомогательный код для сервисов на **Entity Framework Core** и **ASP.NET Core**.

## Назначение

- **DAL** — интерфейсы сущностей (`IEntityBase`, мягкое удаление, метки времени), контракт `IUnitOfWork` и реализация `BaseUnitOfWork` с транзакциями и обновлением `LastUpdateTick`.
- **Расширения** — работа с репозиториями и контекстом EF, даты/время, перечисления с GUID (`EnumGuidAttribute`, конвертеры).
- **Безопасность** — абстракции валидации и выдачи токенов (`ITokenValidator`, `BasicTokenValidator`, `ITokenService`, сервисы безопасности).
- **Настройки** — базовые классы конфигурации приложения и почты.
- **Почта** — DTO и контракт `IEmailService` для отправки писем.
- **Утилиты** — например генератор коротких строк.

## Зависимости

- `Microsoft.EntityFrameworkCore`
- `Microsoft.Extensions.Caching.Abstractions`
- `Microsoft.IdentityModel.Tokens`

Целевая платформа: **net9.0**.