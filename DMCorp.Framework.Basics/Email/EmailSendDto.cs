namespace DMCorp.Framework.Basics.Email;

/// <summary>
/// Модель данных для отправки электронного письма
/// </summary>
public sealed record EmailSendDto
{
    /// <summary>
    /// Тема сообщения
    /// </summary>
    public required string Subject { get; set; }

    /// <summary>
    /// Тело сообщения
    /// </summary>
    public required string Body { get; set; }

    /// <summary>
    /// Указывает, что сообщение является HTML
    /// </summary>
    public bool IsHtml { get; set; } = false;

    /// <summary>
    /// Основные получатели письма
    /// </summary>
    public ICollection<string> MainAddresses { get; set; } = [];

    /// <summary>
    /// Получатели копии письма
    /// </summary>
    public ICollection<string> CopyAddresses { get; set; } = [];

    /// <summary>
    /// Получатели скрытой копии письма
    /// </summary>
    public ICollection<string> CopyHidenAddresses { get; set; } = [];
}