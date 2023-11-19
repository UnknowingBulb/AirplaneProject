using FluentResults;

namespace AirplaneProject.Application.Interfaces
{
    public interface IUserSecurity
    {
        public Result<string> ValidateJwtAndGetUserId(string authToken);

        public bool ValidatePasswordHash(string passwordHash, string password);

        public string HashPassword(string password);
    }
}
