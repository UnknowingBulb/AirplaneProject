using AiplaneProject.Objects;
using AirplaneProject.Authorization;
using AirplaneProject.Interactors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AirplaneProject.Pages
{
    public class IndexModel : AuthOnPage
    {
        private readonly FlightInteractor _flightInteractor;
        private readonly OrderInteractor _orderInteractor;
        public IndexModel(FlightInteractor flightInteractor, OrderInteractor orderInteractor, UserInteractor authorizationInteractor) : base(authorizationInteractor)
        {
            _flightInteractor = flightInteractor;
            _orderInteractor = orderInteractor;
        } 

        /// <summary>
        /// Список ближайших полетов
        /// </summary>
        public IList<Flight> Flights { get; set; }
        /// <summary>
        /// Список заказов текущего пользователя
        /// </summary>
        public IList<Order> ActiveUserOrders { get; set; }

        public async Task OnGetAsync()
        {
            Flights = await _flightInteractor.GetUpcomingFlights().Take(20).ToListAsync();
            if(ActiveUser != null)
            {
                var userOrders = _orderInteractor.GetOrdersByUser(ActiveUser.Id);
                if (userOrders.IsSuccess && !userOrders.Value.IsNullOrEmpty())
                {
                    ActiveUserOrders = await userOrders.Value.ToListAsync();
                }
            }
        }

        public async Task<IActionResult> OnPostBuyAsync(int id)
        {
            //TODO: заполнить переходом к странице покупки

            return RedirectToPage();
        }
    }
}