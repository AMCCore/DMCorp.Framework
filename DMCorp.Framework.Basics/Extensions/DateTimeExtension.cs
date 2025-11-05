namespace DMCorp.Framework.Basics.Extensions;

/// <summary>
/// Расширения для работы с датой и временем
/// </summary>
public static class DateTimeExtension
{
    private static readonly TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Moscow");

    /// <summary>
    /// Получает текущее время в часовом поясе Москвы
    /// </summary>
    public static DateTime MoscowNow
    {
        get
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cstZone);
        }
    }

    /// <summary>
    /// Получает текущее время в часовом поясе Москвы
    /// </summary>
    /// <param name="dateTime">Экземпляр DateTime (не используется, требуется для синтаксиса расширения)</param>
    /// <returns>Текущее время в часовом поясе Москвы</returns>
    public static DateTime GetMoscowNow(this DateTime dateTime)
    {
        return MoscowNow;
    }
}
