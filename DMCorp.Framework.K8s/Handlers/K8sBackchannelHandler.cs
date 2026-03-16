using DMCorp.Framework.K8s.Security;

namespace DMCorp.Framework.K8s.Handlers;

public sealed class K8sJWTBackChannelHandler : HttpClientHandler
{
    public K8sJWTBackChannelHandler()
    {
        ServerCertificateCustomValidationCallback = K8sJwksProvider.GetServerCertificateCustomValidationCallback();
    }


    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Authorization = await K8sJwksProvider.DefaultRequestHeadersSet(cancellationToken);
        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}