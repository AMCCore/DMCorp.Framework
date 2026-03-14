using System.Text;
using DMCorp.Framework.Basics.Security;
using DMCorp.Framework.Basics.Settings;
using DMCorp.Framework.K8s.Helpers;
using Microsoft.IdentityModel.Tokens;

namespace DMCorp.Framework.K8s.Security;

/// <summary>
/// Валидатор токенов для аутентификации через Kubernetes Service Account
/// </summary>
public class K8sTokenValidator(ISecurityTokenValidator securityTokenValidator) : BasicTokenValidator(securityTokenValidator)
{
    /// <summary>
    /// Создает параметры валидации токена для Kubernetes Service Account
    /// </summary>
    public static TokenValidationParameters GetK8sTokenValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://kubernetes.default.svc",
            ValidateAudience = true,
            ValidAudience = K8sEnvironmentVariablesHelper.K8sTokenAudience,
        };
    }

    /// <summary>
    /// Возвращает параметры валидации токена Kubernetes
    /// </summary>
    protected override TokenValidationParameters GetTokenValidationParameters() => GetK8sTokenValidationParameters();
}
