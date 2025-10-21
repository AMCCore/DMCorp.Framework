using DMCorp.Framework.Basics.Attributes;
using System.ComponentModel;

namespace DMCorp.Framework.Basics.Extensions;

public static class EnumExtension
{
    public static Guid GetEnumGuid(this Enum e)
    {
        var enumType = e.GetType();
        var name = Enum.GetName(enumType, e) ?? throw new Exception($"{nameof(GetEnumGuid)} Exception: Enum Name is not recognized");
        var res = enumType?.GetField(name)?.GetCustomAttributes(typeof(EnumGuidAttribute), true).Cast<EnumGuidAttribute>().Select(s => s.Guid).FirstOrDefault() ?? default;
        return res;
    }

    public static string? GetEnumDescription(this Enum e)
    {
        var enumType = e.GetType();
        var name = Enum.GetName(enumType, e) ?? throw new Exception($"{nameof(GetEnumDescription)} Exception: Enum Description is not recognized");
        var res = enumType?.GetField(name)?.GetCustomAttributes(typeof(DescriptionAttribute), true).Cast<DescriptionAttribute>().Select(s => s.Description).FirstOrDefault() ?? default;
        return res;
    }
}