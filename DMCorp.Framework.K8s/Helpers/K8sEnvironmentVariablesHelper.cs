namespace DMCorp.Framework.K8s.Helpers;

public static class K8sEnvironmentVariablesHelper
{
    public static string K8sServiceAccountName => Environment.GetEnvironmentVariable(nameof(K8sServiceAccountName)) ?? throw new ArgumentNullException(nameof(K8sServiceAccountName));

    public static string K8sServiceAccountNamespace => Environment.GetEnvironmentVariable(nameof(K8sServiceAccountNamespace)) ?? throw new ArgumentNullException(nameof(K8sServiceAccountNamespace));

    public static string K8sTokenAudience => Environment.GetEnvironmentVariable(nameof(K8sTokenAudience)) ?? throw new ArgumentNullException(nameof(K8sTokenAudience));
}