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
        /// ����, �� ������� ��������� �����
        /// </summary>
        [BindProperty]
        public Flight Flight { get; set; }

        /// <summary>
        /// ���������, ��������� �������������
        /// </summary>
        [BindProperty]
        public List<Passenger> UserPassengers { get; set; }

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
                Error = $"������ ���������� ������: {orderResult.Errors[0].Message}";
                return;
            }
        }
    }

    struct TicketAcquireErrors
    {
        public const string FlightInfoError = "������ ��� ��������� ������ � �����";
        public const string NoSeatsError = "� ���������, �� ���� �� �������� �������. ���������� ������ �����";
    }
}
