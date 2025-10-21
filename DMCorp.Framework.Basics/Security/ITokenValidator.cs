namespace DMCorp.Framework.Basics.Security;

/// <summary>
/// Интерфейс проверки токена авторизации
/// </summary>
public interface ITokenValidator
{
    /// <summary>
    /// Валидация токена
    /// </summary>
    bool IsTokenValid(string token);
}