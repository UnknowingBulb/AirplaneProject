using AirplaneProject.Objects;
using AirplaneProject.Authorization;
using AirplaneProject.Interactors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using AirplaneProject.Errors;

namespace AirplaneProject.Pages
{
    [Authorize]
    public class TicketAcquireModel : AuthOnPage
    {
        private readonly FlightInteractor _flightInteractor;
        private readonly OrderInteractor _orderInteractor;
        private readonly PassengerInteractor _passengerInteractor;
        public TicketAcquireModel(FlightInteractor flightInteractor, OrderInteractor orderInteractor, PassengerInteractor passengerInteractor, UserInteractor userInteractor) : base(userInteractor)
        {
            _flightInteractor = flightInteractor;
            _orderInteractor = orderInteractor;
            _passengerInteractor = passengerInteractor;
        }

        /// <summary>
        /// ����, �� ������� ��������� �����
        /// </summary>
        [BindProperty]
        public FlightModel Flight { get; set; }

        /// <summary>
        /// ���������, ��������� �������������
        /// </summary>
        [BindProperty]
        public List<PassengerModel> UserPassengers { get; set; }

        /// <summary>
        /// ������ �������� ��������
        /// </summary>
        public string Error { get; set; } = string.Empty;

        /// <summary>
        /// ������ ��� ��������� ����
        /// </summary>
        public List<int> EmptySeatNumbers { get; set; }

        /// <summary>
        /// ������ �������, ������� ����� �� ����
        /// </summary>
        [BindProperty]
        public List<SeatReserveModel> SeatReserves { get; set; } = new List<SeatReserveModel>();

        //TODO: �������� ����� � ����� cancellation
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
                var newEmptyPassenger = new PassengerModel
                {
                    Id = Guid.Empty,
                    Name = ActiveUser!.Name,
                    PassportData = string.Empty,
                    UserId = ActiveUser.Id,
                };
                UserPassengers.Insert(0, newEmptyPassenger);
            }

            var seatReserve = new SeatReserveModel()
            {
                Id = Guid.NewGuid(),
                SeatNumber = EmptySeatNumbers.First(),
                Passenger = UserPassengers[0],
                PassengerId = UserPassengers[0].Id
            };

            SeatReserves.Add(seatReserve);
        }

        public async Task OnPostChangePassengerAsync(Guid flightId)
        {
            //������ ���������� ���������� ��������, ������ ������
            await OnGetAsync(flightId);

            if (!Error.IsNullOrEmpty())
                return;

            if (UserPassengers.IsNullOrEmpty())
            {
                UserPassengers = await _passengerInteractor.GetUserPassengersAsync(ActiveUser!.Id);
                var newEmptyPassenger = new PassengerModel
                {
                    Id = Guid.Empty,
                    Name = ActiveUser!.Name,
                    PassportData = string.Empty,
                    UserId = ActiveUser.Id,
                };
                UserPassengers.Insert(0, newEmptyPassenger);
            }
        }

        public async Task<IActionResult> OnPostCompleteOrderAsync(Guid flightId)
        {
            var flight = await _flightInteractor.GetAsync(flightId);
            if (flight.IsFailed)
            {
                Error = $"������ ���������� ������: {flight.GetResultErrorMessages()}";
                return Page();
            }

            var order = new OrderModel()
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
                Error = $"������ ���������� ������: {orderResult.GetResultErrorMessages()}";
                return Page();
            }
            return RedirectToPage("./Index");
        }
    }

    struct TicketAcquireErrors
    {
        public const string FlightInfoError = "������ ��� ��������� ������ � �����";
        public const string NoSeatsError = "� ���������, �� ���� �� �������� �������. ���������� ������ �����";
    }
}
