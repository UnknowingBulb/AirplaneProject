using Microsoft.AspNetCore.Mvc.RazorPages;
using AiplaneProject.Objects;
using AirplaneProject.Interactors;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace AirplaneProject.Pages
{
    public abstract class AuthOnPage : PageModel
    {
        private readonly UserInteractor _userInteractor;
        private readonly string? _authToken;
        private User? _activeUser;

        public AuthOnPage(UserInteractor userInteractor)
        {
            _userInteractor = userInteractor;
            _authToken = this.Request?.Headers?.Authorization;
        }

        [BindProperty]
        public string? Login { get; set; }
        [BindProperty]
        public string? Password { get; set; }

        /// <summary>
        /// Пользователь авторизован
        /// </summary>
        public bool IsAuthorized
        {
            get
            {
                return ActiveUser != null;
            }
        }

        /// <summary>
        /// Получить ФИО текущего пользователя
        /// </summary>
        public string? ActiveUserName
        {
            get
            {
                return ActiveUser?.Name;
            }
        }

        /// <summary>
        /// Текущий пользователь
        /// </summary>
        public User? ActiveUser
        {
            get
            {
                if (_activeUser == null)
                {
                    var userResult = _userInteractor.GetUser(_authToken);
                    if (userResult.IsSuccess) {
                        _activeUser = userResult.Value;
                    }
                }
                return _activeUser;
            }
        }
    }
}