using DMCorp.Framework.Basics.DAL;
using System.Linq.Expressions;

namespace DMCorp.Framework.Basics.Extensions;

/// <summary>
/// Расширения для работы с репозиторием и Unit of Work
/// </summary>
public static class RepositoryExtension
{
    /// <summary>
    /// Добавляет или обновляет коллекцию сущностей в базе данных
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующий IEntityBase</typeparam>
    /// <param name="uw">Экземпляр Unit of Work</param>
    /// <param name="data">Коллекция сущностей для добавления или обновления</param>
    /// <param name="copy">Действие для копирования данных из новой сущности в существующую при обновлении</param>
    /// <param name="predicate">Предикат для поиска существующей сущности. Если null, используется поиск по Id</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если data или copy равны null</exception>
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

    /// <summary>
    /// Добавляет сущности в базу данных, если они еще не существуют (проверка по Id)
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующий IEntityBase</typeparam>
    /// <param name="uw">Экземпляр Unit of Work</param>
    /// <param name="data">Коллекция сущностей для добавления</param>
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

    /// <summary>
    /// Добавляет сущность в базу данных, если она еще не существует согласно предикату
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующий IEntityBase</typeparam>
    /// <param name="uw">Экземпляр Unit of Work</param>
    /// <param name="data">Сущность для добавления</param>
    /// <param name="predicate">Предикат для проверки существования сущности</param>
    /// <returns>Новая или существующая сущность</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если data или predicate равны null</exception>
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