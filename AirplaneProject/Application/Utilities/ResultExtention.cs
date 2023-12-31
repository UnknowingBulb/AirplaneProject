﻿using FluentResults;
using System.Text;

namespace AirplaneProject.Application.Utilities
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

        /// <summary>
        /// Получить строку со всеми ошибками из результата
        /// </summary>
        public static string GetResultErrorMessages(this Result result)
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
