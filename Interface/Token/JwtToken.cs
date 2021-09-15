using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Interface.Token;

public class JwtToken
{
    public static bool IsEmptyOrInvalid(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return true;
        }

        var jwtToken = new JwtSecurityToken(token);
        return (jwtToken == null) || (jwtToken.ValidFrom > DateTime.UtcNow) || (jwtToken.ValidTo < DateTime.UtcNow);
    }

    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwtToken) => new JwtSecurityToken(jwtToken).Claims;
}