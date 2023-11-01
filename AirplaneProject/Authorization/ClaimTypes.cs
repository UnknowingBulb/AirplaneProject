using System.IdentityModel.Tokens.Jwt;

namespace AirplaneProject.Authorization
{
    public struct ClaimTypes
    {
        public const string UserId = JwtRegisteredClaimNames.Sub;
        public const string UserRole = System.Security.Claims.ClaimTypes.Role;
    }
}
