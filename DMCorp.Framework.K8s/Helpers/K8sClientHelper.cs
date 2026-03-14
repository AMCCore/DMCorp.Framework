using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using k8s;

namespace DMCorp.Framework.K8s.Helpers;

public static class K8sClientHelper
{
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
