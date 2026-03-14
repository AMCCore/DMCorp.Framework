namespace DMCorp.Framework.Basics.Attributes;

/// <summary>
/// Атрибут для указания GUID значения для элемента перечисления
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class EnumGuidAttribute(string guid) : Attribute
{
    /// <summary>
    /// GUID значение, связанное с элементом перечисления
    /// </summary>
    public Guid Guid { get; } = new Guid(guid);
}