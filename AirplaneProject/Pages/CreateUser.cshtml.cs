using Microsoft.AspNetCore.Mvc;
using AirplaneProject.Objects;
using AirplaneProject.Errors;
using AirplaneProject.Interactors;

namespace AirplaneProject.Pages
{
    public class CreateModel : AuthOnPage
    {
        private readonly Interactors.UserInteractor _userInteractor;

        public CreateModel(Interactors.UserInteractor userInteractor) : base(userInteractor)
        {
            _userInteractor = userInteractor;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Objects.User? CreatedUser { get; set; }

        /// <summary>
        /// Регистрация
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostRegistrationAsync()
        {
            var userResult = await _userInteractor.CreateAndSaveAsync(CreatedUser!);

            if (userResult.IsFailed)
            {
                ModelState.AddModelError("RegistrationError", userResult.GetResultErrorMessages());
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}