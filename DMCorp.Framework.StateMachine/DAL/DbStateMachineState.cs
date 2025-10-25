using DMCorp.Framework.Basics.DAL;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMCorp.Framework.StateMachine.DAL;

/// <summary>
/// Текущий статус объекта
/// </summary>
[Table("StateMachineStates")]
public class DbStateMachineState : IEntityBase, ISoftDeleteEntity, IEntityWithDateCreated
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset DateCreated { get; set; }
    
    public Guid StateMachineId { get; set; }

    [Required]
    public required string Name { get; set; }

    public string? Description { get; set; }

    [InverseProperty(nameof(DbStateMachineAction.ToState))]
    public virtual ICollection<DbStateMachineAction> FromActions { get; set; } = new List<DbStateMachineAction>();

    public virtual ICollection<DbStateMachineActionFromState> Actions { get; set; } = new List<DbStateMachineActionFromState>();
}