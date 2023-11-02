using Microsoft.AspNetCore.Mvc;
using AiplaneProject.Objects;
using AirplaneProject.Authorization;

namespace AirplaneProject.Pages
{
    public class CreateModel : AuthOnPage
    {
        private readonly UserInteractor _userInteractor;

        public CreateModel(UserInteractor userInteractor) : base(userInteractor)
        {
            _userInteractor = userInteractor;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User? User { get; set; }

        /// <summary>
        /// Регистрация
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostRegistration()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userResult = _userInteractor.CreateUser(User);

            if (userResult.IsFailed)
            {
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}