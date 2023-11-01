using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace AirplaneProject.Authorization
{
    public class JwtToken
    {
        public static TokenValidationParameters TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };

        public static string GenerateToken(Guid userId, RoleTypes role)
        {
            var securityKey = TokenValidationParameters.IssuerSigningKey;
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var secToken = new JwtSecurityToken(
                signingCredentials: credentials,
                issuer: TokenValidationParameters.ValidIssuer,
                audience: TokenValidationParameters.ValidAudience,
                claims: new[]
                {
                    new Claim(ClaimTypes.UserId, userId.ToString()),
                    new Claim(ClaimTypes.UserRole, role.ToString())
                },
                expires: DateTime.UtcNow.AddDays(1));

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(secToken);
        }

        public static bool ValidateToken(string? authToken)
        {
            if (authToken == null || authToken == string.Empty)
                return false;

            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken validatedToken;
            try
            {
                IPrincipal principal = tokenHandler.ValidateToken(authToken, TokenValidationParameters, out validatedToken);
            }
            //TODO уточнить что за тип исключения там
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static IEnumerable<Claim> ExtractClaims(string? authToken)
        {
            if (!ValidateToken(authToken))
                return Array.Empty<Claim>();

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken securityToken = (JwtSecurityToken)tokenHandler.ReadToken(authToken);
            return securityToken.Claims;
        }
    }
}
