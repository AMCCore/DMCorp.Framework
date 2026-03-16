using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using DMCorp.Framework.K8s.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DMCorp.Framework.K8s.Security;

public class K8sJwksProvider(IMemoryCache? cache = default, ILogger<K8sJwksProvider>? logger = default) : IK8sJwksProvider
{
    private const string CacheKey = "K8s_Jwks";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

    private const string CaPath = "/var/run/secrets/kubernetes.io/serviceaccount/ca.crt";
    private const string SaTokenPath = "/var/run/secrets/kubernetes.io/serviceaccount/token";
    private const string JwksPath = "/openid/v1/jwks";

    private readonly IMemoryCache? _cache = cache;
    private readonly ILogger<K8sJwksProvider>? _logger = logger;

    public async Task<JsonWebKeySet> GetJwksAsync(CancellationToken cancellationToken = default)
    {
        if (_cache?.TryGetValue(CacheKey, out JsonWebKeySet? jwks) ?? false)
        {
            if (jwks is not null)
            {
                return jwks;
            }            
        }

        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = GetServerCertificateCustomValidationCallback();
        using var httpClient = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(10) };
        httpClient.DefaultRequestHeaders.Authorization = await DefaultRequestHeadersSet(cancellationToken);

        var json = await httpClient.GetStringAsync($"{K8sEnvironmentVariablesHelper.K8sJWTAuthority}{JwksPath}", cancellationToken);
        var res = new JsonWebKeySet(json);
        _cache?.Set(CacheKey, res, CacheDuration);
        return res;
    }

    public static Func<HttpRequestMessage, X509Certificate2?, X509Chain?, SslPolicyErrors, bool>? GetServerCertificateCustomValidationCallback() => (_, certificate, _, _) =>
    {
        if (File.Exists(CaPath))
        {
            if (certificate is null) return false;
            var serverCert = new X509Certificate2(certificate);
            var clusterCa = X509CertificateLoader.LoadCertificateFromFile(CaPath);
            using var chain = new X509Chain();
            chain.ChainPolicy.ExtraStore.Add(clusterCa);
            chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;
            chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
            chain.ChainPolicy.CustomTrustStore.Add(clusterCa);
            return chain.Build(serverCert);
        }
        return false;
    };

    public static async Task<AuthenticationHeaderValue?> DefaultRequestHeadersSet(CancellationToken cancellationToken = default)
    {
        if (File.Exists(SaTokenPath))
        {
            var saToken = await File.ReadAllTextAsync(SaTokenPath, cancellationToken);
            return new AuthenticationHeaderValue("Bearer", saToken.Trim());
        }
        return null;
    }
}
