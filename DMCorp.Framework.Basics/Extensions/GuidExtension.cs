namespace DMCorp.Framework.Basics.Extensions;

public static class GuidExtension
{
    public static bool IsNullOrEmpty(this Guid? g)
    {
        return !g.HasValue || g.Value == Guid.Empty;
    }

    public static bool IsNullOrEmpty(this Guid g)
    {
        return g == Guid.Empty;
    }
}
