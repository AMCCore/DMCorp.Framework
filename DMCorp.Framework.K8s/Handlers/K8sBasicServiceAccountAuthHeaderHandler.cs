using DMCorp.Framework.K8s.Helpers;
using k8s;
using k8s.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace DMCorp.Framework.K8s.Handlers;

public class K8sBasicServiceAccountAuthHeaderHandler(ILogger<K8sBasicServiceAccountAuthHeaderHandler>? logger = default, IMemoryCache? cache = default) : DelegatingHandler
{
    private readonly IMemoryCache? _cache = cache;
    private const string TokenCacheKey = "K8sTokenCache";
    private const string TokenCacheExpirationKey = "K8sTokenExpirationCache";

    private async Task<string> GetK8sToken(CancellationToken token = default)
    {
        if ((_cache?.TryGetValue(TokenCacheKey, out string? _token) ?? false) && (_cache?.TryGetValue(TokenCacheExpirationKey, out DateTimeOffset? _tokenExpires) ?? false) && !string.IsNullOrWhiteSpace(_token) && _tokenExpires < DateTimeOffset.UtcNow )
        {
            return _token;
        }

        var k8s_client = K8sClientHelper.GetClient();
        var k8s_request = new Authenticationv1TokenRequest
        {
            Spec = new V1TokenRequestSpec
            {
                Audiences = new List<string> { K8sEnvironmentVariablesHelper.K8sTokenAudience },
                ExpirationSeconds = 7200
            }
        };

        var jtoken = await k8s_client.CreateNamespacedServiceAccountTokenAsync(k8s_request, K8sEnvironmentVariablesHelper.K8sServiceAccountName, K8sEnvironmentVariablesHelper.K8sServiceAccountNamespace, cancellationToken: token);

        _cache?.Set(TokenCacheKey, jtoken.Status.Token, TimeSpan.FromSeconds(k8s_request.Spec.ExpirationSeconds.Value - 60));
        DateTimeOffset utcTime1 = DateTime.SpecifyKind(jtoken.Status.ExpirationTimestamp, DateTimeKind.Utc);
        _cache?.Set(TokenCacheExpirationKey, utcTime1, TimeSpan.FromSeconds(k8s_request.Spec.ExpirationSeconds.Value - 60));

        return jtoken.Status.Token;
    }
}