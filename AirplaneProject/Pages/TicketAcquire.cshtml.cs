using AirplaneProject.Objects;
using AirplaneProject.Interactors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using AirplaneProject.Errors;
using AirplaneProject.Database.DbData;

namespace AirplaneProject.Pages
{
    [Authorize]
    public class TicketAcquireModel : AuthOnPage
    {
        private readonly Interactors.Flight _flightInteractor;
        private readonly Interactors.Order _orderInteractor;
        private readonly Interactors.Passenger _passengerInteractor;
        public TicketAcquireModel(Interactors.Flight flightInteractor, Interactors.Order orderInteractor, Interactors.Passenger passengerInteractor, Interactors.User userInteractor) : base(userInteractor)
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
        public List<Objects.Passenger> UserPassengers { get; set; } = new List<Objects.Passenger>();

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
                UserPassengers = await _passengerInteractor.GetUserPassengersAsync(ActiveUser!.Id);
                var newEmptyPassenger = new Objects.Passenger
                {
                    Id = Guid.Empty,
                    Name = ActiveUser!.Name,
                    PassportData = string.Empty,
                    UserId = ActiveUser.Id,
                };
                await _passengerInteractor.CreateAsync(newEmptyPassenger);
                UserPassengers.Insert(0, newEmptyPassenger);
            }

            var seatReserve = new SeatReserve()
            {
                Id = Guid.NewGuid(),
                SeatNumber = EmptySeatNumbers.First(),
                PassengerId = UserPassengers[0].Id
            };

            SeatReserves.Add(seatReserve);

            await _orderInteractor.CreateSeatReserveAsync(SeatReserves);
        }

        public async Task OnPostChangePassengerAsync(Guid flightId)
        {
            //Просто инициируем обновление страницы, ничего такого
            await OnGetAsync(flightId);

            if (!Error.IsNullOrEmpty())
                return;

            if (UserPassengers.IsNullOrEmpty())
            {
                UserPassengers = await _passengerInteractor.GetUserPassengersAsync(ActiveUser!.Id);
                var newEmptyPassenger = new Objects.Passenger
                {
                    Id = Guid.Empty,
                    Name = ActiveUser!.Name,
                    PassportData = string.Empty,
                    UserId = ActiveUser.Id,
                };
                await _passengerInteractor.CreateAsync(newEmptyPassenger);
                UserPassengers.Insert(0, newEmptyPassenger);
            }
            await _orderInteractor.CreateSeatReserveAsync(SeatReserves);
        }

        public async Task<IActionResult> OnPostCompleteOrderAsync(Guid flightId)
        {
            var flight = await _flightInteractor.GetAsync(flightId);
            if (flight.IsFailed)
            {
                Error = $"Ошибка оформления заказа: {flight.GetResultErrorMessages()}";
                return Page();
            }

            var order = new Objects.Order()
            {
                Id = Guid.NewGuid(),
                Flight = flight.Value,
                UserId = ActiveUser!.Id,
                Price = flight.Value.Price * SeatReserves.Count,
                SeatReserves = SeatReserves,
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
