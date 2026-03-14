namespace DMCorp.Framework.Basics.Settings;

/// <summary>
/// Настройки сервиса электронной почты
/// </summary>
public sealed record EmailServiceSettings
{
    /// <summary>
    /// Адрес электронной почты отправителя
    /// </summary>
    public string? OutAddress { get; set; }

    /// <summary>
    /// Отображаемое имя отправителя
    /// </summary>
    public string? OutAddressDisplayName { get; set; }

    /// <summary>
    /// Хост SMTP сервера
    /// </summary>
    public required string Host { get; set; }

    /// <summary>
    /// Порт SMTP сервера
    /// </summary>
    public int Port { get; set; } = 465;

    /// <summary>
    /// Логин для авторизации на SMTP сервере
    /// </summary>
    public string? Login { get; set; }

    /// <summary>
    /// Пароль для авторизации на SMTP сервере
    /// </summary>
    public string? Password { get; set; }
}