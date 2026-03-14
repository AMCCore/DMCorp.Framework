using DMCorp.Framework.Basics.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DMCorp.Framework.Basics.DAL;

/// <summary>
/// Базовый класс для реализации паттерна Unit of Work с поддержкой транзакций и операций с базой данных
/// </summary>
/// <typeparam name="C">Тип контекста базы данных, наследующий DbContext</typeparam>
public abstract class BaseUnitOfWork<C>(C context) : IUnitOfWork where C : DbContext
{
    /// <summary>
    /// Контекст базы данных Entity Framework
    /// </summary>
    public DbContext Context => context ?? throw new ArgumentNullException(nameof(context));

    private IDbContextTransaction? _transaction = null;

    /// <summary>
    /// Флаг, указывающий, что не нужно изменять LastUpdateTick при сохранении
    /// </summary>
    public bool NotChangeLastUpdateTick { get; set; }

    /// <summary>
    /// Отключает автоматическое отслеживание изменений в контексте
    /// </summary>
    public void AutoDetectChangesDisable()
    {
        Context.ChangeTracker.AutoDetectChangesEnabled = false;
    }

    /// <summary>
    /// Включает автоматическое отслеживание изменений в контексте
    /// </summary>
    public void AutoDetectChangesEnable()
    {
        Context.ChangeTracker.AutoDetectChangesEnabled = true;
    }

    /// <summary>
    /// Начинает транзакцию базы данных
    /// </summary>
    /// <exception cref="Exception">Выбрасывается, если транзакция уже была начата</exception>
    public void BeginTransaction()
    {
        if (_transaction != null)
        {
            throw new Exception("The transaction has been already begun");
        }

        _transaction = Context.Database.BeginTransaction();
    }

    /// <summary>
    /// Асинхронно начинает транзакцию базы данных
    /// </summary>
    /// <param name="token">Токен отмены операции</param>
    /// <exception cref="Exception">Выбрасывается, если транзакция уже была начата</exception>
    public async Task BeginTransactionAsync(CancellationToken token = default)
    {
        if (_transaction != null)
        {
            throw new Exception("The transaction has been already begun");
        }

        _transaction = await Context.Database.BeginTransactionAsync(token);
    }

    /// <summary>
    /// Подтверждает транзакцию. <b>Внимание!</b> Сохранение изменений не включено!
    /// </summary>
    public void Commit()
    {
        _transaction?.Commit();
    }

    /// <summary>
    /// Асинхронно подтверждает транзакцию. <b>Внимание!</b> Сохранение изменений не включено!
    /// </summary>
    /// <param name="token">Токен отмены операции</param>
    public async Task CommitAsync(CancellationToken token = default)
    {
        if (_transaction != null)
            await _transaction.CommitAsync(token);
    }

    /// <summary>
    /// Откатывает транзакцию
    /// </summary>
    public void Rollback()
    {
        _transaction?.Rollback();
    }

    /// <summary>
    /// Асинхронно откатывает транзакцию
    /// </summary>
    /// <param name="token">Токен отмены операции</param>
    public async Task RollbackAsync(CancellationToken token = default)
    {
        if (_transaction != null)
            await _transaction.RollbackAsync(token);
    }

    /// <summary>
    /// Освобождает ресурсы, используемые экземпляром BaseUnitOfWork
    /// </summary>
    public void Dispose()
    {
        _transaction?.Dispose();
        Context?.Dispose();
        //Context = null;
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Сохраняет все изменения в базе данных
    /// </summary>
    /// <param name="hardDelete">Если true, выполняется физическое удаление вместо мягкого</param>
    public void SaveChanges(bool hardDelete = false)
    {
        ProcessEntityOnSave(hardDelete);
        Context.SaveChanges();
    }

    /// <summary>
    /// Асинхронно сохраняет все изменения в базе данных
    /// </summary>
    /// <param name="hardDelete">Если true, выполняется физическое удаление вместо мягкого</param>
    /// <param name="token">Токен отмены операции</param>
    public async Task SaveChangesAsync(bool hardDelete = false, CancellationToken token = default)
    {
        ProcessEntityOnSave(hardDelete);
        await Context.SaveChangesAsync(token);
    }

    /// <summary>
    /// Добавляет новую сущность в базу данных
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующий IEntityBase</typeparam>
    /// <param name="entity">Сущность для добавления</param>
    /// <param name="saveChanges">Если true, изменения сохраняются сразу</param>
    /// <returns>Добавленная сущность</returns>
    public T AddEntity<T>(T entity, bool saveChanges = true) where T : class, IEntityBase
    {
        if (entity.Id.IsNullOrEmpty())
        {
            entity.Id = Guid.NewGuid();
        }

        var res = Context.Set<T>().Add(entity);

        if (saveChanges)
        {
            SaveChanges();
            Context.Entry(entity).State = EntityState.Detached;
            return Context.Set<T>().Single(x => x.Id == entity.Id);
        }

        return res.Entity;
    }

    /// <summary>
    /// Асинхронно добавляет новую сущность в базу данных
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующий IEntityBase</typeparam>
    /// <param name="entity">Сущность для добавления</param>
    /// <param name="saveChanges">Если true, изменения сохраняются сразу</param>
    /// <param name="token">Токен отмены операции</param>
    /// <returns>Добавленная сущность</returns>
    public async Task<T> AddEntityAsync<T>(T entity, bool saveChanges = true, CancellationToken token = default) where T : class, IEntityBase
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (entity.Id.IsNullOrEmpty())
        {
            entity.Id = Guid.NewGuid();
        }

        var res = await Context.Set<T>().AddAsync(entity, token);

        if(saveChanges)
        {
            await SaveChangesAsync(token: token);
            Context.Entry(entity).State = EntityState.Detached;
            entity = await Context.Set<T>().SingleAsync(x => x.Id == entity.Id, token);
        }

        return res.Entity;
    }

    /// <summary>
    /// Удаляет сущность из базы данных
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующий IEntityBase</typeparam>
    /// <param name="entity">Сущность для удаления</param>
    /// <param name="hardDelete">Если true, выполняется физическое удаление вместо мягкого</param>
    /// <param name="saveChanges">Если true, изменения сохраняются сразу</param>
    public void Delete<T>(T entity, bool hardDelete = false, bool saveChanges = true) where T : class, IEntityBase
    {
        Context.Entry(entity).State = EntityState.Deleted;
        if(saveChanges)
            SaveChanges(hardDelete);
    }

    /// <summary>
    /// Асинхронно удаляет сущность из базы данных
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующий IEntityBase</typeparam>
    /// <param name="entity">Сущность для удаления</param>
    /// <param name="hardDelete">Если true, выполняется физическое удаление вместо мягкого</param>
    /// <param name="saveChanges">Если true, изменения сохраняются сразу</param>
    /// <param name="token">Токен отмены операции</param>
    public async Task DeleteAsync<T>(T entity, bool hardDelete = false, bool saveChanges = true, CancellationToken token = default) where T : class, IEntityBase
    {
        Context.Entry(entity).State = EntityState.Deleted;
        if (saveChanges)
            await SaveChangesAsync(hardDelete, token);
    }

    /// <summary>
    /// Удаляет список сущностей из базы данных
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующий IEntityBase</typeparam>
    /// <param name="entities">Список сущностей для удаления</param>
    /// <param name="hardDelete">Если true, выполняется физическое удаление вместо мягкого</param>
    /// <param name="saveChanges">Если true, изменения сохраняются сразу</param>
    public void DeleteList<T>(IEnumerable<T> entities, bool hardDelete = false, bool saveChanges = true) where T : class, IEntityBase
    {
        foreach (var entity in entities)
        {
            Context.Set<T>().Attach(entity);
            Context.Entry(entity).State = EntityState.Deleted;
        }

        if(saveChanges)
            SaveChanges(hardDelete);
    }

    /// <summary>
    /// Асинхронно удаляет список сущностей из базы данных
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующий IEntityBase</typeparam>
    /// <param name="entities">Список сущностей для удаления</param>
    /// <param name="hardDelete">Если true, выполняется физическое удаление вместо мягкого</param>
    /// <param name="saveChanges">Если true, изменения сохраняются сразу</param>
    /// <param name="token">Токен отмены операции</param>
    public async Task DeleteListAsync<T>(IEnumerable<T> entities, bool hardDelete = false, bool saveChanges = true, CancellationToken token = default) where T : class, IEntityBase
    {
        foreach (var entity in entities)
        {
            Context.Set<T>().Attach(entity);
            Context.Entry(entity).State = EntityState.Deleted;
        }

        if(saveChanges)
            await SaveChangesAsync(hardDelete, token);
    }

    /// <summary>
    /// Получает набор сущностей указанного типа
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующий IEntityBase</typeparam>
    /// <returns>Набор сущностей указанного типа</returns>
    public DbSet<T> GetSet<T>() where T : class, IEntityBase
    {
        return Context.Set<T>();
    }

    /// <summary>
    /// Получает запрос для работы с сущностями указанного типа
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующий IEntityBase</typeparam>
    /// <param name="withDeleted">Если true, включаются удаленные сущности (для мягкого удаления)</param>
    /// <returns>Запрос для работы с сущностями</returns>
    public IQueryable<T> Query<T>(bool withDeleted = false) where T : class, IEntityBase
    {
        if (withDeleted && typeof(T) is ISoftDeleteEntity)
        {
            return GetSet<T>().IgnoreQueryFilters();
        }

        return GetSet<T>().AsQueryable();
    }

    /// <summary>
    /// Обрабатывает сущности перед сохранением: устанавливает дату создания, обновляет метку времени, обрабатывает мягкое удаление
    /// </summary>
    /// <param name="hardDelete">Если true, выполняется физическое удаление вместо мягкого</param>
    protected virtual void ProcessEntityOnSave(bool hardDelete)
    {
        var states = new [] { EntityState.Added, EntityState.Modified };

        if (!hardDelete)
        {
            var entitystoDelete = Context.ChangeTracker.Entries<ISoftDeleteEntity>()
                .Where(c => c.State == EntityState.Deleted)
                .Select(c => c.Entity)
                .ToList();

            foreach (var entity in entitystoDelete)
            {
                entity.IsDeleted = true;
                Context.Entry(entity).State = EntityState.Modified;
            }
        }


        var entitystoCreate = Context.ChangeTracker.Entries<IEntityWithDateCreated>()
        .Where(c => c.State == EntityState.Added)
        .Select(c => c.Entity)
        .ToList();

        foreach (var entity in entitystoCreate)
        {
            entity.DateCreated = SetDateTimeNow();
        }

        if (NotChangeLastUpdateTick)
        {
            return;
        }

        // получение измененных
        var entitys = Context.ChangeTracker.Entries<IEntityBase>()
            .Where(c => states.Contains(c.State))
            .Select(c => c.Entity)
            .ToList();

        // фиксация факта изменений
        foreach (var entity in entitys)
        {
            entity.LastUpdateTick = SetDateTimeNow().Ticks;
        }
    }

    /// <summary>
    /// Возвращает текущую дату и время. Может быть переопределен в наследниках для использования другой логики определения времени
    /// </summary>
    /// <returns>Текущая дата и время</returns>
    protected virtual DateTimeOffset SetDateTimeNow()
    {
        return DateTimeOffset.Now;
    }
}