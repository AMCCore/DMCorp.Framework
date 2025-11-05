using DMCorp.Framework.StateMachine.DAL;

namespace DMCorp.Framework.StateMachine.Extensions;

/// <summary>
/// Расширения для получения доступных действий из статуса машины состояний
/// </summary>
public static class GetNextStatesExtension
{
    /// <summary>
    /// Получает список доступных действий из указанного статуса с учетом прав доступа
    /// </summary>
    /// <param name="source">Исходный статус машины состояний</param>
    /// <param name="Rights">Коллекция идентификаторов прав доступа пользователя. Если null или пустая, возвращаются только действия без требования прав</param>
    /// <returns>Запрос действий, доступных из указанного статуса</returns>
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