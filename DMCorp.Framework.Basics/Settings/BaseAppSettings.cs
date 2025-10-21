namespace DMCorp.Framework.Basics.Settings;

public class BaseAppSettings
{
    /// <summary>
    /// Общий ключ шифрования приложения (группы приложений)
    /// </summary>
    public static string SecKey => Environment.GetEnvironmentVariable(nameof(SecKey)) ?? throw new ArgumentNullException(nameof(SecKey));

    /// <summary>
    /// Поставщики токенов безопасности
    /// </summary>
    public static string ISSUER => Environment.GetEnvironmentVariable(nameof(ISSUER)) ?? "AMC.Core default User";

    /// <summary>
    /// Аудитория (получатели) токенов безопасности
    /// </summary>
    public static string AUDIENCE => Environment.GetEnvironmentVariable(nameof(AUDIENCE)) ?? "AMC.Core basic Audience";
}