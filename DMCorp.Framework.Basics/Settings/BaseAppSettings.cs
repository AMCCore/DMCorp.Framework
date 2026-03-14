namespace DMCorp.Framework.Basics.Settings;

/// <summary>
/// Базовые настройки приложения
/// </summary>
public class BaseAppSettings
{
    /// <summary>
    /// Общий ключ шифрования приложения (группы приложений)
    /// </summary>
    /// <exception cref="ArgumentNullException">Выбрасывается, если переменная окружения SecKey не установлена</exception>
    public static string SecKey => Environment.GetEnvironmentVariable(nameof(SecKey)) ?? throw new ArgumentNullException(nameof(SecKey));

    /// <summary>
    /// Поставщик токенов безопасности (издатель)
    /// </summary>
    public static string ISSUER => Environment.GetEnvironmentVariable(nameof(ISSUER)) ?? "AMC.Core default User";

    /// <summary>
    /// Аудитория (получатели) токенов безопасности
    /// </summary>
    public static string AUDIENCE => Environment.GetEnvironmentVariable(nameof(AUDIENCE)) ?? "AMC.Core basic Audience";
}