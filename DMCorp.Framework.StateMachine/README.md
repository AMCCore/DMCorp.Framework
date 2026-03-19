# DMCorp.Framework.StateMachine

Пакет для описания **машины состояний** в базе данных через **Entity Framework Core**: состояния, действия (переходы), связи «из какого состояния доступно какое действие» и фильтрация переходов по правам пользователя.

## Назначение

- **Сущности DAL** — `DbStateMachineState`, `DbStateMachineAction`, `DbStateMachineActionFromState` и связанные таблицы для моделирования автомата с привязкой к `StateMachineId`.
- **Конфигурация модели** — `StateMachineContextExtension.OnModelCreatingStateMachineContext` задаёт уникальные индексы для EF.
- **Логика переходов** — `GetNextStatesExtension.GetNextStates` возвращает доступные из текущего состояния действия с учётом пользовательских (не системных) действий и опциональной коллекции прав (`RightCode`).

Зависит от **DMCorp.Framework.Basics** (интерфейсы сущностей, мягкое удаление и т.д.).

## Зависимости

Косвенно через **Basics**: EF Core. Целевая платформа: **net9.0**.