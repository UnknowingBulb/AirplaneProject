using AirplaneProject.Objects;
using AirplaneProject.Interactors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AirplaneProject.Pages
{
    public class IndexModel : AuthOnPage
    {
        private readonly Interactors.FlightInteractor _flightInteractor;
        private readonly Interactors.OrderInteractor _orderInteractor;
        public IndexModel(Interactors.FlightInteractor flightInteractor, Interactors.OrderInteractor orderInteractor, Interactors.UserInteractor authorizationInteractor) : base(authorizationInteractor)
        {
            _flightInteractor = flightInteractor;
            _orderInteractor = orderInteractor;
        } 

        /// <summary>
        /// Список ближайших полетов
        /// </summary>
        public List<Objects.Flight> Flights { get; set; }
        /// <summary>
        /// Список заказов текущего пользователя
        /// </summary>
        public List<Objects.Order> ActiveUserOrders { get; set; }

        public async Task OnGetAsync()
        {
            Flights = (await _flightInteractor.GetUpcomingFlightsAsync()).Take(20).ToList();
            if(ActiveUser != null)
            {
                var userOrders = await _orderInteractor.GetOrdersByUserAsync(ActiveUser.Id);
                if (userOrders.IsSuccess && !userOrders.Value.IsNullOrEmpty())
                {
                    ActiveUserOrders = userOrders.Value;
                }
            }
        }

        public IActionResult OnPostBuyAsync(Guid flightId)
        {
            return RedirectToPage("TicketAcquire", new {FlightId = flightId });
        }
    }
}