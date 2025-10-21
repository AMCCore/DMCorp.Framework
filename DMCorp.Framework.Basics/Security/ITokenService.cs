using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMCorp.Framework.Basics.Security;

/// <summary>
/// Сервис работы с токеном авторизации
/// </summary>
public interface ITokenService : ITokenValidator
{
    /// <summary>
    /// Создает токен пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    string BuildToken(Guid userId);
}