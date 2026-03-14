using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMCorp.Framework.Basics.Security;

/// <summary>
/// Расширенный интерфейс сервиса безопасности с поддержкой прав доступа
/// </summary>
public interface IAdvancedSecurityService : ISecurityService
{
    /// <summary>
    /// Признак административного доступа
    /// </summary>
    bool IsAdmin { get; }

    /// <summary>
    /// Проверяет наличие указанного права у текущего пользователя
    /// </summary>
    /// <param name="right">Право для проверки</param>
    /// <returns>True, если у пользователя есть указанное право, иначе false</returns>
    bool HasRight(Enum right);

    /// <summary>
    /// Проверяет наличие хотя бы одного из указанных прав у текущего пользователя
    /// </summary>
    /// <param name="rights">Список прав для проверки</param>
    /// <returns>True, если у пользователя есть хотя бы одно из указанных прав, иначе false</returns>
    bool HasAnyRight(IEnumerable<Enum> rights);

    /// <summary>
    /// Возвращает список идентификаторов прав текущего пользователя
    /// </summary>
    IList<Guid> Rights { get; }
}