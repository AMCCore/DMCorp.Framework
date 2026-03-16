using k8s;

namespace DMCorp.Framework.K8s.Helpers;

/// <summary>
/// Вспомогательный класс для создания клиента Kubernetes
/// </summary>
public static class K8sClientHelper
{
    /// <summary>
    /// Создает и возвращает экземпляр клиента Kubernetes.
    /// В режиме DEBUG использует конфигурационный файл из переменной окружения K8sConfigFilePath.
    /// В релизной сборке использует конфигурацию из кластера (InClusterConfig).
    /// </summary>
    /// <returns>Настроенный экземпляр клиента Kubernetes</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается в режиме DEBUG, если переменная окружения K8sConfigFilePath не задана</exception>
    public static Kubernetes GetClient()
    {
#if DEBUG

        var k8s_config = KubernetesClientConfiguration.BuildConfigFromConfigFile(Environment.GetEnvironmentVariable("K8sConfigFilePath") ?? throw new ArgumentNullException("K8sConfigFilePath"));

#else

        var k8s_config = KubernetesClientConfiguration.InClusterConfig();

#endif
        var k8s_client = new Kubernetes(k8s_config);
        return k8s_client;
    }
}