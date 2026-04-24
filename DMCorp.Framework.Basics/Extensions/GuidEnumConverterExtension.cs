using System.Linq.Expressions;
using DMCorp.Framework.Basics.Attributes;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DMCorp.Framework.Basics.Extensions;

/// <summary>
/// Конвертер значений для преобразования между перечислением и GUID в Entity Framework
/// </summary>
/// <typeparam name="TEnum">Тип перечисления</typeparam>
public class GuidEnumConverterExtension<TEnum>(ConverterMappingHints? mappingHints = null) : ValueConverter<TEnum, Guid>(ToGuid(), ToEnum(), mappingHints) where TEnum : Enum
{
    /// <summary>
    /// Создает выражение для преобразования перечисления в GUID
    /// </summary>
    /// <returns>Выражение для преобразования перечисления в GUID</returns>
    protected static Expression<Func<TEnum, Guid>> ToGuid() => v => v.GetEnumGuid();

    /// <summary>
    /// Создает выражение для преобразования GUID в перечисление
    /// </summary>
    /// <returns>Выражение для преобразования GUID в перечисление</returns>
    protected static Expression<Func<Guid, TEnum>> ToEnum() => v => ConvertGuidToEnum<TEnum>(v);

    /// <summary>
    /// Преобразует GUID значение в элемент перечисления по атрибуту EnumGuidAttribute
    /// </summary>
    /// <typeparam name="T">Тип перечисления</typeparam>
    /// <param name="value">GUID значение для поиска</param>
    /// <returns>Элемент перечисления, соответствующий указанному GUID</returns>
    /// <exception cref="InvalidOperationException">Выбрасывается, если не найден элемент перечисления с указанным GUID</exception>
    private static T ConvertGuidToEnum<T>(Guid value) where T : Enum
    {
        var names = Enum.GetNames(typeof(T));
        foreach (var name in names)
        {
            var val = typeof(T)?.GetField(name)?.GetCustomAttributes(true).OfType<EnumGuidAttribute>()
                .Select(ss => ss.Guid)
                .FirstOrDefault();
            if (val == value)
            {
                return (T)Enum.Parse(typeof(T), name);
            }
        }

        throw new InvalidOperationException();
    }

    internal static TEnum ConvertGuidToEnum(Guid value)
    {
        return ConvertGuidToEnum<TEnum>(value);
    }
}