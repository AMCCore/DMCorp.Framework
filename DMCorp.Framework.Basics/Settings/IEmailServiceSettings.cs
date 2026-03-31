namespace DMCorp.Framework.Basics.Settings;

public interface IEmailServiceSettings
{
    /// <summary>
    /// Адрес электронной почты отправителя
    /// </summary>
    string? OutAddress { get; set; }

    /// <summary>
    /// Отображаемое имя отправителя
    /// </summary>
    string? OutAddressDisplayName { get; set; }

    /// <summary>
    /// Хост SMTP сервера
    /// </summary>
    string Host { get; set; }

    /// <summary>
    /// Порт SMTP сервера
    /// </summary>
    int Port { get; set; }

    /// <summary>
    /// Логин для авторизации на SMTP сервере
    /// </summary>
    string? Login { get; set; }

    /// <summary>
    /// Пароль для авторизации на SMTP сервере
    /// </summary>
    string? Password { get; set; }
}
