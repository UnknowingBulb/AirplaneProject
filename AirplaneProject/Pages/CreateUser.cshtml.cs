using Microsoft.AspNetCore.Mvc;
using AirplaneProject.Objects;
using AirplaneProject.Authorization;
using AirplaneProject.Errors;

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
        public UserModel? CreatedUser { get; set; }

        /// <summary>
        /// Регистрация
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostRegistrationAsync()
        {
            var userResult = await _userInteractor.CreateUserAsync(CreatedUser!);

            if (userResult.IsFailed)
            {
                ModelState.AddModelError("RegistrationError", userResult.GetResultErrorMessages());
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}