using Microsoft.EntityFrameworkCore;

namespace DMCorp.Framework.Basics.DAL;

/// <summary>
/// Интерфейс единицы работы (Unit of Work) для управления транзакциями и операциями с базой данных
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Флаг, указывающий, что не нужно изменять LastUpdateTick при сохранении
    /// </summary>
    bool NotChangeLastUpdateTick { get; set; }

    /// <summary>
    /// Контекст базы данных Entity Framework
    /// </summary>
    DbContext Context { get; }

    //IDbContextTransaction? Transaction { get; }

    /// <summary>
    /// Отключает автоматическое отслеживание изменений в контексте
    /// </summary>
    void AutoDetectChangesDisable();

    /// <summary>
    /// Включает автоматическое отслеживание изменений в контексте
    /// </summary>
    void AutoDetectChangesEnable();

    /// <summary>
    /// Сохраняет все изменения в базе данных
    /// </summary>
    /// <param name="hardDelete">Если true, выполняется физическое удаление вместо мягкого</param>
    void SaveChanges(bool hardDelete = false);

    /// <summary>
    /// Асинхронно сохраняет все изменения в базе данных
    /// </summary>
    /// <param name="hardDelete">Если true, выполняется физическое удаление вместо мягкого</param>
    /// <param name="token">Токен отмены операции</param>
    Task SaveChangesAsync(bool hardDelete = false, CancellationToken token = default);

    /// <summary>
    /// Получает набор сущностей указанного типа
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующий IEntityBase</typeparam>
    /// <returns>Набор сущностей указанного типа</returns>
    DbSet<T> GetSet<T>() where T : class, IEntityBase;

    /// <summary>
    /// Удаляет сущность из базы данных
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующий IEntityBase</typeparam>
    /// <param name="entity">Сущность для удаления</param>
    /// <param name="hardDelete">Если true, выполняется физическое удаление вместо мягкого</param>
    /// <param name="saveChanges">Если true, изменения сохраняются сразу</param>
    void Delete<T>(T entity, bool hardDelete = false, bool saveChanges = true) where T : class, IEntityBase;

    /// <summary>
    /// Асинхронно удаляет сущность из базы данных
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующий IEntityBase</typeparam>
    /// <param name="entity">Сущность для удаления</param>
    /// <param name="hardDelete">Если true, выполняется физическое удаление вместо мягкого</param>
    /// <param name="saveChanges">Если true, изменения сохраняются сразу</param>
    /// <param name="token">Токен отмены операции</param>
    Task DeleteAsync<T>(T entity, bool hardDelete = false, bool saveChanges = true, CancellationToken token = default) where T : class, IEntityBase;

    /// <summary>
    /// Удаляет список сущностей из базы данных
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующий IEntityBase</typeparam>
    /// <param name="entities">Список сущностей для удаления</param>
    /// <param name="hardDelete">Если true, выполняется физическое удаление вместо мягкого</param>
    /// <param name="saveChanges">Если true, изменения сохраняются сразу</param>
    void DeleteList<T>(IEnumerable<T> entities, bool hardDelete = false, bool saveChanges = true) where T : class, IEntityBase;

    /// <summary>
    /// Асинхронно удаляет список сущностей из базы данных
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующий IEntityBase</typeparam>
    /// <param name="entities">Список сущностей для удаления</param>
    /// <param name="hardDelete">Если true, выполняется физическое удаление вместо мягкого</param>
    /// <param name="saveChanges">Если true, изменения сохраняются сразу</param>
    /// <param name="token">Токен отмены операции</param>
    Task DeleteListAsync<T>(IEnumerable<T> entities, bool hardDelete = false, bool saveChanges = true, CancellationToken token = default) where T : class, IEntityBase;

    /// <summary>
    /// Добавляет новую сущность в базу данных
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующий IEntityBase</typeparam>
    /// <param name="entity">Сущность для добавления</param>
    /// <param name="saveChanges">Если true, изменения сохраняются сразу</param>
    /// <returns>Добавленная сущность</returns>
    T AddEntity<T>(T entity, bool saveChanges = true) where T : class, IEntityBase;

    /// <summary>
    /// Асинхронно добавляет новую сущность в базу данных
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующий IEntityBase</typeparam>
    /// <param name="entity">Сущность для добавления</param>
    /// <param name="saveChanges">Если true, изменения сохраняются сразу</param>
    /// <param name="token">Токен отмены операции</param>
    /// <returns>Добавленная сущность</returns>
    Task<T> AddEntityAsync<T>(T entity, bool saveChanges = true, CancellationToken token = default) where T : class, IEntityBase;

    /// <summary>
    /// Получает запрос для работы с сущностями указанного типа
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующий IEntityBase</typeparam>
    /// <param name="withDeleted">Если true, включаются удаленные сущности (для мягкого удаления)</param>
    /// <returns>Запрос для работы с сущностями</returns>
    IQueryable<T> Query<T>(bool withDeleted = false) where T : class, IEntityBase;

    //T Update<T>(T entity) where T : class, IEntityBase;

    //Task<T> UpdateAsync<T>(T entity, CancellationToken token = default) where T : class, IEntityBase;

    /// <summary>
    /// Начинает транзакцию базы данных
    /// </summary>
    public void BeginTransaction();

    /// <summary>
    /// Асинхронно начинает транзакцию базы данных
    /// </summary>
    /// <param name="token">Токен отмены операции</param>
    Task BeginTransactionAsync(CancellationToken token = default);

    /// <summary>
    /// Подтверждает транзакцию. Внимание! Сохранение изменений не включено!
    /// </summary>
    void Commit();

    /// <summary>
    /// Асинхронно подтверждает транзакцию. Внимание! Сохранение изменений не включено!
    /// </summary>
    /// <param name="token">Токен отмены операции</param>
    Task CommitAsync(CancellationToken token = default);

    /// <summary>
    /// Откатывает транзакцию
    /// </summary>
    void Rollback();

    /// <summary>
    /// Асинхронно откатывает транзакцию
    /// </summary>
    /// <param name="token">Токен отмены операции</param>
    Task RollbackAsync(CancellationToken token = default);
}