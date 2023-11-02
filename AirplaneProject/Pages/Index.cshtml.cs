using AiplaneProject.Objects;
using AirplaneProject.Authorization;
using AirplaneProject.Database.DatabaseContextes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AirplaneProject.Pages
{
    public class IndexModel : AuthOnPage
    {
        private readonly UserDbContext _context;
        public IndexModel(UserDbContext context, UserInteractor authorizationInteractor) : base(authorizationInteractor)
        {
            _context = context;
        } 

        public IList<User>? Users { get; set; }

        public async Task OnGetAsync()
        {
            Users = await _context.Users.ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var contact = await _context.Users.FindAsync(id);

            if (contact != null)
            {
                _context.Users.Remove(contact);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}