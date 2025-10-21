using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DMCorp.Framework.Basics.DAL;

public interface IUnitOfWork : IDisposable
{
    bool NotChangeLastUpdateTick { get; set; }

    //DbContext Context { get; }
    //IDbContextTransaction? Transaction { get; }

    void AutoDetectChangesDisable();

    void AutoDetectChangesEnable();

    void SaveChanges(bool hardDelete = false);

    Task SaveChangesAsync(bool hardDelete = false, CancellationToken token = default);

    DbSet<T> GetSet<T>() where T : class, IEntityBase;

    void Delete<T>(T entity, bool hardDelete = false, bool saveChanges = true) where T : class, IEntityBase;

    Task DeleteAsync<T>(T entity, bool hardDelete = false, bool saveChanges = true, CancellationToken token = default) where T : class, IEntityBase;

    void DeleteList<T>(IEnumerable<T> entities, bool hardDelete = false, bool saveChanges = true) where T : class, IEntityBase;

    Task DeleteListAsync<T>(IEnumerable<T> entities, bool hardDelete = false, bool saveChanges = true, CancellationToken token = default) where T : class, IEntityBase;

    T AddEntity<T>(T entity, bool saveChanges = true) where T : class, IEntityBase;

    Task<T> AddEntityAsync<T>(T entity, bool saveChanges = true, CancellationToken token = default) where T : class, IEntityBase;

    IQueryable<T> Query<T>(bool withDeleted = false) where T : class, IEntityBase;

    //T Update<T>(T entity) where T : class, IEntityBase;

    //Task<T> UpdateAsync<T>(T entity, CancellationToken token = default) where T : class, IEntityBase;

    public void BeginTransaction();

    Task BeginTransactionAsync(CancellationToken token = default);

    void Commit();

    Task CommitAsync(CancellationToken token = default);

    void Rollback();

    Task RollbackAsync(CancellationToken token = default);
}