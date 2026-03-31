using DMCorp.Framework.K8s.Handlers;
using DMCorp.Framework.K8s.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DMCorp.Framework.K8s.Helpers;

public class JwtBearerOptionsHelper(IK8sJwksProvider jwksProvider, ILogger<JwtBearerOptionsHelper>? logger = null) : IConfigureNamedOptions<JwtBearerOptions>
{
    public void Configure(JwtBearerOptions options) => Configure(JwtBearerDefaults.AuthenticationScheme, options);

    public void Configure(string? name, JwtBearerOptions options)
    {
        logger?.LogInformation("Configuring JwtBearerOptions for scheme: {SchemeName}", name);

        if (name != JwtBearerDefaults.AuthenticationScheme)
            return;

        options.Authority = K8sEnvironmentVariablesHelper.K8sJWTAuthority;
        options.TokenValidationParameters = K8sTokenValidator.GetK8sTokenValidationParameters();
        options.TokenValidationParameters.IssuerSigningKeyResolver = (_, __, ___, ____) => jwksProvider.GetJwksAsync().GetAwaiter().GetResult().GetSigningKeys();
        options.BackchannelHttpHandler = new K8sJWTBackChannelHandler();
    }
}