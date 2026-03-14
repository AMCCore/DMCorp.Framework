using DMCorp.Framework.Basics.Attributes;
using System.ComponentModel;

namespace DMCorp.Framework.Basics.Extensions;

/// <summary>
/// Расширения для работы с перечислениями
/// </summary>
public static class EnumExtension
{
    /// <summary>
    /// Получает GUID значение, связанное с элементом перечисления через атрибут EnumGuidAttribute
    /// </summary>
    /// <param name="e">Элемент перечисления</param>
    /// <returns>GUID значение, связанное с элементом перечисления, или Guid.Empty, если атрибут не найден</returns>
    /// <exception cref="Exception">Выбрасывается, если имя элемента перечисления не распознано</exception>
    public static Guid GetEnumGuid(this Enum e)
    {
        var enumType = e.GetType();
        var name = Enum.GetName(enumType, e) ?? throw new Exception($"{nameof(GetEnumGuid)} Exception: Enum Name is not recognized");
        var res = enumType?.GetField(name)?.GetCustomAttributes(typeof(EnumGuidAttribute), true).Cast<EnumGuidAttribute>().Select(s => s.Guid).FirstOrDefault() ?? default;
        return res;
    }

    /// <summary>
    /// Получает описание элемента перечисления из атрибута DescriptionAttribute
    /// </summary>
    /// <param name="e">Элемент перечисления</param>
    /// <returns>Описание элемента перечисления или null, если атрибут не найден</returns>
    /// <exception cref="Exception">Выбрасывается, если имя элемента перечисления не распознано</exception>
    public static string? GetEnumDescription(this Enum e)
    {
        var enumType = e.GetType();
        var name = Enum.GetName(enumType, e) ?? throw new Exception($"{nameof(GetEnumDescription)} Exception: Enum Description is not recognized");
        var res = enumType?.GetField(name)?.GetCustomAttributes(typeof(DescriptionAttribute), true).Cast<DescriptionAttribute>().Select(s => s.Description).FirstOrDefault() ?? default;
        return res;
    }
}