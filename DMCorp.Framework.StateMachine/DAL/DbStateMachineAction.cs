using DMCorp.Framework.Basics.DAL;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMCorp.Framework.StateMachine.DAL;

/// <summary>
/// Действия которые переводят статус
/// Для перевода в другой статус возможны несколько разных действий с разными названиями
/// и идентификаторами (например для разных прав пользователя, или для разных промежуточнх
/// действий что дифференцируется разными идентификаторми действий)
/// </summary>
[Table("StateMachineActions")]
public class DbStateMachineAction : IEntityBase, ISoftDeleteEntity, IEntityWithDateCreated
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset DateCreated { get; set; }

    public Guid StateMachineId { get; set; }

    [Required]
    public required string ActionName { get; set; }

    public string? Description { get; set; }

    public string? ConfirmText { get; set; }

    [Required]
    public required string ActionCode { get; set; }

    public bool IsSystemAction { get; set; } = false;

    [ForeignKey(nameof(ToState))]
    public Guid? ToStateId { get; set; }

    public virtual DbStateMachineState? ToState { get; set; }

    public int SortingOrder { get; set; } = 0;

    public virtual ICollection<DbStateMachineActionFromState> FromState { get; set; } = new List<DbStateMachineActionFromState>();
}