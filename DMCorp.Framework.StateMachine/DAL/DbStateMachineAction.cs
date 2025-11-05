using DMCorp.Framework.Basics.DAL;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMCorp.Framework.StateMachine.DAL;

/// <summary>
/// Действия, которые переводят статус в другой статус.
/// Для перевода в другой статус возможны несколько разных действий с разными названиями
/// и идентификаторами (например, для разных прав пользователя, или для разных промежуточных
/// действий, что дифференцируется разными идентификаторами действий)
/// </summary>
[Table("StateMachineActions")]
public class DbStateMachineAction : IEntityBase, ISoftDeleteEntity, IEntityWithDateCreated
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
    /// Признак удаления действия (мягкое удаление)
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Дата и время создания действия
    /// </summary>
    public DateTimeOffset DateCreated { get; set; }

    /// <summary>
    /// Идентификатор машины состояний
    /// </summary>
    public Guid StateMachineId { get; set; }

    /// <summary>
    /// Название действия
    /// </summary>
    [Required]
    public required string ActionName { get; set; }

    /// <summary>
    /// Описание действия
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Текст подтверждения для действия
    /// </summary>
    public string? ConfirmText { get; set; }

    /// <summary>
    /// Код действия (уникальный идентификатор)
    /// </summary>
    [Required]
    public required string ActionCode { get; set; }

    /// <summary>
    /// Признак системного действия (не отображается пользователю)
    /// </summary>
    public bool IsSystemAction { get; set; } = false;

    /// <summary>
    /// Идентификатор целевого статуса, в который переводит действие
    /// </summary>
    [ForeignKey(nameof(ToState))]
    public Guid? ToStateId { get; set; }

    /// <summary>
    /// Целевой статус, в который переводит действие
    /// </summary>
    public virtual DbStateMachineState? ToState { get; set; }

    /// <summary>
    /// Порядок сортировки действия
    /// </summary>
    public int SortingOrder { get; set; } = 0;

    /// <summary>
    /// Коллекция связей с исходными статусами, из которых возможно выполнение данного действия
    /// </summary>
    public virtual ICollection<DbStateMachineActionFromState> FromState { get; set; } = new List<DbStateMachineActionFromState>();
}