using AirplaneProject.Objects;
using AirplaneProject.Authorization;
using AirplaneProject.Interactors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
        [BindProperty]
        public Flight Flight { get; set; }

        /// <summary>
        /// Пассажиры, созданные пользователем
        /// </summary>
        [BindProperty]
        public List<Passenger> UserPassengers { get; set; }

        /// <summary>
        /// Ошибки открытия страницы
        /// </summary>
        public string Error { get; set; } = string.Empty;

        /// <summary>
        /// Номера еще свободных мест
        /// </summary>
        public List<int> EmptySeatNumbers { get; set; }

        /// <summary>
        /// Список билетов, которые берем на рейс
        /// </summary>
        [BindProperty]
        public List<SeatReserve> SeatReserves { get; set; } = new List<SeatReserve>();

        public async Task OnGetAsync(Guid flightId)
        {
            var flightResult = await _flightInteractor.GetAsync(flightId);
            if (flightResult.IsFailed)
            {
                Error = TicketAcquireErrors.FlightInfoError;
                return;
            }

            Flight = flightResult.Value;

            var emptySeatsResult = _flightInteractor.GetEmptySeatNumbers(Flight);
            if (emptySeatsResult.IsNullOrEmpty())
            {
                Error = TicketAcquireErrors.NoSeatsError;
                return;
            }

            EmptySeatNumbers = emptySeatsResult.ToList();

            UserPassengers = await _passengerInteractor.GetUserPassengers(ActiveUser.Id);
        }

        public async Task OnPostAddTicketAsync(Guid flightId)
        {
            await OnGetAsync(flightId);
            if (!Error.IsNullOrEmpty())
                return;

            var seatReserve = new SeatReserve();
            if (UserPassengers.IsNullOrEmpty())
            {
                seatReserve.Passenger = new Passenger
                {
                    Id = Guid.NewGuid(),
                    Name = ActiveUser.Name,
                    PassportData = string.Empty,
                    UserId = ActiveUser.Id,
                };
            }
            else
            {
                seatReserve.Passenger = UserPassengers[0];
            }

            seatReserve.SeatNumber = EmptySeatNumbers.First();

            SeatReserves.Add(seatReserve);
        }

        public async Task OnPostCompleteOrderAsync(Guid flightId)
        {
            await OnGetAsync(flightId);
            var order = new Order()
            {
                Id = Guid.NewGuid(),
                Flight = Flight,
                UserId = ActiveUser.Id,
                Price =Flight.Price * SeatReserves.Count,
                SeatReserves = SeatReserves,
                IsActive = true

            };
            var orderResult = await _orderInteractor.CreateAsync(order);
            if (orderResult.IsFailed)
            {
                Error = $"Ошибка оформления заказа: {orderResult.Errors[0].Message}";
                return;
            }
        }
    }

    struct TicketAcquireErrors
    {
        public const string FlightInfoError = "Ошибка при получении данных о рейсе";
        public const string NoSeatsError = "К сожалению, на рейс не осталось билетов. Посмотрите другие рейсы";
    }
}
