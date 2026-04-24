# DMCorp.Framework

Набор библиотек **.NET 9** для внутренних сервисов DMCorp: общая инфраструктура данных и безопасности, интеграция с **Kubernetes** и модель **машины состояний** для Entity Framework.

**Репозиторий:** [github.com/AMCCore/DMCorp.Framework](https://github.com/AMCCore/DMCorp.Framework)

## Состав решения

| Проект | NuGet-пакет | Назначение |
|--------|-------------|------------|
| [DMCorp.Framework.Basics](DMCorp.Framework.Basics/README.md) | `DMCorp.Framework.Basics` | Базовые абстракции DAL, Unit of Work, расширения EF/репозиториев, токены, настройки, утилиты |
| [DMCorp.Framework.StateMachine](DMCorp.Framework.StateMachine/README.md) | `DMCorp.Framework.StateMachine` | Сущности и расширения EF для хранения машины состояний и переходов с учётом прав |
| [DMCorp.Framework.K8s](DMCorp.Framework.K8s/README.md) | `DMCorp.Framework.K8s` | Клиент Kubernetes, JWT для Service Account, JWKS и обработчики обратного канала |

## Версия

Текущая версия пакетов в проектах: **1.1.4**
