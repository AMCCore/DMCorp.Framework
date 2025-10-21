using DMCorp.Framework.StateMachine.DAL;

namespace DMCorp.Framework.StateMachine.Extensions;

public static class GetNextStatesExtension
{
    public static IQueryable<DbStateMachineAction> GetNextStates(this DbStateMachineState source, ICollection<Guid>? Rights = null)
    {

        var q = source.Actions
            //Проверка что действие пользовательское, а не системное
            .Where(x => !x.StateMachineAction.IsSystemAction)
            //Все сущьности принадлежать одной машине
            .Where(x => x.StateMachineId == source.StateMachineId && x.StateMachineAction.StateMachineId == source.StateMachineId)
            .AsQueryable();

        //Проверка прав
        if (Rights?.Count > 0)
        {
            q = q.Where(x => x.RightCode == null || Rights.Any(y => y == x.RightCode));
        }
        else
        {
            q = q.Where(x => x.RightCode == null);
        }

        return q.Select(x => x.StateMachineAction).DistinctBy(x => x.Id).AsQueryable();
    }
}