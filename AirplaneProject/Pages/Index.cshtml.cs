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
        private readonly UserDbContext _userDbContext;
        private readonly OrderDbContext _orderDbContext;
        public IndexModel(UserDbContext userDbContext, OrderDbContext orderDbContext, UserInteractor authorizationInteractor) : base(authorizationInteractor)
        {
            _userDbContext = userDbContext;
            _orderDbContext = orderDbContext;
        } 

        public IList<User>? Users { get; set; }

        public async Task OnGetAsync()
        {
            Users = await _userDbContext.User.ToListAsync();
            var k = await _orderDbContext.Order.Include(x => x.SeatReserves).ToListAsync();
            var b = k[0].SeatReserves;
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var contact = await _userDbContext.User.FindAsync(id);

            if (contact != null)
            {
                _userDbContext.User.Remove(contact);
                await _userDbContext.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}