namespace DMCorp.Framework.Basics.DAL;

/// <summary>
/// Интерфейс для сущностей с поддержкой мягкого удаления
/// </summary>
public interface ISoftDeleteEntity : IEntityBase
{
    /// <summary>
    /// Признак удаления сущности (мягкое удаление)
    /// </summary>
    bool IsDeleted { get; set; }
}
