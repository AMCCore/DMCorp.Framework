using DMCorp.Framework.StateMachine.DAL;
using Microsoft.EntityFrameworkCore;

namespace DMCorp.Framework.StateMachine.Extensions;

public static class StateMachineContextExtension
{
    public static void OnModelCreatingStateMachineContext(this ModelBuilder source)
    {
        source.Entity<DbStateMachineActionFromState>().HasIndex(a => new { a.StateMachineActionId, a.StateMachineStateId }).IsUnique();
        source.Entity<DbStateMachineAction>().HasIndex(a => new { a.ActionCode }).IsUnique();
    }
}