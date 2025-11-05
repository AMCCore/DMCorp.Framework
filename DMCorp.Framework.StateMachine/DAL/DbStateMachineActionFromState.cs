using DMCorp.Framework.Basics.DAL;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMCorp.Framework.StateMachine.DAL;

/// <summary>
/// Связь между текущим статусом и возможными действиями из этого статуса.
/// На одно и то же действие может ссылаться несколько статусов, как и наоборот.
/// </summary>
[Table("StateMachineActionFromStates")]
public class DbStateMachineActionFromState : IEntityBase, IEntityWithDateCreated
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Метка времени последнего обновления в тиках
    /// </summary>
    public long LastUpdateTick { get; set; }

    /// <summary>
    /// Дата и время создания связи
    /// </summary>
    public DateTimeOffset DateCreated { get; set; }

    /// <summary>
    /// Идентификатор машины состояний
    /// </summary>
    public Guid StateMachineId { get; set; }

    /// <summary>
    /// Код права доступа, необходимого для выполнения действия
    /// </summary>
    public virtual Guid? RightCode { get; set; }

    /// <summary>
    /// Идентификатор действия
    /// </summary>
    [Required]
    [ForeignKey(nameof(StateMachineAction))]
    public Guid StateMachineActionId { get; set; }

    /// <summary>
    /// Действие машины состояний
    /// </summary>
    public virtual required DbStateMachineAction StateMachineAction { get; set; }

    /// <summary>
    /// Идентификатор статуса, из которого возможно выполнение действия
    /// </summary>
    [Required]
    [ForeignKey(nameof(StateMachineState))]
    public Guid StateMachineStateId { get; set; }

    /// <summary>
    /// Статус машины состояний, из которого возможно выполнение действия
    /// </summary>
    public virtual required DbStateMachineState StateMachineState { get; set; }
}