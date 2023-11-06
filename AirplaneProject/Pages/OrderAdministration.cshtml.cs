using AirplaneProject.Objects;
using AirplaneProject.Interactors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AirplaneProject.Utilities;
using AirplaneProject.Authorization;
using AirplaneProject.Pages.Shared;

namespace AirplaneProject.Pages
{
    [Authorize(Roles = RoleTypes.Employee)]
    public class OrderAdministrationModel : AuthOnPage
    {
        private readonly OrderInteractor _orderInteractor;
        public OrderAdministrationModel(OrderInteractor orderInteractor, UserInteractor userInteractor) : base(userInteractor)
        {
            _orderInteractor = orderInteractor;
        }

        /// <summary>
        /// Список всех заказов
        /// </summary>
        public List<Order> Orders { get; set; } = new List<Order>();

        /// <summary>
        /// Номер телефона клиента, по которому будем искать заказы
        /// </summary>
        [BindProperty]
        public string SearchClientPhoneNumber { get; set; }

        /// <summary>
        /// ФИО клиента, по которому будем искать заказы
        /// </summary>
        [BindProperty]
        public string SearchClientName { get; set; }

        public async Task OnGetAsync()
        {
            Orders = await _orderInteractor.GetOrdersAsync();
        }

        public async Task OnPostApplyNameFilterAsync()
        {
            var ordersResult = await _orderInteractor.GetOrdersByUserNameAsync(SearchClientName);
            if (ordersResult.IsSuccess)
                Orders = ordersResult.Value;
            else
            {
                ModelState.AddModelError("FilterError", ordersResult.GetResultErrorMessages());
            }
        }

        public async Task OnPostApplyPhoneFilterAsync()
        {
            var ordersResult = await _orderInteractor.GetOrdersByPhoneAsync(SearchClientPhoneNumber);
            if (ordersResult.IsSuccess)
                Orders = ordersResult.Value;
            else
            {
                ModelState.AddModelError("FilterError", ordersResult.GetResultErrorMessages());
            }
        }

        public async Task OnPostResetFiltersAsync()
        {
            Orders = await _orderInteractor.GetOrdersAsync();
        }

        public async Task OnPostCancelAsync(Guid orderId)
        {
            var orderResult = await _orderInteractor.GetAsync(orderId);

            // TODO: тут могли бы быть какие-нибудь сообщения об ошибке
            if (orderResult.IsFailed)
                return;

            await _orderInteractor.SetNotActiveAsync(orderResult.Value);
            await OnGetAsync();
        }
    }
}