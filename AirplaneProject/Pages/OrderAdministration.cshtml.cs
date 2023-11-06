using AirplaneProject.Objects;
using AirplaneProject.Interactors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace AirplaneProject.Pages
{
    [Authorize]
    public class OrderAdministrationModel : AuthOnPage
    {
        private readonly OrderInteractor _orderInteractor;
        public OrderAdministrationModel(OrderInteractor orderInteractor, UserInteractor authorizationInteractor) : base(authorizationInteractor)
        {
            _orderInteractor = orderInteractor;
        }

        /// <summary>
        /// Список всех заказов
        /// </summary>
        public List<Order> Orders { get; set; }

        /// <summary>
        /// Номер телефона клиента, по которому будем искать заказы
        /// </summary>
        public string SearchClientPhoneNumber { get; set; }

        /// <summary>
        /// ФИО клиента, по которому будем искать заказы
        /// </summary>
        public string SearchClientName { get; set; }

        public async Task OnGetAsync()
        {
            Orders = await _orderInteractor.GetOrdersAsync();
        }

        public void OnGetByPhoneAsync()
        {
            Orders = Orders.Where(o=> o.User.PhoneNumber == SearchClientPhoneNumber).ToList();
        }

        public void OnGetByNameAsync()
        {
            Orders = Orders.Where(o => o.User.Name == SearchClientName).ToList();
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