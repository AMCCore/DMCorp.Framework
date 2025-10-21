using DMCorp.Framework.Basics.DAL;
using System.Linq.Expressions;

namespace DMCorp.Framework.Basics.Extensions;

public static class RepositoryExtension
{
    public static void AddOrUpdate<T>(this IUnitOfWork uw, ICollection<T> data, Action<T, T> copy, Expression<Func<T, bool>>? predicate = null) where T : class, IEntityBase
    {
        ArgumentNullException.ThrowIfNull(data);
        ArgumentNullException.ThrowIfNull(copy);

        foreach (var r in data)
        {
            T? entity = null;
            if (predicate != null)
            {
                entity = uw.Query<T>(true).SingleOrDefault(predicate);
            }
            else
            {
                entity = uw.Query<T>(true).SingleOrDefault(x => x.Id  == r.Id);
            }

            if (entity == null)
            {
                uw.AddEntity(r, false);
            }
            else
            {
                copy.Invoke(r, entity);
            }
        }
        uw.SaveChanges();
    }

    public static void AddIfNotExists<T>(this IUnitOfWork uw, ICollection<T> data) where T : class, IEntityBase
    {
        var entitys = uw.Query<T>(true).Select(x => x.Id).ToArray();
        foreach (var r in data)
        {
            if (entitys.All(x => x != r.Id))
            {
                uw.AddEntity(r, false);
            }
        }
        uw.SaveChanges();
    }

    public static T AddIfNotExists<T>(this IUnitOfWork uw, T data, Expression<Func<T, bool>> predicate) where T : class, IEntityBase
    {
        ArgumentNullException.ThrowIfNull(data);
        ArgumentNullException.ThrowIfNull(predicate);

        if (!uw.Query<T>(true).Any(predicate))
        {
            return uw.AddEntity(data, true);
        }

        return uw.Query<T>(true).Single(predicate);
    }
}