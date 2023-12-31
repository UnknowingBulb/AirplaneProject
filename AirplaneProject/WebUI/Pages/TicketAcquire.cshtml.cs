using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using AirplaneProject.Infrastructure.Authorization;
using AirplaneProject.Domain.Entities;
using AirplaneProject.WebUI.ViewModels;
using AirplaneProject.Application.Interactors;
using AirplaneProject.Application.Utilities;
using AirplaneProject.WebUI.Pages.Shared;

namespace AirplaneProject.Pages
{
    [Authorize(Roles = RoleTypes.Customer)]
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
        public Flight Flight { get; set; }

        /// <summary>
        /// ���������, ��������� �������������
        /// </summary>
        [BindProperty]
        public List<PassengerView> UserPassengers { get; set; } = new List<PassengerView>();

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
        public List<SeatReserveView> SeatReserves { get; set; } = new List<SeatReserveView>();

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
            await OnGetAsync(flightId);

            if (!Error.IsNullOrEmpty())
                return Page();

            foreach (var seatReserve in SeatReserves)
            {
                if (seatReserve.PassengerId != Guid.Empty)
                {
                    seatReserve.Passenger = UserPassengers.First(p => p.Id == seatReserve.PassengerId);
                }
            }
            var order = new Order()
            {
                Id = Guid.NewGuid(),
                Flight = Flight,
                UserId = ActiveUser!.Id,
                Price = Flight.Price * SeatReserves.Count,
                SeatReserves = SeatReserves.Select(r => r.ToSeatReserve(ActiveUser.Id)).ToList(),
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
