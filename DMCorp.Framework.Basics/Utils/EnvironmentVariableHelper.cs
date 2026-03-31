using Microsoft.Extensions.Configuration;

namespace DMCorp.Framework.Basics.Utils;

public static class EnvironmentVariableHelper
{
    public static void DebugSet(this IConfiguration builder, string Name, string? EnvName = default)
    {
        Environment.SetEnvironmentVariable(string.IsNullOrWhiteSpace(EnvName) ? Name : EnvName, builder.GetValue<string>(Name));
    }
}