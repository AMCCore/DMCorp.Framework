using DMCorp.Framework.Basics.Settings;

namespace DMCorp.Framework.Basics.Email;

/// <summary>
/// Интерфейс сервиса для отправки электронной почты
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Настройки сервиса электронной почты
    /// </summary>
    IEmailServiceSettings Settings { get; }

    /// <summary>
    /// Асинхронно отправляет электронное письмо
    /// </summary>
    /// <param name="model">Модель данных для отправки письма</param>
    /// <param name="token">Токен отмены операции</param>
    Task SendAsync(EmailSendDto model, CancellationToken token = default);

    /// <summary>
    /// Отправляет электронное письмо
    /// </summary>
    /// <param name="model">Модель данных для отправки письма</param>
    void Send(EmailSendDto model);
}