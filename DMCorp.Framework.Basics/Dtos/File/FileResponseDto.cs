namespace DMCorp.Framework.Basics.Dtos.File;

/// <summary>
/// DTO ответа с данными файла или медиа (поток, MIME-тип, имя).
/// </summary>
public sealed class FileResponseDto : IDisposable
{
    /// <summary>
    /// Имя файла.
    /// </summary>
    public string? FileNmae { get; set; }

    /// <summary>
    /// MIME-тип содержимого.
    /// </summary>
    public string? ContentType { get; set; }

    /// <summary>
    /// Поток с содержимым файла.
    /// </summary>
    public Stream? Content { get; set; }

    /// <summary>
    /// Признак успешного получения файла (есть содержимое, имя и тип).
    /// </summary>
    public bool IsSuccess => Content?.Length > 0 && !string.IsNullOrEmpty(FileNmae) && !string.IsNullOrWhiteSpace(ContentType);

    /// <summary>
    /// Освобождает поток содержимого.
    /// </summary>
    public void Dispose()
    {
        Content?.Dispose();
    }
}