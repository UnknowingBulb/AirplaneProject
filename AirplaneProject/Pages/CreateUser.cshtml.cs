using Microsoft.AspNetCore.Mvc;
using AiplaneProject.Objects;
using AirplaneProject.Authorization;
using FluentResults;
using System.Text;

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
        public async Task<IActionResult> OnPostRegistrationAsync()
        {
            var userResult = await _userInteractor.CreateUserAsync(User!);

            if (userResult.IsFailed)
            {
                //TODO: вынести куда-нибудь в другое место
                var errorMessages = new StringBuilder();
                foreach (var error in userResult.Errors)
                {
                    errorMessages.AppendLine(error.Message);
                }
                ModelState.AddModelError("RegistrationError", errorMessages.ToString());
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}