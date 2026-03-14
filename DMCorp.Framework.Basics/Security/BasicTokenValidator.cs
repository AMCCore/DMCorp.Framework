using System.Text;
using DMCorp.Framework.Basics.Settings;
using Microsoft.IdentityModel.Tokens;

namespace DMCorp.Framework.Basics.Security;

/// <summary>
/// Базовый класс проверки токена авторизации
/// </summary>
public class BasicTokenValidator(ISecurityTokenValidator securityTokenValidator) : ITokenValidator
{
    /// <summary>
    /// Создает стандартные параметры валидации токена с настройками из переменных окружения
    /// </summary>
    /// <returns>Basic TokenValidationParameters for DMCorp</returns>
    public static TokenValidationParameters GetBasicTokenValidationParameters()
    {
        var mySecret = Encoding.UTF8.GetBytes(BaseAppSettings.SecKey);
        var mySecurityKey = new SymmetricSecurityKey(mySecret);


        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = BaseAppSettings.ISSUER,
            ValidAudience = BaseAppSettings.AUDIENCE,
            IssuerSigningKey = mySecurityKey,
        };
    }

    /// <summary>
    /// Проверяет валидность токена авторизации
    /// </summary>
    /// <param name="token">Токен для проверки</param>
    /// <returns>True, если токен валиден, иначе false</returns>
    public bool IsTokenValid(string token)
    {
        try
        {
            securityTokenValidator.ValidateToken(token, GetBasicTokenValidationParameters(), out SecurityToken validatedToken);
        }
        catch
        {
            return false;
        }
        return true;
    }
}