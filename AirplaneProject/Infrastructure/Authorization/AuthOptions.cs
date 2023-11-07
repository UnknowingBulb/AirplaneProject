using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AirplaneProject.Infrastructure.Authorization
{
    public class AuthOptions
    {
        public const string ISSUER = "test1"; // издатель токена
        public const string AUDIENCE = "user1"; // потребитель токена
        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
