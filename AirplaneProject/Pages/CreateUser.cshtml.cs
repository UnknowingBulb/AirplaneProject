using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AirplaneProject.Database.DatabaseContextes;
using AiplaneProject.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
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
        public CustomerUser? CustomerUser { get; set; }

        public IActionResult OnPostRegistration()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userResult = _userInteractor.CreateUser(CustomerUser);

            if (userResult.IsFailed)
            {
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}