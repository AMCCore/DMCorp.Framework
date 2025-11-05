using DMCorp.Framework.Basics.DAL;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMCorp.Framework.StateMachine.DAL;

/// <summary>
/// Статус объекта в машине состояний
/// </summary>
[Table("StateMachineStates")]
public class DbStateMachineState : IEntityBase, ISoftDeleteEntity, IEntityWithDateCreated
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
    /// Признак удаления статуса (мягкое удаление)
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Дата и время создания статуса
    /// </summary>
    public DateTimeOffset DateCreated { get; set; }
    
    /// <summary>
    /// Идентификатор машины состояний
    /// </summary>
    public Guid StateMachineId { get; set; }

    /// <summary>
    /// Название статуса
    /// </summary>
    [Required]
    public required string Name { get; set; }

    /// <summary>
    /// Описание статуса
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Коллекция действий, которые переводят объект в данный статус
    /// </summary>
    [InverseProperty(nameof(DbStateMachineAction.ToState))]
    public virtual ICollection<DbStateMachineAction> FromActions { get; set; } = new List<DbStateMachineAction>();

    /// <summary>
    /// Коллекция связей с действиями, которые доступны из данного статуса
    /// </summary>
    public virtual ICollection<DbStateMachineActionFromState> Actions { get; set; } = new List<DbStateMachineActionFromState>();
}