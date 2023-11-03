using AiplaneProject.Objects;
using AirplaneProject.Authorization;
using AirplaneProject.Database.DatabaseContextes;
using AirplaneProject.Interacotrs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirplaneProject.Pages
{
    public class IndexModel : AuthOnPage
    {
        private readonly UserDbContext _userDbContext;
        private readonly FlightInteractor _flightInteractor;
        public IndexModel(UserDbContext userDbContext, FlightInteractor flightInteractor, UserInteractor authorizationInteractor) : base(authorizationInteractor)
        {
            _userDbContext = userDbContext;
            _flightInteractor = flightInteractor;
        } 

        public IList<User>? Users { get; set; }
        public IList<Flight>? Flights { get; set; }

        public async Task OnGetAsync()
        {
            Users = await _userDbContext.User.ToListAsync();
            Flights = _flightInteractor.GetUpcomingFlights().ToList();
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