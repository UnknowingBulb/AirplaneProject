using AirplaneProject.Database.DbData;
using FluentResults;
using Microsoft.IdentityModel.Tokens;
using AirplaneProject.Infrastructure.Authorization;
using AirplaneProject.Infrastructure.Database;
using AirplaneProject.Domain.Entities;
using AirplaneProject.Application.Utilities;

namespace AirplaneProject.Application.Interactors
{
    public class UserInteractor
    {
        private readonly UserDb _userDb;
        public UserInteractor(ApplicationDbContext dbContext)
        {
            _userDb = new UserDb(dbContext);
        }

        /// <summary>
        /// Получить пользователя по токену авторизации
        /// </summary>
        /// <param name="authToken">Токен</param>
        public async Task<Result<User>> GetAsync(string? authToken)
        {
            if (JwtToken.ValidateToken(authToken) == false)
                return Result.Fail("Не удалось получить пользователя");

            var claims = JwtToken.ExtractClaims(authToken);
            if (claims.IsNullOrEmpty())
                return Result.Fail("Не удалось получить данные из токена");

            var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.UserId)?.Value;

            if (userId == null)
                return Result.Fail("Не удалось получить данные из токена");

            var userFromDb = await _userDb.GetAsync(userId);
            if (userFromDb == null)
                return Result.Fail("Не удалось найти пользователя в БД");
            return userFromDb;
        }

        /// <summary>
        /// Получить пользователя по логину/паролю
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        public async Task<Result<User>> GetAsync(string login, string password)
        {
            if (login == string.Empty)
                return Result.Fail("Логин пуст, заполните поле");

            if (password == string.Empty)
                return Result.Fail("Пароль пуст, заполните поле");

            var customerUser = await _userDb.GetByLoginAsync(login);

            if (customerUser == null || !PasswordHasher.Validate(customerUser.Password, password))
                return Result.Fail("Неправильное имя пользователя или пароль");

            return customerUser;
        }

        /// <summary>
        /// Создать пользователя с сохранением в БД
        /// </summary>
        /// <param name="user">Пользователь</param>
        public async Task<Result<User>> CreateAndSaveAsync(User user)
        {
            var validationResult = await ValidateUserForRegistrationAsync(user);
            if (validationResult.IsFailed)
            {
                return validationResult;
            }
            // По хорошему бы разделить эти пароли, но да ладно
            user.Password = PasswordHasher.Hash(user.Password);
            user.IsEmployee = false;

            await _userDb.SaveAsync(user);

            return user;
        }

        /// <summary>
        /// Проверка, что пользователь корректно заполнен для регистрации
        /// </summary>
        /// <param name="user">Пользователь</param>
        private async Task<Result> ValidateUserForRegistrationAsync(User user)
        {
            var result = Result.Ok();
            if (user == null)
                return Result.Fail("Данные пусты. Произошла ошибка. Попробуйте позже");

            if (user.Login.IsNullOrEmpty())
                result = Result.Merge(result, Result.Fail("Логин пуст, заполните поле"));

            if (user.Password.IsNullOrEmpty())
                result = Result.Merge(result, Result.Fail("Пароль пуст, заполните поле"));

            if (user.Name.IsNullOrEmpty())
                result = Result.Merge(result, Result.Fail("ФИО пусто, заполните поле"));

            var phoneNumberResult = PhoneNumberUtility.TryConvertToPlusSeven(user.PhoneNumber);

            if (phoneNumberResult.IsSuccess)
                user.PhoneNumber = phoneNumberResult.Value;
            else
                result = Result.Merge(result, Result.Fail(phoneNumberResult.GetResultErrorMessages()));

            if (await _userDb.IsAnyWithSameLoginAsync(user.Login))
                result = Result.Merge(result, Result.Fail("Пользователь с таким логином уже существует, выберите другой"));

            return result;
        }
    }
}
