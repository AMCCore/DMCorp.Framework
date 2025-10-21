namespace DMCorp.Framework.Basics.Extensions;

public static class DateTimeExtension
{
    private static readonly TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Moscow");

    public static DateTime MoscowNow
    {
        get
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cstZone);
        }
    }

    public static DateTime GetMoscowNow(this DateTime dateTime)
    {
        return MoscowNow;
    }
}
