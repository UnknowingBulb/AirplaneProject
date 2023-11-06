using AirplaneProject.Objects;
using AirplaneProject.Interactors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using AirplaneProject.Errors;
using AirplaneProject.Database.DbData;
using AirplaneProject.ViewModels;

namespace AirplaneProject.Pages
{
    [Authorize]
    public class TicketAcquireModel : AuthOnPage
    {
        private readonly Interactors.FlightInteractor _flightInteractor;
        private readonly Interactors.OrderInteractor _orderInteractor;
        private readonly Interactors.PassengerInteractor _passengerInteractor;
        public TicketAcquireModel(Interactors.FlightInteractor flightInteractor, Interactors.OrderInteractor orderInteractor, Interactors.PassengerInteractor passengerInteractor, Interactors.UserInteractor userInteractor) : base(userInteractor)
        {
            _flightInteractor = flightInteractor;
            _orderInteractor = orderInteractor;
            _passengerInteractor = passengerInteractor;
        }

        /// <summary>
        /// Рейс, на который оформляем билет
        /// </summary>
        [BindProperty]
        public Objects.Flight Flight { get; set; }

        /// <summary>
        /// Пассажиры, созданные пользователем
        /// </summary>
        [BindProperty]
        public List<PassengerView> UserPassengers { get; set; } = new List<PassengerView>();

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
        public List<SeatReserveView> SeatReserves { get; set; } = new List<SeatReserveView>();

        //TODO: добавить везде в асинк cancellation
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
        }

        public async Task OnPostAddTicketAsync(Guid flightId)
        {
            await OnGetAsync(flightId);

            if (!Error.IsNullOrEmpty())
                return;

            if (UserPassengers.IsNullOrEmpty())
            {
                UserPassengers = PassengerView.FromPassengerList(await _passengerInteractor.GetUserPassengersAsync(ActiveUser!.Id));
                var newEmptyPassenger = new PassengerView(ActiveUser.Id, ActiveUser!.Name, string.Empty);
                UserPassengers.Insert(0, newEmptyPassenger);
            }

            var newSeatReserve = new SeatReserveView()
            {
                Id = Guid.NewGuid(),
                SeatNumber = EmptySeatNumbers.First(),
                Passenger = UserPassengers[0],
                PassengerId = UserPassengers[0].Id
            };

            SeatReserves.Add(newSeatReserve);
        }

        public async Task OnPostChangePassengerAsync(Guid flightId)
        {
            await OnGetAsync(flightId);

            if (!Error.IsNullOrEmpty())
                return;
        }

        public async Task<IActionResult> OnPostCompleteOrderAsync(Guid flightId)
        {
            var flight = await _flightInteractor.GetAsync(flightId);
            if (flight.IsFailed)
            {
                Error = $"Ошибка оформления заказа: {flight.GetResultErrorMessages()}";
                return Page();
            }


            foreach (var seatReserve in SeatReserves)
            {
                if (seatReserve.PassengerId != Guid.Empty)
                {
                    seatReserve.Passenger = UserPassengers.First(p => p.Id == seatReserve.PassengerId);
                }
            }
            var order = new Objects.Order()
            {
                Id = Guid.NewGuid(),
                Flight = flight.Value,
                UserId = ActiveUser!.Id,
                Price = flight.Value.Price * SeatReserves.Count,
                SeatReserves = SeatReserves.Select(r => r.ToSeatReserve(ActiveUser.Id)).ToList(),
                IsActive = true
            };

            var orderResult = await _orderInteractor.CreateAsync(order);
            if (orderResult.IsFailed)
            {
                Error = $"Ошибка оформления заказа: {orderResult.GetResultErrorMessages()}";
                return Page();
            }
            return RedirectToPage("./Index");
        }
    }

    struct TicketAcquireErrors
    {
        public const string FlightInfoError = "Ошибка при получении данных о рейсе";
        public const string NoSeatsError = "К сожалению, на рейс не осталось билетов. Посмотрите другие рейсы";
    }
}
