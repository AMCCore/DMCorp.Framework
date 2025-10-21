using System.Text;
using DMCorp.Framework.Basics.Settings;
using Microsoft.IdentityModel.Tokens;

namespace DMCorp.Framework.Basics.Security;

/// <summary>
/// Базовый класс проверки токена авторизации
/// </summary>
public class BasicTokenValidator(ISecurityTokenValidator securityTokenValidator) : ITokenValidator
{
    public bool IsTokenValid(string token)
    {
        var mySecret = Encoding.UTF8.GetBytes(BaseAppSettings.SecKey);
        var mySecurityKey = new SymmetricSecurityKey(mySecret);

        try
        {
            securityTokenValidator.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = BaseAppSettings.ISSUER,
                ValidAudience = BaseAppSettings.AUDIENCE,
                IssuerSigningKey = mySecurityKey,
            }, out SecurityToken validatedToken);
        }
        catch
        {
            return false;
        }
        return true;
    }
}