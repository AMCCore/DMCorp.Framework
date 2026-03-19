namespace DMCorp.Framework.K8s.Helpers;

/// <summary>
/// Вспомогательный класс для работы с переменными окружения Kubernetes
/// </summary>
public static class K8sEnvironmentVariablesHelper
{
    /// <summary>
    /// Имя сервисного аккаунта Kubernetes из переменной окружения
    /// </summary>
    /// <exception cref="ArgumentNullException">Выбрасывается, если переменная окружения K8sServiceAccountName не установлена</exception>
    public static string K8sServiceAccountName => Environment.GetEnvironmentVariable(nameof(K8sServiceAccountName)) ?? throw new ArgumentNullException(nameof(K8sServiceAccountName));

    /// <summary>
    /// Пространство имен (namespace) сервисного аккаунта Kubernetes из переменной окружения
    /// </summary>
    /// <exception cref="ArgumentNullException">Выбрасывается, если переменная окружения K8sServiceAccountNamespace не установлена</exception>
    public static string K8sServiceAccountNamespace => Environment.GetEnvironmentVariable(nameof(K8sServiceAccountNamespace)) ?? throw new ArgumentNullException(nameof(K8sServiceAccountNamespace));

    /// <summary>
    /// Аудитория (audience) для токена Kubernetes из переменной окружения
    /// </summary>
    /// <exception cref="ArgumentNullException">Выбрасывается, если переменная окружения K8sTokenAudience не установлена</exception>
    public static string K8sTokenAudience => Environment.GetEnvironmentVariable(nameof(K8sTokenAudience)) ?? throw new ArgumentNullException(nameof(K8sTokenAudience));

    public static string K8sJWTAuthority = Environment.GetEnvironmentVariable(nameof(K8sJWTAuthority)) ?? "https://kubernetes.default.svc";
}