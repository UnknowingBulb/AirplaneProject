using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace AirplaneProject.Authorization
{
    public class JwtToken
    {
        /// <summary>
        /// Параметры валидации токена
        /// </summary>
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

        /// <summary>
        /// Создать токен
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="role">Роль</param>
        public static string GenerateToken(Guid userId, string role)
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
                    new Claim(ClaimTypes.UserRole, role)
                },
                expires: DateTime.UtcNow.AddDays(1));

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(secToken);
        }

        /// <summary>
        /// Валидация токена
        /// </summary>
        /// <param name="authToken">Токен</param>
        public static bool ValidateToken(string? authToken)
        {
            if (authToken == null || authToken == string.Empty)
                return false;

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(authToken, TokenValidationParameters, out _);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Получить Claims из токена
        /// </summary>
        /// <param name="authToken"><Токен/param>
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
