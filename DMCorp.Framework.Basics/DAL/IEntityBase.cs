using System.ComponentModel.DataAnnotations;

namespace DMCorp.Framework.Basics.DAL;

/// <summary>
/// Базовый интерфейс для всех сущностей в системе
/// </summary>
public interface IEntityBase
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    [Key]
    Guid Id { get; set; }

    /// <summary>
    /// Метка времени последнего обновления в тиках
    /// </summary>
    long LastUpdateTick { get; set; }
}