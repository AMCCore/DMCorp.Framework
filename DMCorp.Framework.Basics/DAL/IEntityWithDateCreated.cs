namespace DMCorp.Framework.Basics.DAL;

/// <summary>
/// Интерфейс для сущностей, имеющих дату создания
/// </summary>
public interface IEntityWithDateCreated : IEntityBase
{
    /// <summary>
    /// Дата и время создания сущности
    /// </summary>
    DateTimeOffset DateCreated { get; set; }
}