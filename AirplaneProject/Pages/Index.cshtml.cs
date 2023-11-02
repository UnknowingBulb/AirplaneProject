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
        private readonly CustomerDbContext _context;
        public IndexModel(CustomerDbContext context, UserInteractor authorizationInteractor) : base(authorizationInteractor)
        {
            _context = context;
        } 

        public IList<CustomerUser>? Customers { get; set; }

        public async Task OnGetAsync()
        {
            Customers = await _context.Customer.ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var contact = await _context.Customer.FindAsync(id);

            if (contact != null)
            {
                _context.Customer.Remove(contact);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}