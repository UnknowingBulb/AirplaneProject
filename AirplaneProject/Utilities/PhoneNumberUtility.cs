using FluentResults;
using Microsoft.IdentityModel.Tokens;

namespace AirplaneProject.Utilities
{
    public class PhoneNumberUtility
    {
        /// <summary>
        /// Попытка конвертации номера телефона в формат +7...
        /// </summary>
        /// <param name="phoneNumber">Номер телефона</param>
        public static Result<string> TryConvertToPlusSeven(string phoneNumber)
        {
            if (phoneNumber.IsNullOrEmpty())
                return Result.Fail("Номер пуст");

            if (phoneNumber.StartsWith("+7") && phoneNumber.Length == 12)
                return Result.Ok(phoneNumber);
            else if ((phoneNumber.StartsWith("8") || phoneNumber.StartsWith("7")) && phoneNumber.Length == 11)
            {
                var correctedPhone = string.Concat("+7", phoneNumber.AsSpan(1));
                return Result.Ok(correctedPhone);
            }

            return Result.Fail("Номер телефона пользователя должен начинаться с +7 или 8 и быть корректной длины");
        }
    }
}
