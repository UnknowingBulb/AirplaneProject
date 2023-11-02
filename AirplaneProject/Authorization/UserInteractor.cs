using AiplaneProject.Objects;
using AirplaneProject.Database.DatabaseContextes;
using FluentResults;
using Microsoft.IdentityModel.Tokens;

namespace AirplaneProject.Authorization
{
    public class UserInteractor
    {
        private readonly CustomerDbContext _context;
        public UserInteractor(CustomerDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить пользователя по токену авторизации
        /// </summary>
        /// <param name="authToken">Токен</param>
        public Result<User> GetUser(string? authToken)
        {
            if (JwtToken.ValidateToken(authToken) == false)
                return Result.Fail("Не удалось получить пользователя");

            var claims = JwtToken.ExtractClaims(authToken);
            if (claims.IsNullOrEmpty())
                return Result.Fail("Не удалось получить данные из токена");

            var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.UserRole)?.Value;
            var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.UserId)?.Value;

            if (userId == null)
                return Result.Fail("Не удалось получить данные из токена");

            if (role == null || Enum.Parse<RoleTypes>(role) == RoleTypes.Customer)
            {
                var userFromDb = _context.Customer.FirstOrDefault(c => c.Id == Guid.Parse(userId));
                if (userFromDb == null)
                    return Result.Fail("Не удалось найти пользователя в БД");
                return userFromDb;
            }
            //TODO: впихнуть сюда сотрудника
            var userFromDb2 = _context.Customer.FirstOrDefault(c => c.Id == Guid.Parse(userId));
            if (userFromDb2 == null)
                return Result.Fail("Не удалось найти пользователя в БД");
            return userFromDb2;
        }

        /// <summary>
        /// Получить пользователя по логину/паролю
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        public Result<User> GetUser(string login, string password)
        {
            if (login == string.Empty)
                return Result.Fail("Логин пуст, заполните поле");

            if (password == string.Empty)
                return Result.Fail("Пароль пуст, заполните поле");

            var user = _context.Customer.FirstOrDefault(c => c.Login == login);

            if (user == null || !PasswordHasher.Validate(user.Password, password))
                return Result.Fail("Неправильное имя пользователя или пароль");

            return user;
        }

        /// <summary>
        /// Создать пользователя в БД
        /// </summary>
        /// <param name="user">Пользователь</param>
        public Result<User> CreateUser(CustomerUser user)
        {
            var validationResult = IsUserValidForRegistration(user);
            if (validationResult.IsFailed)
            {
                return validationResult;
            }
            // По хорошему бы разделить эти пароли, но да ладно
            user.Password = PasswordHasher.Hash(user.Password);

            _context.Customer.Add(user);
            _context.SaveChanges();

            return user;
        }

        /// <summary>
        /// Проверка, что пользователь корректно заполнен для регистрации
        /// </summary>
        /// <param name="user">Пользователь</param>
        public Result IsUserValidForRegistration(CustomerUser user)
        {
            var failResult = Result.Ok();
            if (user == null)
                return Result.Fail("Данные пусты");

            if (user.Login == string.Empty)
                failResult = Result.Merge(failResult, Result.Fail("Логин пуст, заполните поле"));

            if (user.Password == string.Empty)
                failResult = Result.Merge(failResult, Result.Fail("Пароль пуст, заполните поле"));

            if (user.Name == string.Empty)
                failResult = Result.Merge(failResult, Result.Fail("ФИО пусто, заполните поле"));

            if (IsPhoneNumberValid(user.PhoneNumber))
                failResult = Result.Merge(failResult, Result.Fail("Номер телефона пользователя должен начинаться с +7 и быть корректной длины"));

            if (_context.Customer.Any(c => c.Login == user.Login))
                failResult = Result.Merge(failResult, Result.Fail("Пользователь с таким логином уже существует, выберите другой"));

            return failResult;
        }

        /// <summary>
        /// Валидация корректности номера телефона
        /// </summary>
        /// <param name="phoneNumber">Номер телефона</param>
        private bool IsPhoneNumberValid(string phoneNumber)
        {
            return phoneNumber == string.Empty ||
                !(phoneNumber.StartsWith("+7") && phoneNumber.Length == 12 ||
                phoneNumber.StartsWith("8") && phoneNumber.Length == 12);
        }
    }
}
