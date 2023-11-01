using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AirplaneProject.Database.DatabaseContextes;
using AiplaneProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AirplaneProject.Pages
{
    public class CreateModel : PageModel
    {
        private readonly CustomerDbContext _context;

        public CreateModel(CustomerDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public CustomerUser? CustomerUser { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            //var contextOptions = new DbContextOptionsBuilder<CustomerDbContext>().Options;
            using (var context = new CustomerDbContext())
            {
                if (!ModelState.IsValid)
            {
                return Page();
            }

            if (CustomerUser != null) _context.Customer.Add(CustomerUser);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
            }
        }
    }
}