using AirplaneProject.Application.Interfaces;
using AirplaneProject.Infrastructure.Authorization;
using FluentResults;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authorization
{
    public class UserSecurity : IUserSecurity
    {
        /// <summary>
        /// Проверяет токен и возвращает идентификатор пользователя, если удалось его извлечь
        /// </summary>
        /// <param name="authToken">Токен авторизации пользователя</param>
        public Result<string> ValidateJwtAndGetUserId(string authToken)
        {
            if (JwtToken.ValidateToken(authToken) == false)
                return Result.Fail("Не удалось получить пользователя");

            var claims = JwtToken.ExtractClaims(authToken);
            if (claims.IsNullOrEmpty())
                return Result.Fail("Не удалось получить данные из токена");

            var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.UserId)?.Value;

            if (userId == null)
                return Result.Fail("Не удалось получить данные из токена");

            return Result.Ok(userId);
        }

        /// <summary>
        /// Сверить захешированный пароль с переданным
        /// </summary>
        /// <param name="passwordHash">Захешированный пароль</param>
        /// <param name="password">Пароль</param>
        public bool ValidatePasswordHash(string passwordHash, string password)
        {
            return PasswordHasher.Validate(passwordHash, password);
        }

        /// <summary>
        /// Хэшировать пароль
        /// </summary>
        /// <param name="password">Пароль</param>
        public string HashPassword(string password)
        {
            return PasswordHasher.Hash(password);
        }
    }
}
