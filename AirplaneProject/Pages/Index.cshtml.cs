using AiplaneProject.Objects;
using AirplaneProject.Authorization;
using AirplaneProject.Interacotrs;
using Microsoft.AspNetCore.Mvc;

namespace AirplaneProject.Pages
{
    public class IndexModel : AuthOnPage
    {
        private readonly FlightInteractor _flightInteractor;
        public IndexModel(FlightInteractor flightInteractor, UserInteractor authorizationInteractor) : base(authorizationInteractor)
        {
            _flightInteractor = flightInteractor;
        } 

        public IList<Flight>? Flights { get; set; }

        public void OnGetAsync()
        {
            Flights = _flightInteractor.GetUpcomingFlights().Take(20).ToList();
        }

        public async Task<IActionResult> OnPostBuyAsync(int id)
        {
            //TODO: заполнить переходом к странице покупки

            return RedirectToPage();
        }
    }
}