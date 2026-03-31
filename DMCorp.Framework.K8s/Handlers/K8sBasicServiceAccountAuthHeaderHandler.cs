using System.Net.Http.Headers;
using DMCorp.Framework.K8s.Helpers;
using k8s;
using k8s.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace DMCorp.Framework.K8s.Handlers;

/// <summary>
/// Обработчик HTTP-запросов для добавления заголовка авторизации с токеном сервисного аккаунта Kubernetes.
/// Поддерживает кэширование токена для оптимизации производительности.
/// </summary>
public class K8sBasicServiceAccountAuthHeaderHandler(IMemoryCache? cache = default) : DelegatingHandler
{
    /// <summary>
    /// Кэш для хранения токена
    /// </summary>
    private readonly IMemoryCache? _cache = cache;

    /// <summary>
    /// Ключ кэша для хранения токена
    /// </summary>
    private const string TokenCacheKey = "K8sTokenCache";

    /// <summary>
    /// Ключ кэша для хранения времени истечения токена
    /// </summary>
    private const string TokenCacheExpirationKey = "K8sTokenExpirationCache";

    /// <summary>
    /// Получает токен авторизации Kubernetes для сервисного аккаунта.
    /// Токен кэшируется для повторного использования до истечения срока действия.
    /// </summary>
    /// <param name="token">Токен отмены операции</param>
    /// <returns>Строка токена авторизации</returns>
    protected async Task<string> GetK8sToken(CancellationToken token = default)
    {
        if ((_cache?.TryGetValue(TokenCacheKey, out string? _token) ?? false))
        {
            if ((_cache?.TryGetValue(TokenCacheExpirationKey, out DateTimeOffset? _tokenExpires) ?? false))
            {
                if (!string.IsNullOrWhiteSpace(_token))
                {
                    if (DateTimeOffset.UtcNow < _tokenExpires)
                    {
                        return _token;
                    }
                }
            }
        }

        using var k8s_client = K8sClientHelper.GetClient();
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
        _cache?.Set(TokenCacheExpirationKey, DateTimeOffset.UtcNow.AddSeconds(k8s_request.Spec.ExpirationSeconds.Value), TimeSpan.FromSeconds(k8s_request.Spec.ExpirationSeconds.Value - 60));

        return jtoken.Status.Token;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
        var token = await GetK8sToken(cancellationToken);

        //potentially refresh token here if it has expired etc.
        request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
        //todo: add tenant header if needed
        //request.Headers.Add("X-Tenant-Id", tenantProvider.GetTenantId());

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}