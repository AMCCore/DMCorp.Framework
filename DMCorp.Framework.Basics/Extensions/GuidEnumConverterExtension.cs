using System.Linq.Expressions;
using DMCorp.Framework.Basics.Attributes;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DMCorp.Framework.Basics.Extensions;

public class GuidEnumConverterExtension<TEnum>(ConverterMappingHints? mappingHints = null) : ValueConverter<TEnum, Guid>(ToGuid(), ToEnum(), mappingHints) where TEnum : Enum
{
    protected static Expression<Func<TEnum, Guid>> ToGuid() => v => v.GetEnumGuid();

    protected static Expression<Func<Guid, TEnum>> ToEnum() => v => ConvertGuidToEnum<TEnum>(v);

    public static T ConvertGuidToEnum<T>(Guid value)
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
}