using DMCorp.Framework.Basics.DAL;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMCorp.Framework.StateMachine.DAL;

/// <summary>
/// Связь между текущм статусом и возможными действиями из этого статуса
/// (на одно и то же действие может ссылаться несколько сатусов, как и, очевидно, наоборот)
/// </summary>
[Table("StateMachineActionFromStates")]
public class DbStateMachineActionFromState : IEntityBase, IEntityWithDateCreated
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    public DateTimeOffset DateCreated { get; set; }

    public Guid StateMachineId { get; set; }

    /// <summary>
    /// Код права
    /// </summary>
    public virtual Guid? RightCode { get; set; }

    [Required]
    [ForeignKey(nameof(StateMachineAction))]
    public Guid StateMachineActionId { get; set; }

    public virtual DbStateMachineAction StateMachineAction { get; set; }

    [Required]
    [ForeignKey(nameof(StateMachineState))]
    public Guid StateMachineStateId { get; set; }

    public virtual DbStateMachineState StateMachineState { get; set; }
}