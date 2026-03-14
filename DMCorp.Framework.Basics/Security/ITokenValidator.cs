namespace DMCorp.Framework.Basics.Security;

/// <summary>
/// Интерфейс проверки токена авторизации
/// </summary>
public interface ITokenValidator
{
    /// <summary>
    /// Проверяет валидность токена авторизации
    /// </summary>
    /// <param name="token">Токен для проверки</param>
    /// <returns>True, если токен валиден, иначе false</returns>
    bool IsTokenValid(string token);
}