using DMCorp.Framework.StateMachine.DAL;
using Microsoft.EntityFrameworkCore;

namespace DMCorp.Framework.StateMachine.Extensions;

/// <summary>
/// Расширения для настройки модели Entity Framework для машины состояний
/// </summary>
public static class StateMachineContextExtension
{
    /// <summary>
    /// Настраивает модель Entity Framework для работы с машиной состояний: создает уникальные индексы
    /// </summary>
    /// <param name="source">Построитель модели Entity Framework</param>
    public static void OnModelCreatingStateMachineContext(this ModelBuilder source)
    {
        source.Entity<DbStateMachineActionFromState>().HasIndex(a => new { a.StateMachineActionId, a.StateMachineStateId }).IsUnique();
        source.Entity<DbStateMachineAction>().HasIndex(a => new { a.ActionCode }).IsUnique();
    }
}