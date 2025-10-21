using DMCorp.Framework.Basics.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DMCorp.Framework.Basics.DAL;

public abstract class BaseUnitOfWork<C>(C context) : IUnitOfWork where C : DbContext
{
    private C Context = context ?? throw new ArgumentNullException(nameof(context));
    private IDbContextTransaction? _transaction = null;

    public bool NotChangeLastUpdateTick { get; set; }

    //DbContext IUnitOfWork.Context => _context;

    public void AutoDetectChangesDisable()
    {
        Context.ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public void AutoDetectChangesEnable()
    {
        Context.ChangeTracker.AutoDetectChangesEnabled = true;
    }

    public void BeginTransaction()
    {
        if (_transaction != null)
        {
            throw new Exception("The transaction has been already begun");
        }

        _transaction = Context.Database.BeginTransaction();
    }

    public async Task BeginTransactionAsync(CancellationToken token = default)
    {
        if (_transaction != null)
        {
            throw new Exception("The transaction has been already begun");
        }

        _transaction = await Context.Database.BeginTransactionAsync(token);
    }

    /// <summary>
    /// Commits transaction. <b>Warning!</b> Save changes is not included! 
    /// </summary>
    public void Commit()
    {
        _transaction?.Commit();
    }

    /// <summary>
    /// Commits transaction. <b>Warning!</b> Save changes is not included! 
    /// </summary>
    public async Task CommitAsync(CancellationToken token = default)
    {
        if (_transaction != null)
            await _transaction.CommitAsync(token);
    }

    /// <summary>
    /// Rollback transaction
    /// </summary>
    public void Rollback()
    {
        _transaction?.Rollback();
    }

    /// <summary>
    /// Rollback transaction
    /// </summary>
    public async Task RollbackAsync(CancellationToken token = default)
    {
        if (_transaction != null)
            await _transaction.RollbackAsync(token);
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        Context?.Dispose();
        Context = null;
        GC.SuppressFinalize(this);
    }

    public void SaveChanges(bool hardDelete = false)
    {
        ProcessEntityOnSave(hardDelete);
        Context.SaveChanges();
    }

    public async Task SaveChangesAsync(bool hardDelete = false, CancellationToken token = default)
    {
        ProcessEntityOnSave(hardDelete);
        await Context.SaveChangesAsync(token);
    }

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

    public void Delete<T>(T entity, bool hardDelete = false, bool saveChanges = true) where T : class, IEntityBase
    {
        Context.Entry(entity).State = EntityState.Deleted;
        if(saveChanges)
            SaveChanges(hardDelete);
    }

    public async Task DeleteAsync<T>(T entity, bool hardDelete = false, bool saveChanges = true, CancellationToken token = default) where T : class, IEntityBase
    {
        Context.Entry(entity).State = EntityState.Deleted;
        if (saveChanges)
            await SaveChangesAsync(hardDelete, token);
    }

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

    public DbSet<T> GetSet<T>() where T : class, IEntityBase
    {
        return Context.Set<T>();
    }

    public IQueryable<T> Query<T>(bool withDeleted = false) where T : class, IEntityBase
    {
        if (withDeleted && typeof(T) is ISoftDeleteEntity)
        {
            return GetSet<T>().IgnoreQueryFilters();
        }

        return GetSet<T>().AsQueryable();
    }

    private void ProcessEntityOnSave(bool hardDelete)
    {
        var states = new EntityState[] { EntityState.Added, EntityState.Modified };

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
            entity.DateCreated = DateTimeOffset.Now;
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
            entity.LastUpdateTick = DateTimeOffset.Now.Ticks;
        }
    }
}