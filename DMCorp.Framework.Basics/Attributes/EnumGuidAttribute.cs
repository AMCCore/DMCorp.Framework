namespace DMCorp.Framework.Basics.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class EnumGuidAttribute(string guid) : Attribute
{
    public Guid Guid { get; } = new Guid(guid);
}