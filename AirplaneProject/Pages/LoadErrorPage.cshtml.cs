using AirplaneProject.Application.Interactors;
using AirplaneProject.WebUI.Pages.Shared;
using System.Net;

namespace AirplaneProject.WebUI.Pages
{
    public class LoadErrorPage : AuthOnPage
    {
        public string Error { get; set; } = string.Empty;

        public LoadErrorPage(UserInteractor userInteractor) : base(userInteractor)
        {
        }

        public void OnGet(int statusCode)
        {
            switch (statusCode)
            {
                case (int)HttpStatusCode.Unauthorized:
                    Error = "Авторизуйтесь и повторите действие";
                    return;

                case (int)HttpStatusCode.Forbidden:
                    Error = "Недоступно текущему пользователю";
                    return;

                case (int)HttpStatusCode.NotFound:
                    Error = "Такой страницы не существует";
                    return;

                default:
                    Error = "Произошла непредвиденная ошибка";
                    return;

            }
        }
    }
}