using Microsoft.AspNetCore.Mvc.RazorPages;
using AiplaneProject.Models;
using AirplaneProject.Authorization;

namespace AirplaneProject.Pages
{
    public abstract class AuthOnPage : PageModel
    {
        private readonly AuthorizationInteractor _authorizationInteractor;
        private readonly string? _authToken;
        private User? _activeUser;

        public AuthOnPage(AuthorizationInteractor authorizationInteractor)
        {
            _authorizationInteractor = authorizationInteractor;
            _authToken = this.Request?.Headers?.Authorization;
        }

        public bool IsAuthorized
        {
            get
            {
                return ActiveUser != null;
            }
        }

        public string? ActiveUserName
        {
            get
            {
                return ActiveUser?.Name;
            }
        }

        public User? ActiveUser
        {
            get
            {
                if (_activeUser == null)
                {
                    _activeUser = _authorizationInteractor.GetUser(_authToken);
                }
                return _activeUser;
            }
        }
    }
}