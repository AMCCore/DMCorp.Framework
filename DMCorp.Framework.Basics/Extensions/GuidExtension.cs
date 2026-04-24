namespace DMCorp.Framework.Basics.Extensions;

/// <summary>
/// Расширения для работы с GUID
/// </summary>
public static class GuidExtension
{
    /// <summary>
    /// Проверяет, является ли nullable GUID пустым или null
    /// </summary>
    /// <param name="g">Nullable GUID для проверки</param>
    /// <returns>True, если GUID равен null или Guid.Empty, иначе false</returns>
    public static bool IsNullOrEmpty(this Guid? g)
    {
        return !g.HasValue || g.Value == Guid.Empty;
    }

    /// <summary>
    /// Проверяет, является ли GUID пустым
    /// </summary>
    /// <param name="g">GUID для проверки</param>
    /// <returns>True, если GUID равен Guid.Empty, иначе false</returns>
    public static bool IsNullOrEmpty(this Guid g)
    {
        return g == Guid.Empty;
    }

    public static TEnum GetEnum<TEnum>(this Guid g) where TEnum : Enum
    {
        return GuidEnumConverterExtension<TEnum>.ConvertGuidToEnum(g);
    }
}
