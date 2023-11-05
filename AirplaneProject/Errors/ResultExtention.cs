using FluentResults;
using System.Text;

namespace AirplaneProject.Errors
{
    public static class ResultExtention
    {
        /// <summary>
        /// Получить строку со всеми ошибками из результата
        /// </summary>
        public static string GetResultErrorMessages<T>(this Result<T> result)
        {
            var errorMessages = new StringBuilder();
            foreach (var error in result.Errors)
            {
                errorMessages.AppendLine(error.Message);
            }
            return errorMessages.ToString();
        }
    }
}
