using Microsoft.IdentityModel.Tokens;

namespace DMCorp.Framework.K8s.Security;

/// <summary>
/// Провайдер JWKS из Kubernetes API для валидации JWT (ServiceAccount токенов).
/// </summary>
public interface IK8sJwksProvider
{
    /// <summary>
    /// Возвращает набор ключей подписи из K8s OpenID Connect JWKS endpoint.
    /// </summary>
    Task<JsonWebKeySet> GetJwksAsync(CancellationToken cancellationToken = default);
}