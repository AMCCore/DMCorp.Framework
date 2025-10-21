using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMCorp.Framework.Basics.Security;

public interface IAdvancedSecurityService : ISecurityService
{
    /// <summary>
    /// Признак административного доступа
    /// </summary>
    bool IsAdmin { get; }

    /// <summary>
    /// Проверка наличия права
    /// </summary>
    bool HasRight(Enum right);

    /// <summary>
    /// Проверка наличия любого из прав
    /// </summary>
    bool HasAnyRight(IEnumerable<Enum> rights);

    /// <summary>
    /// Возвращает список прав текущего пользователя
    /// </summary>
    IList<Guid> Rights { get; }
}