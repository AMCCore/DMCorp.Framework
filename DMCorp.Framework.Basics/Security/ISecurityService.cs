namespace DMCorp.Framework.Basics.Security;

/// <summary>
/// Интерфейс сервиса безопасности для работы с текущим пользователем
/// </summary>
public interface ISecurityService
{
    /// <summary>
    /// Идентификатор текущего аккаунта
    /// </summary>
    Guid CurrentAccountId { get; }

    /// <summary>
    /// Признак авторизованности текущего аккаунта
    /// </summary>
    bool IsAuthenticated { get; }
}