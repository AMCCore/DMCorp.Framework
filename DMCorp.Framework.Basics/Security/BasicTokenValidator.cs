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
    /// <returns>Параметры валидации токена безопасности</returns>
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
    /// Возвращает параметры валидации токена. Может быть переопределен в наследниках.
    /// </summary>
    /// <returns>Параметры валидации токена безопасности</returns>
    protected virtual TokenValidationParameters GetTokenValidationParameters() => GetBasicTokenValidationParameters();


    /// <summary>
    /// Проверяет валидность токена авторизации
    /// </summary>
    /// <param name="token">Токен для проверки</param>
    /// <returns>True, если токен валиден, иначе false</returns>
    public virtual bool IsTokenValid(string token)
    {
        try
        {
            securityTokenValidator.ValidateToken(token, GetTokenValidationParameters(), out SecurityToken validatedToken);
        }
        catch
        {
            return false;
        }
        return true;
    }
}