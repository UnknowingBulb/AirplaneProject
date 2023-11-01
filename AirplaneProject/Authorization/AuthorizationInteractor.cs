using AiplaneProject.Models;
using AirplaneProject.Database.DatabaseContextes;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AirplaneProject.Authorization
{
    public class AuthorizationInteractor
    {
        private readonly CustomerDbContext _context;
        public AuthorizationInteractor(CustomerDbContext context)
        {
            _context = context;
        }
        public User? GetUser(string? authToken)
        {
            if (JwtToken.ValidateToken(authToken) == false)
                return null;

            var claims = JwtToken.ExtractClaims(authToken);
            if (claims.IsNullOrEmpty())
                return null;

            var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.UserRole)?.Value;
            var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.UserId)?.Value;

            if (userId == null) return null;

            if (role == null || Enum.Parse<RoleTypes>(role) == RoleTypes.Customer)
            {
                return _context.Customer.FirstOrDefault(c => c.Id == Guid.Parse(userId));
            }
            //TODO: впихнуть сюда сотрудника
            return _context.Customer.FirstOrDefault(c => c.Id == Guid.Parse(userId));
        }
    }
}
