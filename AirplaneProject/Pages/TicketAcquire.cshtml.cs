using AiplaneProject.Objects;
using AirplaneProject.Authorization;
using AirplaneProject.Interactors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;

namespace AirplaneProject.Pages
{
    [Authorize]
    public class TicketAcquireModel : AuthOnPage
    {
        private readonly FlightInteractor _flightInteractor;
        private readonly OrderInteractor _orderInteractor;
        private readonly PassengerInteractor _passengerInteractor;
        public TicketAcquireModel(FlightInteractor flightInteractor, OrderInteractor orderInteractor, PassengerInteractor passengerInteractor, UserInteractor authorizationInteractor) : base(authorizationInteractor)
        {
            _flightInteractor = flightInteractor;
            _orderInteractor = orderInteractor;
            _passengerInteractor = passengerInteractor;
        }

        /// <summary>
        /// Рейс, на который оформляем билет
        /// </summary>
        public Flight Flight { get; set; }

        /// <summary>
        /// Пассажиры, созданные пользователем
        /// </summary>
        public List<Passenger> UserPassengers { get; set; }

        /// <summary>
        /// Ошибки открытия страницы
        /// </summary>
        public string Error { get; set; } = string.Empty;

        /// <summary>
        /// Номера еще свободных мест
        /// </summary>
        public List<int> EmptySeatNumbers { get; set; }

        public async Task OnGet(Guid flightId)
        {
            var flightResult = await _flightInteractor.GetAsync(flightId);
            if (flightResult.IsFailed)
            {
                Error = TicketAcquireErrors.FlightInfoError;
                return;
            }

            Flight = flightResult.Value;

            var emptySeatsResult = await _flightInteractor.GetEmptySeatNumbers(flightId);
            if(emptySeatsResult.IsFailed)
            {
                Error = TicketAcquireErrors.SeatsInfoError;
                return;
            }
            if (emptySeatsResult.Value.IsNullOrEmpty())
            {
                Error = TicketAcquireErrors.NoSeatsError;
                return;
            }

            EmptySeatNumbers = emptySeatsResult.Value;

            UserPassengers = await _passengerInteractor.GetUserPassengers(ActiveUser.Id);
        }
    }

    struct TicketAcquireErrors
    {
        public const string FlightInfoError = "Ошибка при получении данных о рейсе";
        public const string SeatsInfoError = "Ошибка при получении данных о свободных местах";
        public const string NoSeatsError = "К сожалению, на рейс не осталось билетов. Посмотрите другие рейсы";
    }
}
