namespace DMCorp.Framework.Basics.Email;

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
    /// Сообщение является HTML
    /// </summary>
    public bool IsHtml { get; set; } = false;

    /// <summary>
    /// Получатели
    /// </summary>
    public ICollection<string> MainAddresses { get; set; } = [];

    /// <summary>
    /// Получатели копий
    /// </summary>
    public ICollection<string> CopyAddresses { get; set; } = [];

    /// <summary>
    /// Получатели скрытых копий
    /// </summary>
    public ICollection<string> CopyHidenAddresses { get; set; } = [];
}