using Microsoft.AspNetCore.Mvc.RazorPages;
using AiplaneProject.Objects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using AirplaneProject.Authorization;

namespace AirplaneProject.Pages
{
    public abstract class AuthOnPage : PageModel
    {
        private readonly UserInteractor _userInteractor;
        private string? _authToken;
        private User? _activeUser;

        public AuthOnPage(UserInteractor userInteractor)
        {
            _userInteractor = userInteractor;
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
                if (_authToken == null)
                {
                    _authToken = Request?.Headers.Authorization.ToString().Replace("Bearer ", "");
                }
                if (_activeUser == null)
                {
                    var userResult = _userInteractor.GetUser(_authToken);
                    if (userResult.IsSuccess)
                    {
                        _activeUser = userResult.Value;
                    }
                }
                return _activeUser;
            }
        }

        /// <summary>
        /// Логин
        /// </summary>
        public async Task<IActionResult> OnPostLogin()
        {
            if (Login.IsNullOrEmpty() || Password.IsNullOrEmpty())
            {
                return Page();
            }

            var userResult = _userInteractor.GetUser(Login, Password);

            if (userResult.IsFailed)
            {
                return Page();
            }

            _activeUser = userResult.Value;
            _authToken = JwtToken.GenerateToken(_activeUser.Id, RoleTypes.Customer);
            Response.Cookies.Append("authToken", _authToken);

            return RedirectToPage("./Index");
        }

        /// <summary>
        /// Разлогин
        /// </summary>
        public async Task<IActionResult> OnPostLogout()
        {
            Response.Cookies.Delete("authToken");
            _authToken = null;
            _activeUser = null;

            return RedirectToPage("./Index");
        }
    }
}